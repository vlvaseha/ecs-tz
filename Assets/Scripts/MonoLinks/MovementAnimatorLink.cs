using Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace MonoLinks
{
    public class MovementAnimatorLink : MonoLinkBase
    {
        private static readonly int _animationHash = Animator.StringToHash("isWalking");

        [SerializeField] private Animator _animator;

        private EcsPool<MovementStateComponent> _movementStatePool;
        private int _entity;
        
        public override void Make(ref int entity, EcsWorld world)
        {
            _movementStatePool = world.GetPool<MovementStateComponent>();
            _entity = entity;
        }

        private void Update()
        {
            if (_movementStatePool != null && _movementStatePool.Has(_entity))
            {
                _animator.SetBool(_animationHash, _movementStatePool.Get(_entity).isMoving);
            }
        }
    }
}
