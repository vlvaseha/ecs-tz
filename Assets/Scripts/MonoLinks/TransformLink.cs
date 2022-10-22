using Components;
using Leopotam.EcsLite;

namespace MonoLinks
{
    public class TransformLink : MonoLinkBase
    {
        public override void Make(ref int entity, EcsWorld world)
        {
            var transformComponentPool = world.GetPool<TransformComponent>();

            ref var transformComponent = ref transformComponentPool.Add(entity);
            transformComponent.transform = transform;
        }
    }
}
