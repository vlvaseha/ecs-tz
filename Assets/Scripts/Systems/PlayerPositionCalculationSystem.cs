using Leopotam.EcsLite;

namespace Systems
{
    public class PlayerPositionCalculationSystem : IEcsRunSystem
    {
        private readonly IPlayerPositionCalculator _positionCalculator;

        public PlayerPositionCalculationSystem(IPlayerPositionCalculator positionCalculator)
        {
            _positionCalculator = positionCalculator;
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
                    var playerInputPool = systems.GetWorld().GetPool<MovementComponent>();

                    foreach (var entity in filter)
                    {
                        ref var movementComponent = ref playerInputPool.Get(entity);
                        movementComponent.destination = position;
                    }
                }
            }
        }
    }
}
