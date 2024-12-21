using System;
using Tabs;
using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<TabService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<WebRequestService>().AsSingle().NonLazy();
        
        Container.BindIFactory<Type, BaseTab>().FromFactory<TabFactory>();
    }
}