using Leopotam.EcsLite;

namespace MonoLinks
{
    public class PhysicsTriggerLink : MonoLinkBase
    {
        protected int _entity;
        protected EcsWorld _world;

        public override void Make(ref int entity, EcsWorld world)
        {
            _entity = entity;
            _world = world;
        }
    }
}
