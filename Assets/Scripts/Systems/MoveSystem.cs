using Components;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Systems
{
    public class MoveSystem : IEcsRunSystem, IEcsInitSystem
    {
        [Inject] private Camera _camera;
        private GroundRaycastHandler _positionCalculator;

        public void Init(IEcsSystems systems)
        {
            _positionCalculator = new GroundRaycastHandler(_camera);
        }

        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var clickedFilter = world.Filter<OnClickedTag>().End();

            foreach (var clickedEntity in clickedFilter)
            {
                systems.GetWorld().DelEntity(clickedEntity);
                
                if (_positionCalculator.TryHandleRaycast(out var position))
                {
                    var filter = world.Filter<MainCharacterComponent>()
                        .Inc<MovementComponent>()
                        .Inc<RotationComponent>()
                        .End();
                    
                    var playerMovementPool = world.GetPool<MovementComponent>();
                    var playerRotationPool = world.GetPool<RotationComponent>();

                    foreach (var entity in filter)
                    {
                        ref var movementComponent = ref playerMovementPool.Get(entity);
                        var lastPosition = movementComponent.currentPosition;
                        movementComponent.destination = position;
                        
                        ref var rotationComponent = ref playerRotationPool.Get(entity);
                        rotationComponent.targetRotation = Quaternion.LookRotation(position - lastPosition);
                    }
                }
            }
        }
    }
}
