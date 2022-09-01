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
            var buttonPool = world.GetPool<ButtonComponent>();

            foreach (var button in _buttons)
            {
                var buttonEntity = world.NewEntity();
                buttonPool.Add(buttonEntity);
                button.SetEntity(buttonEntity);
                
                _entityButtons.Add(buttonEntity, button);
            }
        }

        public void Run(IEcsSystems systems)
        {
            var ecsFilter = systems.GetWorld().Filter<ButtonComponent>().End();
            var pool = systems.GetWorld().GetPool<ButtonComponent>();

            foreach (var entity in ecsFilter)
            {
                var buttonComponent = pool.Get(entity);
                UpdateButtonPressedState(buttonComponent.isPressed, entity);
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
