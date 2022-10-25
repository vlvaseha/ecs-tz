using Components;
using Leopotam.EcsLite;
using MonoLinks;
using UnityEngine;
using Zenject;

public class SceneData : MonoBehaviour
{
    [SerializeField] private Transform _spawnPlayerTransform;
    [Space]
    [SerializeField] private Transform[] _buttonsRoot;
    [SerializeField] private Transform[] _doors;

    [Inject] private ButtonsFactory _buttonsFactory;
    [Inject] private EcsWorld _world;
    [Inject] private Camera _camera;

    private void Awake()
    {
        if (_buttonsRoot.Length != _doors.Length)
        {
            Debug.LogError("Count of buttons and doors must match!");
        }
    }

    public void Initialize()
    {
        SetupMainCharacter();
        SetupCamera();
        CreateButtons();
    }

    private void SetupMainCharacter()
    {
        var prefab = Resources.Load<GameObject>("Character");
        var player = Instantiate(prefab, transform);
        player.transform.SetPositionAndRotation(_spawnPlayerTransform.position, _spawnPlayerTransform.rotation);

        var links = player.GetComponents<MonoLinkBase>();
        var newEntity = _world.NewEntity();
        
        var mainCharacterPool = _world.GetPool<MainCharacterComponent>();
        var playerMovementPool = _world.GetPool<MovementComponent>();
        var playerRotationPool = _world.GetPool<RotationComponent>();
        var playerAnimationPool = _world.GetPool<MovementStateComponent>();
            
        mainCharacterPool.Add(newEntity);
        playerMovementPool.Add(newEntity);
        playerRotationPool.Add(newEntity);
        playerAnimationPool.Add(newEntity);

        foreach (var link in links)
        {
            link.Make(ref newEntity, _world);
        }
    }

    private void SetupCamera()
    {
        var links = _camera.GetComponents<MonoLinkBase>();
        var newEntity = _world.NewEntity();

        var cameraPool = _world.GetPool<MainCameraComponent>();
        var movementComponent = _world.GetPool<MovementComponent>();
        
        cameraPool.Add(newEntity);
        movementComponent.Add(newEntity);

        foreach (var link in links)
        {
            link.Make(ref newEntity, _world);
        }
    }

    private void CreateButtons()
    {
        var colorProperty = Shader.PropertyToID("_Color");
        var buttonsPool = _world.GetPool<ButtonComponent>();
        var movementPool = _world.GetPool<MovementComponent>();
        var pressedStatePool = _world.GetPool<PressedStateComponent>();
        
        for (int i = 0; i < _buttonsRoot.Length; i++)
        {
            var button = _buttonsFactory.Create();

            SetupTransformAndColor(button.transform, i);
            SetupSceneObject(button.gameObject, out var entity);
            SetupButtonComponents(entity, button.transform);
            LinkButtonToDoor(entity, i);
        }

        void SetupTransformAndColor(Transform buttonTransform, int buttonIndex)
        {
            var buttonRenderer = buttonTransform.GetComponentInChildren<Renderer>();
            
            buttonTransform.SetParent(_buttonsRoot[buttonIndex]);
            buttonTransform.localPosition = Vector3.zero;
            buttonRenderer.material.SetColor(colorProperty, GetRandomColor());
        }

        void SetupButtonComponents(int entity, Transform buttonTransform)
        {
            buttonsPool.Add(entity);
            
            ref var movementComponent = ref movementPool.Add(entity);
            ref var pressedComponent = ref pressedStatePool.Add(entity);
            var buttonRoot = buttonTransform.parent;

            movementComponent.destination = buttonRoot.TransformPoint(Vector3.zero);
            movementComponent.currentPosition = movementComponent.destination;
            
            pressedComponent.defaultPosition = movementComponent.destination;
            pressedComponent.pressedPosition = buttonRoot.TransformPoint(Vector3.up * -.95f);
        }
    }

    private void LinkButtonToDoor(int buttonEntity, int buttonIndex)
    {
        var door = _doors[buttonIndex];
        var doorRoot = door.parent;
        
        var doorsPool = _world.GetPool<DoorComponent>();
        var movementPool = _world.GetPool<MovementComponent>();
        var pressedStatePool = _world.GetPool<PressedStateComponent>();
        
        SetupSceneObject(door.gameObject, out var doorEntity);

        ref var doorComponent = ref doorsPool.Add(doorEntity);
        ref var movementComponent = ref movementPool.Add(doorEntity);
        ref var pressedStateComponent = ref pressedStatePool.Add(doorEntity);
        
        doorComponent.linkedButtonEntity = buttonEntity;
        movementComponent.destination = door.position;
        movementComponent.currentPosition = door.position;

        pressedStateComponent.defaultPosition = doorRoot.TransformPoint(Vector3.zero);
        pressedStateComponent.pressedPosition = doorRoot.TransformPoint(-Vector3.up);
    }

    private void SetupSceneObject(GameObject obj, out int entity)
    {
        var links = obj.GetComponents<MonoLinkBase>();
        entity = _world.NewEntity();
        
        foreach (var link in links)
        {
            link.Make(ref entity, _world);
        }
    }
    
    private Color GetRandomColor() => 
        Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
}
