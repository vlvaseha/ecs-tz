using Components;
using Interfaces;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class MovementSystem : IEcsRunSystem
    {
        private readonly IMovable _movableObject;
        private readonly ITimer _timer;
        private readonly float _moveSpeed;
        
        public MovementSystem(IMovable movableObject, ITimer timer, float moveSpeed)
        {
            _movableObject = movableObject;
            _timer = timer;
            _moveSpeed = moveSpeed;
        }
        
        public void Run(IEcsSystems systems)
        {
            var filter = systems.GetWorld().Filter<MovementComponent>().End();
            var playerPool = systems.GetWorld().GetPool<MovementComponent>();

            foreach (var entity in filter)
            {
                ref var playerComponent = ref playerPool.Get(entity);

                var currentPosition = _movableObject.GetPosition();
                var newPosition = 
                    Vector3.MoveTowards(currentPosition, 
                        currentPosition + (playerComponent.destination - currentPosition),
                        _timer.DeltaTime * _moveSpeed);
                
                _movableObject.SetPosition(newPosition);
            }
        }
    }
}
