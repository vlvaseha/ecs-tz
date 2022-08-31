using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class InputSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
        }

        public void Run(IEcsSystems systems)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("space pressed: " + (_world == null));
            }
        }
    }
}
