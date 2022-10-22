using UnityEngine;

public class OnTriggerExitLink : PhysicsTriggerLink
{
    public void OnTriggerExit(Collider other)
    {
        var triggeredPool = _world.GetPool<TriggeredComponent>();
        triggeredPool.Del(_entity);
    }
}
