using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class InputSystem : IEcsRunSystem
    {
        public void Run(IEcsSystems systems)
        {
            var hasInput = Input.GetMouseButton(0);
            
            if (!hasInput)
                return;
            
            var onClickPool = systems.GetWorld().GetPool<OnClickedTag>();
            onClickPool.Add(systems.GetWorld().NewEntity());
        }
    }
}
