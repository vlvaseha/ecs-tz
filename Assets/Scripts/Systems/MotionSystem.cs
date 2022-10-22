using Components;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Systems
{
    public class MotionSystem : IEcsRunSystem
    {
        [Inject] private GameSettings _gameSettings;
        
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var deltaTime = Time.deltaTime;
            
            var filter = world.Filter<TransformComponent>()
                .Inc<MovementComponent>()
                .End();
            
            var movementPool = world.GetPool<MovementComponent>();
            var transformsPool = world.GetPool<TransformComponent>();

            foreach (var entity in filter)
            {
                ref var movementComponent = ref movementPool.Get(entity);
                ref var transformComponent = ref transformsPool.Get(entity);
                
                movementComponent.currentPosition = transformComponent.transform.position;
                
                var currentPosition = movementComponent.currentPosition;
                var newPosition = Vector3.MoveTowards(currentPosition, 
                        currentPosition + (movementComponent.destination - currentPosition),
                        deltaTime * _gameSettings.PlayerMoveSpeed);

                transformComponent.transform.position = newPosition;
            }
        }
    }
}
