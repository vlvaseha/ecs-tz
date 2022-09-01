using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

public class Button : MonoBehaviour
{
    [Inject] private EcsWorld _ecsWorld;

    private void OnTriggerEnter(Collider other)
    {
        if(!other.CompareTag("Player")) return;
        
        Debug.Log("enter " + other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.CompareTag("Player")) return;
        
        Debug.Log("exit " + other.name);
    }
}
