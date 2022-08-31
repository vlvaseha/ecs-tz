using UnityEngine;

public class PlayerPositionCalculator : IPlayerPositionCalculator
{
    private readonly Camera _camera;

    public PlayerPositionCalculator(Camera camera)
    {
        _camera = camera;
    }

    public bool TryCalculate(out Vector3 position)
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, 1000f))
        {
            if (hitInfo.transform.CompareTag("Ground"))
            {
                position = hitInfo.point;
                return true;
            }
        }

        position = Vector3.zero;
        return false;
    }
}
