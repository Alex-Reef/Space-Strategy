using GalaxySystem.Behavior;
using UnityEngine;
using Zenject;

public class GalaxySystemInstaller : MonoInstaller
{
    [SerializeField] private GalaxyBehavior target;
    
    public override void InstallBindings()
    {
        Container.Bind<GalaxyBehavior>().FromInstance(target).AsSingle().NonLazy();
    }
}