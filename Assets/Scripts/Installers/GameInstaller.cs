using Leopotam.EcsLite;
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
            
            CreateButtonsFactory();
        }

        private void CreateButtonsFactory()
        {
            var prefab = Resources.Load<Button>("Button");
            Container.BindFactory<Button, ButtonsFactory>().FromComponentInNewPrefab(prefab).AsSingle();
        }
    }
}
