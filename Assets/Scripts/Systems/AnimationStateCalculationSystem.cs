using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class AnimationStateCalculationSystem : IEcsRunSystem
    {
        private readonly IMovable _player;
        private readonly float _velocityTreshold;

        private Vector3 _lastPosition;

        public AnimationStateCalculationSystem(IMovable player)
        {
            _player = player;
            _velocityTreshold = .3f;
        }

        public void Run(IEcsSystems systems)
        {
            var velocity = (_player.GetPosition() - _lastPosition).magnitude / Time.deltaTime;
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
