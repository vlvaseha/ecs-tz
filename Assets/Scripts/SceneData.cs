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

    private Button[] _buttons;
    private Camera _camera;

    public Transform CameraTransform => _cameraTransform;
    public Camera Camera => _camera ??= _cameraTransform.GetComponentInChildren<Camera>();

    private void Awake()
    {
        if (_buttonsRoot.Length != _doors.Length)
        {
            Debug.LogError("Count of buttons and doors must match!");
            return;
        }

        CreateButtons();
        var colorProperty = Shader.PropertyToID("_Color");

        for (int i = 0; i < _buttons.Length; i++)
        {
            var randomColor = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

            _buttons[i].GetComponent<MeshRenderer>().material.SetColor(colorProperty, randomColor);
            _doors[i].GetComponent<MeshRenderer>().material.SetColor(colorProperty, randomColor);
        }
    }

    public GameObject CreatePlayer()
    {
        var prefab = Resources.Load<GameObject>("Character");
        var player = Instantiate(prefab, transform);
        player.transform.SetPositionAndRotation(_spawnPlayerTransform.position, _spawnPlayerTransform.rotation);

        return player;
    }

    private void CreateButtons()
    {
        _buttons = new Button[_buttonsRoot.Length];

        for (int i = 0; i < _buttonsRoot.Length; i++)
        {
            var button = _buttonsFactory.Create();
            button.transform.SetParent(_buttonsRoot[i]);
            button.transform.localPosition = Vector3.zero;
            
            _buttons[i] = button;
        }
    }
}