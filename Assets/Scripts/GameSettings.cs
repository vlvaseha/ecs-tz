using System;
using UnityEngine;

[Serializable]
public class GameSettings
{
    [SerializeField] private float _playerMoveSpeed = 2f;
    [SerializeField] private float _playerRotationSpeed = 300f;
    [SerializeField] private float _cameraFollowSmoothTime = .6f;

    public float PlayerMoveSpeed => _playerMoveSpeed;
    public float PlayerRotationSpeed => _playerRotationSpeed;
    public float CameraFollowSmoothTime => _cameraFollowSmoothTime;
}
