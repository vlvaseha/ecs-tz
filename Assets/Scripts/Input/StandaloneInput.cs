using UnityEngine;

public class StandaloneInput : IInputService
{
    public bool GetClick() => Input.GetMouseButtonDown(0);
}
