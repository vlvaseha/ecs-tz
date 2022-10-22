using Components;
using Leopotam.EcsLite;
using UnityEngine;

public class UpdateMovingAnimStateSystem : IEcsRunSystem
{
    private static readonly int _animationHash = Animator.StringToHash("isWalking");
    
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        
        var filter = world.Filter<MovementStateComponent>()
            .Inc<AnimatorComponent>()
            .End();
        
        var animationPool = world.GetPool<MovementStateComponent>();
        var animatorComponentsPool = world.GetPool<AnimatorComponent>();

        foreach (var entity in filter)
        {
            ref var animationComponent = ref animationPool.Get(entity);
            ref var animatorComponent = ref animatorComponentsPool.Get(entity);
            
            animatorComponent.animator.SetBool(_animationHash, animationComponent.isMoving);
        }
    }
}
