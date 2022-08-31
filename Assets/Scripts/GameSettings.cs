using System;
using UnityEngine;

[Serializable]
public class GameSettings
{
    [SerializeField] private float _playerMoveSpeed = 5f;
    [SerializeField] private float _playerRotationSpeed = 5f;

    public float PlayerMoveSpeed => _playerMoveSpeed;
    public float PlayerRotationSpeed => _playerRotationSpeed;
}
