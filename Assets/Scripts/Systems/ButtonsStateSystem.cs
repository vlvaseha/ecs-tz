using System.Collections.Generic;
using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class ButtonsStateSystem : IEcsInitSystem, IEcsRunSystem
    {
        private const float PressForce = 15f;
        
        private readonly IButton[] _buttons;
        private readonly Dictionary<int, IButton> _entityButtons;
        
        public ButtonsStateSystem(IButton[] buttons)
        {
            _buttons = buttons;
            _entityButtons = new Dictionary<int, IButton>(_buttons.Length);
        }
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var buttonsPool = world.GetPool<ButtonComponent>();

            foreach (var button in _buttons)
            {
                var buttonEntity = world.NewEntity();
                buttonsPool.Add(buttonEntity);
                button.SetEntity(buttonEntity);
                
                _entityButtons.Add(buttonEntity, button);
            }
        }

        public void Run(IEcsSystems systems)
        {
            var buttonsComponents = systems.GetWorld().Filter<ButtonComponent>().End();
            var doorsComponents = systems.GetWorld().Filter<DoorComponent>().End();
            var buttonsPool = systems.GetWorld().GetPool<ButtonComponent>();
            var doorsPool = systems.GetWorld().GetPool<DoorComponent>();

            foreach (var entity in buttonsComponents)
            {
                var buttonComponent = buttonsPool.Get(entity);
                UpdateButtonPressedState(buttonComponent.isPressed, entity);
                TrySetDoorOpeningState(entity, buttonComponent.isPressed);
                // if (TryGetDoorComponent(entity, out var doorComponent))
                // doorComponent.isOpening = buttonComponent.isPressed;
            }
            
            void TrySetDoorOpeningState(int buttonEntity, bool isButtonPressed)
            {
                foreach (var doorEntity in doorsComponents)
                {
                    ref var component = ref doorsPool.Get(doorEntity);
                    // Debug.Log("compare: " + component.linkedButtonEntity + " " + buttonEntity);

                    if (component.linkedButtonEntity == buttonEntity)
                    {
                        component.isOpening = isButtonPressed;
                    }
                }
            }
        }

        private void UpdateButtonPressedState(bool isPressed, int entity)
        {
            var targetPosition = isPressed ? -1.3f * Vector3.up : Vector3.zero;
            var button = _entityButtons[entity];
            var buttonPosition = Vector3.MoveTowards(button.GetPosition(), targetPosition, 
                PressForce * Time.deltaTime);
            button.SetPosition(buttonPosition);
        }
    }
}
