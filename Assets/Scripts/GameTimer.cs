using Interfaces;
using UnityEngine;

public class GameTimer : ITimer
{
    public float DeltaTime => Time.deltaTime;
}
