using UnityEngine;

public class ObjectRotator : IRotatable
{
    private readonly Transform _transform;

    public ObjectRotator(Transform transform)
    {
        _transform = transform;
    }

    public void SetRotation(Quaternion rotation) => _transform.rotation = rotation;

    public Quaternion GetRotation() => _transform.rotation;
}
