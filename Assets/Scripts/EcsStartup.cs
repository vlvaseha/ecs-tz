using System;
using Systems;
using Leopotam.EcsLite;
using Zenject;

public class EcsStartup : IInitializable, ITickable, IDisposable
{ 
	private IEcsSystems _updateSystem;

	[Inject] private EcsWorld _world;
	[Inject] private SceneData _sceneData;
	[Inject] private DiContainer _diContainer;

	public void Initialize()
	{
		_sceneData.Initialize();
		_updateSystem = new EcsSystems(_world);

		_updateSystem
			.Add(new PlayerInitSystem())
			.Add(new InputSystem())
			.Add(GetSystemInjected<MoveSystem>())
			.Add(GetSystemInjected<MotionSystem>())
			.Add(GetSystemInjected<RotationSystem>())
			.Add(GetSystemInjected<CameraFollowSystem>())
			.Add(new AnimatorStateSystem())
			.Add(new UpdateMovingAnimStateSystem())
			.Add(new ButtonsStateControllerSystem())
			// .Add(new ButtonsStateSystem(_sceneData.Buttons))
			// .Add(new DoorsSystem())
			.Init();
	}

	public void Tick()
	{
		_updateSystem?.Run();
	}

	public void Dispose()
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
	
	private T GetSystemInjected<T>() where T : class, new()
	{
		var obj = new T();
		_diContainer.Inject(obj);

		return obj;
	}
}
