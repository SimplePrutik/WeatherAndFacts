using System;
using UnityEngine;
using Zenject;

namespace Tabs
{
    public class TabFactory : IFactory<Type, BaseTab>
    {
        private readonly DiContainer container;
        private Transform tabRoot;

        public TabFactory(DiContainer container)
        {
            this.container = container;

            tabRoot = GameObject.Find("TabController").transform;
        }

        public BaseTab Create(Type tabType)
        {
            var prefab = Resources.Load($"Tabs/{tabType.Name}");
            
            return (BaseTab)container.InstantiatePrefabForComponent(tabType, prefab, tabRoot, Array.Empty<object>());
        }
    }
}