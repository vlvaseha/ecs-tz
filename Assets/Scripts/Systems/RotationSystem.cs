using Components;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Systems
{
    public class RotationSystem : IEcsRunSystem
    {
        [Inject] private GameSettings _gameSettings;

        public void Run(IEcsSystems systems)
        {
            var deltaTime = Time.deltaTime;
            var world = systems.GetWorld();
            
            var filter = systems.GetWorld().Filter<TransformComponent>()
                .Inc<RotationComponent>()
                .End();
            
            var rotationComponentPool = systems.GetWorld().GetPool<RotationComponent>();
            var transformsPool = world.GetPool<TransformComponent>();

            foreach (var entity in filter)
            {
                ref var rotationComponent = ref rotationComponentPool.Get(entity);
                ref var transformComponent = ref transformsPool.Get(entity);

                rotationComponent.currentRotation = transformComponent.transform.rotation;

                var currentRotation = rotationComponent.currentRotation;
                var newRotation = Quaternion.RotateTowards(currentRotation, 
                    rotationComponent.targetRotation,_gameSettings.PlayerRotationSpeed * deltaTime);
                
                transformComponent.transform.rotation = newRotation;
            }
        }
    }
}
