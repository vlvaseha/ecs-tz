using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        [SerializeField] private GameObject _container;
        
        public override void InstallBindings()
        {
            Container.Bind<EcsStartup>().FromNewComponentOn(_container).AsSingle().NonLazy();
            Container.BindInterfacesAndSelfTo<EcsWorld>().AsSingle();
        }
    }
}
