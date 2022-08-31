using Leopotam.EcsLite;

namespace Systems
{
    public class PlayerInitSystem : IEcsInitSystem
    {
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var playerEntity = world.NewEntity();
            
            var playerInputPool = world.GetPool<MovementComponent>();
            playerInputPool.Add(playerEntity);
        }
    }
}
