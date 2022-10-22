using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class DoorsSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var buttonsFilter = world.Filter<ButtonComponent>().End();
            var movementPool = world.GetPool<MovementComponent>();
            var transformPool = world.GetPool<TransformComponent>();

            foreach (var entity in buttonsFilter)
            {
                var isPressed = IsButtonPressed(entity, world);
                var linkedDoorEntity = FindDoorEntityBuButton(entity, world);
                var doorTransformComponent = transformPool.Get(linkedDoorEntity);

                ref var movementComponent = ref movementPool.Get(linkedDoorEntity);
                movementComponent.destination = isPressed
                    ? doorTransformComponent.transform.parent.TransformPoint(-Vector3.up)
                    : movementComponent.currentPosition;
            }
        }

        private bool IsButtonPressed(int buttonEntity, EcsWorld world)
        {
            var buttonsPool = world.GetPool<ButtonComponent>();
            return buttonsPool.Get(buttonEntity).isPressed;
        }

        private int FindDoorEntityBuButton(int buttonEntity, EcsWorld world)
        {
            var doorsFilter = world.Filter<DoorComponent>().End();
            var doorsPool = world.GetPool<DoorComponent>();

            foreach (var doorEntity in doorsFilter)
            {
                if (doorsPool.Get(doorEntity).linkedButtonEntity == buttonEntity)
                    return doorEntity;
            }

            return -1;
        }
    }
}
