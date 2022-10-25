using UnityEngine;

namespace Services
{
    public class InputService
    {
        public bool GetClick() => Input.GetMouseButton(0);
    }
}
