using System.Security.Cryptography;
using Systems;
using Leopotam.EcsLite;
using UnityEngine;
using Zenject;

public class EcsStartup : MonoBehaviour
{ 
	private IEcsSystems _initSystem;
	private IEcsSystems _updateSystem;

	[Inject] private EcsWorld _world;
	[Inject] private GameSettings _settings;

	private void Start()
	{
		_initSystem = new EcsSystems(_world, "InitSystem");
		_updateSystem = new EcsSystems(_world, "UpdateSystem");
		
		_initSystem
			.Add(new PlayerInitSystem())
			.Init();

		var camera = Camera.main.transform;
		var player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
		player.transform.position = Vector3.zero;

		_updateSystem
			.Add(new InputSystem(new StandaloneInput()))
			.Add(new PlayerPositionCalculationSystem(new PlayerPositionCalculator(Camera.main)))
			.Add(new MovementSystem(new ObjectMover(player.transform), 2f))
			.Add(new FollowerSystem(new ObjectMover(camera), new ObjectMover(player.transform), 5f))
			.Init();
		
	}

	private void Update()
	{
		_updateSystem?.Run();
	}

	private void OnDestroy()
	{
		if (_initSystem != null)
		{
			_initSystem.Destroy();
			_initSystem = null;
		}
		
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
