using Leopotam.EcsLite;
using MonoLinks;
using Services;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<SceneData>().FromComponentInHierarchy().AsSingle();
            Container.Bind<EcsWorld>().AsSingle();
            Container.BindInterfacesAndSelfTo<EcsStartup>().AsSingle().NonLazy();
            Container.Bind<TimeService>().AsSingle();
            Container.Bind<InputService>().AsSingle();
            Container.Bind<GroundRaycastHandler>().AsSingle();
            
            CreateButtonsFactory();
        }

        private void CreateButtonsFactory()
        {
            var prefab = Resources.Load<TransformLink>("Button");
            Container.BindFactory<TransformLink, ButtonsFactory>().FromComponentInNewPrefab(prefab).AsSingle();
        }
    }
}
