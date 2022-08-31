using Leopotam.EcsLite;

namespace Systems
{
    public class InputSystem : IEcsRunSystem
    {
        private readonly IInputService _inputService;
        
        public InputSystem(IInputService inputService)
        {
            _inputService = inputService;
        }

        public void Run(IEcsSystems systems)
        {
            if (_inputService.GetClick())
            {
                var onClickPool = systems.GetWorld().GetPool<OnClickedTag>();
                onClickPool.Add(systems.GetWorld().NewEntity());
            }
        }
    }
}
