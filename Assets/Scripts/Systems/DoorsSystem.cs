using Components;
using Leopotam.EcsLite;

namespace Systems
{
    public class DoorsSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var buttonsFilter = world.Filter<ButtonComponent>().End();
            var movementPool = world.GetPool<MovementComponent>();
            var pressedPool = world.GetPool<PressedStateComponent>();

            foreach (var entity in buttonsFilter)
            {
                var isPressed = IsButtonPressed(entity, world);
                var linkedDoorEntity = FindDoorEntityBuButton(entity, world);
                var pressedComponent = pressedPool.Get(linkedDoorEntity);

                ref var movementComponent = ref movementPool.Get(linkedDoorEntity);
                movementComponent.destination = isPressed
                    ? pressedComponent.pressedPosition
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
