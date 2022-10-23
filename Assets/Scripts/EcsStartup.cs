using System;
using Systems;
using Leopotam.EcsLite;
using Zenject;

public class EcsStartup : IInitializable, ITickable, IDisposable
{ 
	[Inject] private EcsWorld _world;
	[Inject] private SceneData _sceneData;
	[Inject] private DiContainer _diContainer;
	
	private IEcsSystems _updateSystem;

	public void Initialize()
	{
		_sceneData.Initialize();
		_updateSystem = new EcsSystems(_world);

		_updateSystem
			.Add(GetSystemInjected<MoveSystem>())
			.Add(GetSystemInjected<MotionSystem>())
			.Add(GetSystemInjected<RotationSystem>())
			.Add(GetSystemInjected<CameraFollowSystem>())
			.Add(new InputSystem())
			.Add(new AnimatorStateSystem())
			.Add(new UpdateMovingAnimStateSystem())
			.Add(new ButtonsStateControllerSystem())
			.Add(new DoorsSystem())
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
