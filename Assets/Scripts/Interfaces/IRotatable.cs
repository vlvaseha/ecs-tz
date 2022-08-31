using UnityEngine;

public interface IRotatable
{
    void SetRotation(Quaternion rotation);

    Quaternion GetRotation();
}
