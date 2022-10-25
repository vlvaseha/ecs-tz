using Components;
using Leopotam.EcsLite;

namespace Systems
{
    public class ButtonsStateControllerSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var buttonsFilter = world.Filter<ButtonComponent>().End();
            var triggers = world.Filter<TriggeredComponent>().End();
            
            var buttonsPool = world.GetPool<ButtonComponent>();
            var movementPool = world.GetPool<MovementComponent>();
            var pressedStatePool = world.GetPool<PressedStateComponent>();

            foreach (var buttonEntity in buttonsFilter)
            {
                var entity = buttonEntity;
                var isTriggered = IsFilterContains(ref triggers, ref entity);

                ref var buttonComponent = ref buttonsPool.Get(entity);
                buttonComponent.isPressed = isTriggered;
                
                ref var pressedStateComponent = ref pressedStatePool.Get(entity);
                ref var movementComponent = ref movementPool.Get(entity);
                
                movementComponent.destination = isTriggered
                    ? pressedStateComponent.pressedPosition
                    : pressedStateComponent.defaultPosition;
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
}
