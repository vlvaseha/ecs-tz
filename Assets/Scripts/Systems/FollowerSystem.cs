using Interfaces;
using Leopotam.EcsLite;
using UnityEngine;

namespace Systems
{
    public class FollowerSystem : IEcsRunSystem
    {
        private readonly IMovable _follower;
        private readonly IMovable _target;
        private readonly float _smoothTime;

        private readonly Vector3 _offset;
    
        private Vector3 _velocity;

        public FollowerSystem(IMovable follower, IMovable target, float smoothTime)
        {
            _follower = follower;
            _target = target;
            _smoothTime = smoothTime;

            _offset = _follower.GetPosition() - _target.GetPosition();
            _velocity = Vector3.zero;
        }

        public void Run(IEcsSystems systems)
        {
            var position = Vector3.SmoothDamp(_follower.GetPosition(), 
                _target.GetPosition() + _offset, ref _velocity, _smoothTime);

            _follower.SetPosition(position);
        }
    }
}
