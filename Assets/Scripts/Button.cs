using Components;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

public class Button : MonoBehaviour, IButton
{
    private int _entity;
    
    [Inject] private EcsWorld _ecsWorld;

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player")) return;
        
        SetButtonPressedState(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("Player")) return;
        
        SetButtonPressedState(false);
    }

    public void SetPosition(Vector3 position) => transform.localPosition = position;

    public Vector3 GetPosition() => transform.localPosition;

    public void SetEntity(int entity) => _entity = entity;

    public void SetColor(int property, Color color) => 
        GetComponent<MeshRenderer>().material.SetColor(property, color);

    private void SetButtonPressedState(bool isPressed)
    {
        var pool = _ecsWorld.GetPool<ButtonComponent>();
        
        if (pool.Has(_entity))
        {
            ref var buttonComponent = ref pool.Get(_entity);
            buttonComponent.isPressed = isPressed;
        }
    }
}
