using Interfaces;
using UnityEngine;

public class AnimatorStateUpdater : IAnimatorStateUpdater
{
    private readonly Animator _animator;
    private readonly int _isMovingKey;

    public AnimatorStateUpdater(Animator animator)
    {
        _animator = animator;
        _isMovingKey = Animator.StringToHash("isWalking");
    }
    
    public void SetMovingState(bool isMoving)
    {
        _animator.SetBool(_isMovingKey, isMoving);
    }
}
