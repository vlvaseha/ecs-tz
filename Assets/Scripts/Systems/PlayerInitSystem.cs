using Components;
using Leopotam.EcsLite;

namespace Systems
{
    public class PlayerInitSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var filter = world.Filter<MainCharacterComponent>().End();

            foreach (var playerEntity in filter)
            {
                var playerMovementPool = world.GetPool<MovementComponent>();
                var playerRotationPool = world.GetPool<RotationComponent>();
                var playerAnimationPool = world.GetPool<MovementStateComponent>();
            
                playerMovementPool.Add(playerEntity);
                playerRotationPool.Add(playerEntity);
                playerAnimationPool.Add(playerEntity);
            }
        }
    }
}
