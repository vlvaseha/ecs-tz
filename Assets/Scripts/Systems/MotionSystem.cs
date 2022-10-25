using Components;
using Leopotam.EcsLite;
using Services;
using UnityEngine;
using Zenject;

namespace Systems
{
    public class MotionSystem : IEcsRunSystem
    {
        [Inject] private GameSettings _gameSettings;
        [Inject] private TimeService _timeService;
        
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var deltaTime = _timeService.DeltaTime;
            
            var filter = world.Filter<MovementComponent>()
                .End();
            
            var movementPool = world.GetPool<MovementComponent>();

            foreach (var entity in filter)
            {
                ref var movementComponent = ref movementPool.Get(entity);
                
                var currentPosition = movementComponent.currentPosition;
                var newPosition = Vector3.MoveTowards(currentPosition, 
                        currentPosition + (movementComponent.destination - currentPosition),
                        deltaTime * _gameSettings.PlayerMoveSpeed);

                movementComponent.currentPosition = newPosition;
            }
        }
    }
}
