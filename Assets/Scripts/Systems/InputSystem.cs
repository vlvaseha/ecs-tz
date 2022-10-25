using Components;
using Leopotam.EcsLite;
using Services;
using Zenject;

namespace Systems
{
    public class InputSystem : IEcsRunSystem
    {
        [Inject] private InputService _inputService;
        
        public void Run(IEcsSystems systems)
        {
            var hasInput = _inputService.GetClick();
            
            if (!hasInput)
                return;

            var world = systems.GetWorld();
            var onClickPool = world.GetPool<OnClickedTag>();
            onClickPool.Add(world.NewEntity());
        }
    }
}
