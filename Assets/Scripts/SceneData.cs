using Components;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

public class SceneData : MonoBehaviour
{
    [SerializeField] private Transform _spawnPlayerTransform;
    [SerializeField] private Transform _cameraTransform;
    [Space]
    [SerializeField] private Transform[] _buttonsRoot;
    [SerializeField] private Transform[] _doors;

    [Inject] private ButtonsFactory _buttonsFactory;
    [Inject] private EcsWorld _world;

    private Camera _camera;

    public Camera Camera => _camera ??= _cameraTransform.GetComponentInChildren<Camera>();

    private void Awake()
    {
        if (_buttonsRoot.Length != _doors.Length)
        {
            Debug.LogError("Count of buttons and doors must match!");
            return;
        }
    }

    public void SetupMainCharacter()
    {
        var prefab = Resources.Load<GameObject>("Character");
        var player = Instantiate(prefab, transform);
        player.transform.SetPositionAndRotation(_spawnPlayerTransform.position, _spawnPlayerTransform.rotation);

        var links = player.GetComponents<MonoLinkBase>();
        var newEntity = _world.NewEntity();
        var mainCharacterPool = _world.GetPool<MainCharacterComponent>();
        
        mainCharacterPool.Add(newEntity);

        foreach (var link in links)
        {
            link.Make(ref newEntity, _world);
        }
    }

    public void SetupCamera()
    {
        var links = Camera.GetComponents<MonoLinkBase>();
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

    public void Initialize()
    {
        SetupMainCharacter();
        SetupCamera();
        CreateButtons();
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
        }
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
