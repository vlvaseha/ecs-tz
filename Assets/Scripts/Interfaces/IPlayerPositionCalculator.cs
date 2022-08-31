using UnityEngine;

public interface IPlayerPositionCalculator
{
    bool TryCalculate(out Vector3 position);
}
