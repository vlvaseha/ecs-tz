using Components;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

namespace Systems
{
    public class CameraFollowSystem : IEcsRunSystem, IEcsInitSystem
    {
        [Inject] private GameSettings _gameSettings;
        
        private Vector3 _velocity;
        private Vector3 _offset;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            _offset = GetPosition<MainCameraComponent>(world) - GetPosition<MainCharacterComponent>(world);
        }
        
        public void Run(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            var camPosition = GetPosition<MainCameraComponent>(world);
            var characterPosition = GetPosition<MainCharacterComponent>(world);

            var filter = world.Filter<MainCameraComponent>()
                .Inc<MovementComponent>()
                .End();
            
            var movementPool = world.GetPool<MovementComponent>();

            var position = Vector3.SmoothDamp(camPosition,
                characterPosition + _offset, ref _velocity, _gameSettings.CameraFollowSmoothTime);

            foreach (var entity in filter)
            {
                ref var movementComponent = ref movementPool.Get(entity);
                movementComponent.destination = position;
            }
        }

        private Vector3 GetPosition<T>(EcsWorld world) where T : struct
        {
            var filter = world.Filter<T>()
                .Inc<TransformComponent>()
                .End();
            
            var transformsPool = world.GetPool<TransformComponent>();

            foreach (var entity in filter)
            {
                ref var transformComponent = ref transformsPool.Get(entity);
                return transformComponent.transform.position;
            }
            
            return Vector3.zero;
        }
    }
}
