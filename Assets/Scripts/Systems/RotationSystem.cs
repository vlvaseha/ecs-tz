using Components;
using Leopotam.EcsLite;
using Services;
using UnityEngine;
using Zenject;

namespace Systems
{
    public class RotationSystem : IEcsRunSystem
    {
        [Inject] private GameSettings _gameSettings;
        [Inject] private TimeService _timeService;

        public void Run(IEcsSystems systems)
        {
            var deltaTime = _timeService.DeltaTime;
            var world = systems.GetWorld();
            var filter = systems.GetWorld().Filter<RotationComponent>().End();
            var rotationComponentPool = world.GetPool<RotationComponent>();

            foreach (var entity in filter)
            {
                ref var rotationComponent = ref rotationComponentPool.Get(entity);

                var currentRotation = rotationComponent.currentRotation;
                var newRotation = Quaternion.RotateTowards(currentRotation, 
                    rotationComponent.targetRotation,_gameSettings.PlayerRotationSpeed * deltaTime);

                rotationComponent.currentRotation = newRotation;
            }
        }
    }
}
