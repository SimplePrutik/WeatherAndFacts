using System;

namespace Requests
{
    public class StringRequestHandler : IRequestHandler
    {
        public Action<string> Callback { get; set; }
        public IObservable<string> ObservableString { get; set; }

        public Type GetDataType()
        {
            return typeof(string);
        }
    }
}