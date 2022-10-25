using Components;
using Leopotam.EcsLite;

namespace MonoLinks
{
    public class TransformLink : MonoLinkBase
    {
        private EcsPool<MovementComponent> _movementPool;
        private EcsPool<RotationComponent> _rotationPool;
        private int _entity;
        
        public override void Make(ref int entity, EcsWorld world)
        {
            _movementPool = world.GetPool<MovementComponent>();
            _rotationPool = world.GetPool<RotationComponent>();
            _entity = entity;

            if (_movementPool.Has(entity))
            {
                ref var movementComponent = ref _movementPool.Get(_entity);
                var currentPosition = transform.position;
            
                movementComponent.destination = currentPosition;
                movementComponent.currentPosition = currentPosition;
            }

            if (_rotationPool.Has(entity))
            {
                ref var rotationComponent = ref _rotationPool.Get(_entity);
                var currentRotation = transform.rotation;
            
                rotationComponent.targetRotation = currentRotation;
                rotationComponent.currentRotation = currentRotation;
            }
        }

        private void Update()
        {
            UpdatePosition();
            UpdateRotation();
        }

        private void UpdatePosition()
        {
            if (_movementPool == null || !_movementPool.Has(_entity)) return;
            
            ref var movementComponent = ref _movementPool.Get(_entity);
            transform.position = movementComponent.currentPosition;
        }

        private void UpdateRotation()
        {
            if (_rotationPool == null || !_rotationPool.Has(_entity)) return;
            
            ref var rotationComponent = ref _rotationPool.Get(_entity);
            transform.rotation = rotationComponent.currentRotation;
        }
    }
}
