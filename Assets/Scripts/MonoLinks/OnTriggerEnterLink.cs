using Components;
using UnityEngine;

namespace MonoLinks
{
    public class OnTriggerEnterLink : PhysicsTriggerLink
    {
        private void OnTriggerEnter(Collider other)
        {
            var triggeredPool = _world.GetPool<TriggeredComponent>();
            triggeredPool.Add(_entity);
        }
    }
}
