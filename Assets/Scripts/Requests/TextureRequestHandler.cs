using System;
using UnityEngine;

namespace Requests
{
    public class TextureRequestHandler : IRequestHandler
    {
        public Action<Texture2D> Callback { get; set; }
        public IObservable<Texture2D> ObservableTexture { get; set; }

        public Type GetDataType()
        {
            return typeof(Texture2D);
        }
    }
}