using Systems;
using Leopotam.EcsLite;
using UnityEngine;

public class EcsStartup : MonoBehaviour
{ 
	private EcsWorld _world;
	private EcsSystems _systems;

	private void Start()
	{
		_world = new EcsWorld();
		_systems = new EcsSystems(_world);

		_systems.Add(new InputSystem());
		
		_systems
			.Init();
	}

	private void Update()
	{
		_systems?.Run();
	}

	private void OnDestroy()
	{
		if (_systems != null)
		{
			_systems.Destroy();
			_systems = null;
		}

		if (_world != null)
		{
			_world.Destroy();
			_world = null;
		}
	}
}
