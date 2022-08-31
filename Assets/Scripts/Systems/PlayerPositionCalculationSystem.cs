using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class PlayerPositionCalculationSystem : IEcsRunSystem
    {
        private readonly IPlayerPositionCalculator _positionCalculator;
        private readonly IMovable _player;

        public PlayerPositionCalculationSystem(IPlayerPositionCalculator positionCalculator, IMovable player)
        {
            _positionCalculator = positionCalculator;
            _player = player;
        }

        public void Run(IEcsSystems systems)
        {
            var clickedFilter = systems.GetWorld().Filter<OnClickedTag>().End();

            foreach (var clickedEntity in clickedFilter)
            {
                systems.GetWorld().DelEntity(clickedEntity);
                
                if (_positionCalculator.TryCalculate(out var position))
                {
                    var filter = systems.GetWorld().Filter<MovementComponent>().End();
                    var playerMovementPool = systems.GetWorld().GetPool<MovementComponent>();
                    var playerRotationPool = systems.GetWorld().GetPool<RotationComponent>();

                    foreach (var entity in filter)
                    {
                        ref var movementComponent = ref playerMovementPool.Get(entity);
                        movementComponent.destination = position;
                        
                        ref var rotationComponent = ref playerRotationPool.Get(entity);
                        rotationComponent.targetRotation = Quaternion.LookRotation(position - _player.GetPosition());
                    }
                }
            }
        }
    }
}
