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
            Container.Bind<SceneData>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<EcsWorld>().AsSingle();
            Container.Bind<EcsStartup>().FromNewComponentOn(_container).AsSingle().NonLazy();
            
            CreateButtonsFactory();
        }

        private void CreateButtonsFactory()
        {
            var prefab = Resources.Load<Button>("Button");
            Container.BindFactory<Button, ButtonsFactory>().FromComponentInNewPrefab(prefab).AsSingle();
        }
    }
}
