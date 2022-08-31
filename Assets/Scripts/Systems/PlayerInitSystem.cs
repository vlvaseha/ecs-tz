using Components;
using Leopotam.EcsLite;

namespace Systems
{
    public class PlayerInitSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var playerEntity = world.NewEntity();
            
            var playerMovementPool = world.GetPool<MovementComponent>();
            playerMovementPool.Add(playerEntity);

            var playerRotationPool = world.GetPool<RotationComponent>();
            playerRotationPool.Add(playerEntity);

            var playerAnimationPool = world.GetPool<AnimationComponent>();
            playerAnimationPool.Add(playerEntity);
        }
    }
}
