using System.Collections;
using Components;
using Leopotam.EcsLite;
using UnityEngine;

public class ButtonsStateControllerSystem : IEcsRunSystem
{
    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var buttonsFilter = world.Filter<ButtonComponent>()
            .Inc<TransformComponent>()
            .End();
        
        var triggers = world.Filter<TriggeredComponent>().End();
        var buttonsPool = world.GetPool<ButtonComponent>();
        var transformsPool = world.GetPool<TransformComponent>();
        var movementPool = world.GetPool<MovementComponent>();

        foreach (var buttonEntity in buttonsFilter)
        {
            var entity = buttonEntity;
            var isTriggered = IsFilterContains(ref triggers, ref entity);

            ref var buttonComponent = ref buttonsPool.Get(entity);
            buttonComponent.isPressed = isTriggered;

            ref var transformComponent = ref transformsPool.Get(entity);
            ref var movementComponent = ref movementPool.Get(entity);

            var buttonRoot = transformComponent.transform.parent;
            movementComponent.currentPosition = transformComponent.transform.position;
            movementComponent.destination = isTriggered
                ? buttonRoot.TransformPoint(Vector3.up * -.95f)
                : buttonRoot.TransformPoint(Vector3.zero);
        }
    }

    private bool IsFilterContains(ref EcsFilter filter, ref int entity)
    {
        foreach (var f in filter)
        {
            if (f == entity) return true;
        }

        return false;
    }
}
