using UnityEngine;

public class GroundRaycastHandler
{
    private readonly Camera _camera;
    private readonly int _layerMask;

    public GroundRaycastHandler(Camera camera)
    {
        _camera = camera;
        _layerMask = ~(1 << 6 | 1 << 7);
    }

    public bool TryHandleRaycast(out Vector3 position)
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo, 1000f, _layerMask))
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
