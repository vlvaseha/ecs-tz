using Leopotam.EcsLite;
using UnityEngine;

public class AnimatorLink : MonoLinkBase
{
    [SerializeField] private Animator _animator;
    
    public override void Make(ref int entity, EcsWorld world)
    {
        var transformComponentPool = world.GetPool<AnimatorComponent>();

        ref var transformComponent = ref transformComponentPool.Add(entity);
        transformComponent.animator = _animator;
    }
}
