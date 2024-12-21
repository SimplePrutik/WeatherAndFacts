using System.Collections.Generic;
using JSONProperties.Facts;
using Tabs.Items;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Tabs.ConcreteTabs
{
    public class FactsTab : BaseTab
    {
        [SerializeField] private FactsTabItem itemPrefab;
        [SerializeField] private GameObject modalWindow;
        [SerializeField] private TMP_Text breedTitle;
        [SerializeField] private TMP_Text breedDescription;
        [SerializeField] private Button modalButton;

        private CompositeDisposable disposable = new CompositeDisposable();

        private List<FactsTabItem> items = new List<FactsTabItem>();
        
        private const string BREEDS_URL = "https://dogapi.dog/api/v2/breeds";

        [Inject]
        public void Construct()
        {
            modalButton.OnClickAsObservable()
                .Subscribe(_ => { modalWindow.SetActive(false); })
                .AddTo(this);
        }
        
        public override void Open()
        {
            base.Open();
            modalWindow.SetActive(false);
            webRequestService.SendStringRequest(BREEDS_URL, HandleData);
        }

        public override void Close()
        {
            base.Close();
            modalWindow.SetActive(false);
            items.ForEach(item =>
            {
                Destroy(item.gameObject);
            });
            items.Clear();
            disposable.Dispose();
        }


        private void HandleData(string data)
        {
            var weatherResponse = JsonUtility.FromJson<BreedsResponse>(data);

            tabService.OnHideLoader.Execute();

            var breedCounter = 1;
            foreach (var breedData in weatherResponse.data)
            {
                var item = container.InstantiatePrefabForComponent<FactsTabItem>(itemPrefab, transform);
                item.Init(breedCounter++.ToString(), breedData.attributes.name);
                var breedId = breedData.id;
                item.Button.OnClickAsObservable()
                    .Subscribe(_ =>
                    {
                        item.Loader.SetEnable(true);
                        OpenModal(breedId, item.Loader);
                    })
                    .AddTo(disposable);
                items.Add(item);
            }
            modalWindow.GetComponent<RectTransform>().SetAsLastSibling();
        }

        private void OpenModal(string breedId, Loader loader)
        {
            webRequestService.ClearQueue();
            webRequestService.SendStringRequest(BREEDS_URL + "/" + breedId,
                description =>
                {
                    var breedInfo = JsonUtility.FromJson<ConcreteBreedData>(description);
                    breedDescription.text = breedInfo.data.attributes.description;
                    breedTitle.text = breedInfo.data.attributes.name;
                    modalWindow.SetActive(true);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(modalWindow.GetComponent<RectTransform>());
                    loader.SetEnable(false);
                });
        }
    }
}