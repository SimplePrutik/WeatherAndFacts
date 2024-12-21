using System;
using System.Collections.Generic;
using Requests;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

public class WebRequestService
{
    private readonly Queue<IRequestHandler> queue = new Queue<IRequestHandler>();
    private CompositeDisposable requestsDisposable = new CompositeDisposable();
    
    private bool isProcessing = false;

    [Inject]
    public void Construct()
    {
        Observable
            .EveryUpdate()
            .Subscribe(_ => ProcessQueue());
    }

    public void SendStringRequest(string url, Action<string> callback)
    {
        var request = new StringRequestHandler {ObservableString = GetData(url), Callback = callback};
        queue.Enqueue(request);
    }

    public void SendTextureRequest(string url, Action<Texture2D> callback)
    {
        var request = new TextureRequestHandler {ObservableTexture = GetTexture(url), Callback = callback};
        queue.Enqueue(request);
    }

    public void ClearQueue()
    {
        requestsDisposable?.Dispose();
        requestsDisposable = new CompositeDisposable();
        queue.Clear();
    }

    private void ProcessQueue()
    {
        if (isProcessing || queue.Count == 0)
            return;

        isProcessing = true;
        var request = queue.Dequeue();
        if (request.GetDataType() == typeof(string))
        {
            var currentObservable = (StringRequestHandler) request;
            currentObservable.ObservableString
                .Subscribe(data =>
                {
                    currentObservable.Callback.Invoke(data);
                    isProcessing = false;
                })
                .AddTo(requestsDisposable);
        }
        if (request.GetDataType() == typeof(Texture2D))
        {
            var currentObservable = (TextureRequestHandler) request;
            currentObservable.ObservableTexture
                .Subscribe(data =>
                {
                    currentObservable.Callback.Invoke(data);
                    isProcessing = false;
                })
                .AddTo(requestsDisposable);
        }
    }
    
    private IObservable<string> GetData(string url)
    {
        return Observable.Create<string>(observer =>
        {
            var request = UnityWebRequest.Get(url);
            var operation = request.SendWebRequest();
            return Observable.EveryUpdate().Where(_ => operation.isDone).Take(1).Subscribe(_ =>
                {
                    if (request.result == UnityWebRequest.Result.ConnectionError ||
                        request.result == UnityWebRequest.Result.ProtocolError)
                    {
                        observer.OnError(new Exception(request.error));
                    }
                    else
                    {
                        var response = request.downloadHandler.text;
                        observer.OnNext(response);
                        observer.OnCompleted();
                    }

                    request.Dispose();
                })
                .AddTo(requestsDisposable);
        });
    }

    private IObservable<Texture2D> GetTexture(string url)
    {
        return Observable.Create<Texture2D>(observer =>
        {
            var request = UnityWebRequestTexture.GetTexture(url);
            var operation = request.SendWebRequest();
            return Observable.EveryUpdate().Where(_ => operation.isDone).Take(1).Subscribe(_ =>
                {
                    if (request.result == UnityWebRequest.Result.ConnectionError ||
                        request.result == UnityWebRequest.Result.ProtocolError)
                    {
                        observer.OnError(new Exception(request.error));
                    }
                    else
                    {
                        Texture2D texture = DownloadHandlerTexture.GetContent(request);
                        observer.OnNext(texture);
                        observer.OnCompleted();
                    }

                    request.Dispose();
                })
                .AddTo(requestsDisposable);
        });
    }
}