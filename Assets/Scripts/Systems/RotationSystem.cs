using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class RotationSystem : IEcsRunSystem
    {
        private readonly IRotatable _objectRotator;
        private readonly float _rotationSpeed;
    
        public RotationSystem(IRotatable objectRotator, float rotationSpeed)
        {
            _objectRotator = objectRotator;
            _rotationSpeed = rotationSpeed;
        }

        public void Run(IEcsSystems systems)
        {
            var filter = systems.GetWorld().Filter<RotationComponent>().End();
            var playerPool = systems.GetWorld().GetPool<RotationComponent>();

            foreach (var entity in filter)
            {
                ref var playerComponent = ref playerPool.Get(entity);

                var currentRotation = _objectRotator.GetRotation();
                var newRotation = Quaternion.RotateTowards(currentRotation, 
                    playerComponent.targetRotation,_rotationSpeed * Time.deltaTime);
                
                _objectRotator.SetRotation(newRotation);
            }
        }
    }
}
