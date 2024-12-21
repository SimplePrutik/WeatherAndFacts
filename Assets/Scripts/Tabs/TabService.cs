using System;
using System.Collections.Generic;
using Tabs.Items;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Tabs
{
    public class TabService
    {
        private IFactory<Type, BaseTab> tabFactory;
        private Dictionary<Type, BaseTab> tabCache = new Dictionary<Type, BaseTab>();
        private BaseTab currentTab;
        
        public ReactiveCommand OnHideLoader = new ReactiveCommand();
        public ReactiveCommand OnShowLoader = new ReactiveCommand();
        
        [Inject]
        public void Construct(IFactory<Type, BaseTab> tabFactory)
        {
            this.tabFactory = tabFactory;
        }
        
        public void ChangeTab<T>() where T : BaseTab
        {
            currentTab?.Close();
            var tabType = typeof(T);
            if (tabCache.ContainsKey(tabType))
                currentTab = tabCache[tabType] as T;
            else
            {
                currentTab = tabFactory.Create(typeof(T));
            }
            tabCache[tabType] = currentTab;
            currentTab.Open();
        }
    }
}
