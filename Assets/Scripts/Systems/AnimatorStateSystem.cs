using Components;
using Leopotam.EcsLite;

namespace Systems
{
    public class AnimatorStateSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            const float movementTreshold = .03f;
            
            var world = systems.GetWorld();
            var filter = world.Filter<MainCharacterComponent>()
                .Inc<MovementStateComponent>()
                .Inc<MovementComponent>()
                .End();
            
            var movementPool = world.GetPool<MovementComponent>();
            var animationPool = world.GetPool<MovementStateComponent>();

            foreach (var entity in filter)
            {
                ref var movementComponent = ref movementPool.Get(entity);
                ref var animationComponent = ref animationPool.Get(entity);

                var remainingDistance = (movementComponent.destination - movementComponent.currentPosition).magnitude;
                var isMoving = remainingDistance > movementTreshold;
                
                animationComponent.isMoving = isMoving;
            }
        }
    }
}
