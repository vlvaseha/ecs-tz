using Interfaces;
using UnityEngine;

public class ObjectMover : IMovable
{
    private readonly Transform _transform;

    public ObjectMover(Transform transform)
    {
        _transform = transform;
    }

    public void SetPosition(Vector3 position) => _transform.position = position;

    public Vector3 GetPosition() => _transform.position;
}
