using UniRx;
using UnityEngine;
using Zenject;

namespace Tabs
{
    public class BaseTab : MonoBehaviour
    {
        protected TabService tabService;
        protected WebRequestService webRequestService;
        protected DiContainer container;
        
        [Inject]
        public void Construct(
            TabService tabService,
            WebRequestService webRequestService,
            DiContainer container)
        {
            this.tabService = tabService;
            this.webRequestService = webRequestService;
            this.container = container;
        }
        
        public virtual void Open()
        {
            gameObject.SetActive(true);
        }

        public virtual void Close()
        {
            gameObject.SetActive(false);
            webRequestService.ClearQueue();
        }
    }
}