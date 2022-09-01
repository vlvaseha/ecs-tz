using Components;
using Interfaces;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class AnimationStateCalculationSystem : IEcsRunSystem
    {
        private readonly IMovable _player;
        private readonly ITimer _timer;
        private readonly float _velocityTreshold;

        private Vector3 _lastPosition;

        public AnimationStateCalculationSystem(IMovable player, ITimer timer)
        {
            _player = player;
            _timer = timer;
            _velocityTreshold = .3f;
        }

        public void Run(IEcsSystems systems)
        {
            var velocity = (_player.GetPosition() - _lastPosition).magnitude / _timer.DeltaTime;
            var isMoving = velocity > _velocityTreshold;
            
            _lastPosition = _player.GetPosition();
            
            var filter = systems.GetWorld().Filter<AnimationComponent>().End();
            var animationPool = systems.GetWorld().GetPool<AnimationComponent>();

            foreach (var entity in filter)
            {
                ref var animationComponent = ref animationPool.Get(entity);
                animationComponent.isMoving = isMoving;
            }
        }
    }
}
