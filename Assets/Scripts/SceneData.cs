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
    [Inject] Camera _camera;

    private void Awake()
    {
        if (_buttonsRoot.Length != _doors.Length)
        {
            Debug.LogError("Count of buttons and doors must match!");
            return;
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
        
        for (int i = 0; i < _buttonsRoot.Length; i++)
        {
            var button = _buttonsFactory.Create();
            var buttonTransform = button.transform;
            var buttonPosition = buttonTransform.position;
            var buttonRenderer = buttonTransform.GetComponentInChildren<Renderer>();
            
            buttonTransform.SetParent(_buttonsRoot[i]);
            buttonTransform.localPosition = Vector3.zero;
            buttonRenderer.material.SetColor(colorProperty, GetRandomColor());
            
            SetupSceneObject(buttonTransform.gameObject, out var entity);
            buttonsPool.Add(entity);
            
            ref var movementComponent = ref movementPool.Add(entity);
            movementComponent.destination = buttonTransform.position;
            movementComponent.currentPosition = buttonPosition;

            LinkButtonToDoor(entity, i);
        }
    }

    private void LinkButtonToDoor(int buttonEntity, int buttonIndex)
    {
        var door = _doors[buttonIndex];
        var doorsPool = _world.GetPool<DoorComponent>();
        var movementPool = _world.GetPool<MovementComponent>();
        
        SetupSceneObject(door.gameObject, out var doorEntity);

        ref var doorComponent = ref doorsPool.Add(doorEntity);
        ref var movementComponent = ref movementPool.Add(doorEntity);
        
        doorComponent.linkedButtonEntity = buttonEntity;
        movementComponent.destination = door.position;
        movementComponent.currentPosition = door.position;
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
