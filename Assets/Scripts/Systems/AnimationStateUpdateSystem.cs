using Components;
using Interfaces;
using Leopotam.EcsLite;

public class AnimationStateUpdateSystem : IEcsRunSystem
{
    private readonly IAnimatorStateUpdater _animatorStateUpdater;

    public AnimationStateUpdateSystem(IAnimatorStateUpdater animatorStateUpdater)
    {
        _animatorStateUpdater = animatorStateUpdater;
    }

    public void Run(IEcsSystems systems)
    {
        var filter = systems.GetWorld().Filter<AnimationComponent>().End();
        var animationPool = systems.GetWorld().GetPool<AnimationComponent>();

        foreach (var entity in filter)
        {
            ref var animationComponent = ref animationPool.Get(entity);
            _animatorStateUpdater.SetMovingState(animationComponent.isMoving);
        }
    }
}
