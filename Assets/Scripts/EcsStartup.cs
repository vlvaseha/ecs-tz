using Systems;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

public class EcsStartup : MonoBehaviour
{ 
	private IEcsSystems _updateSystem;

	[Inject] private EcsWorld _world;
	[Inject] private GameSettings _settings;
	[Inject] private SceneData _sceneData;

	private void Start()
	{
		_updateSystem = new EcsSystems(_world);

		var cameraTransform = _sceneData.CameraTransform;
		var player = _sceneData.CreatePlayer();
		
		var playerRotator = new ObjectRotator(player.transform);
		var playerMover = new ObjectMover(player.transform);
		var cameraMover = new ObjectMover(cameraTransform);
		var playerPositionCalculator = new PlayerPositionCalculator(_sceneData.Camera);
		var playerInput = new StandaloneInput();
		var animatorController = new AnimatorStateUpdater(player.GetComponent<Animator>());

		_updateSystem
			.Add(new PlayerInitSystem())
			.Add(new InputSystem(playerInput))
			.Add(new PlayerPositionCalculationSystem(playerPositionCalculator, playerMover))
			.Add(new MovementSystem(playerMover, _settings.PlayerMoveSpeed))
			.Add(new RotationSystem(playerRotator, _settings.PlayerRotationSpeed))
			.Add(new FollowerSystem(cameraMover, playerMover, _settings.CameraFollowSmoothTime))
			.Add(new AnimationStateCalculationSystem(playerMover))
			.Add(new AnimationStateUpdateSystem(animatorController))
			.Init();
	}

	private void Update()
	{
		_updateSystem?.Run();
	}

	private void OnDestroy()
	{
		if (_updateSystem != null)
		{
			_updateSystem.Destroy();
			_updateSystem = null;
		}

		if (_world != null)
		{
			_world.Destroy();
			_world = null;
		}
	}
}
