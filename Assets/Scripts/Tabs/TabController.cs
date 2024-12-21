using System;
using Tabs.ConcreteTabs;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Tabs
{
    public class TabController : MonoBehaviour
    {
        private TabService tabService;

        [SerializeField] private Button weatherButton;
        [SerializeField] private Button factsButton;
        
        [SerializeField] private Loader loader;
        
        [Inject]
        public void Construct(TabService tabService)
        {
            this.tabService = tabService;

            weatherButton.OnClickAsObservable()
                .Subscribe(_ => ChangeTab<WeatherTab>())
                .AddTo(this);

            factsButton.OnClickAsObservable()
                .Subscribe(_ => ChangeTab<FactsTab>())
                .AddTo(this);
            
            tabService.OnHideLoader
                .Subscribe(_ =>
                {
                    loader.SetEnable(false);
                })
                .AddTo(this);
            
            tabService.OnShowLoader
                .Subscribe(_ =>
                {
                    loader.SetEnable(true);
                })
                .AddTo(this);
            
            ChangeTab<WeatherTab>();
        }

        private void ChangeTab<T>() where T : BaseTab
        {
            loader.SetEnable(true);
            tabService.ChangeTab<T>();
            loader.SettleOnTop();
        }
    }
}