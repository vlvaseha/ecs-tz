using UnityEngine;

public class StandaloneInput : IInputService
{
    public bool GetClick() => Input.GetMouseButtonUp(0);
}
