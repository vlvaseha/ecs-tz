using UnityEngine;

public interface IButton : IMovable
{
    void SetEntity(int entity);

    void SetColor(int property, Color color);
}
