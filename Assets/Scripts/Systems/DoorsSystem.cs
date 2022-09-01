using System.Collections.Generic;
using Components;
using Leopotam.EcsLite;
using UnityEngine;

public class DoorsSystem : IEcsInitSystem, IEcsRunSystem
{
    private const float HiddenDoorYPos = -3;
    private const float HidingSpeed = 1f;
    
    private readonly IMovable[] _doors;
    private readonly Dictionary<int, IMovable> _doorEntities;

    public DoorsSystem(IMovable[] doors)
    {
        _doors = doors;
        _doorEntities = new Dictionary<int, IMovable>(doors.Length);
    }
    
    public void Init(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var doorsPool = world.GetPool<DoorComponent>();

        foreach (var door in _doors)
        {
            var doorEntity = world.NewEntity();
            doorsPool.Add(doorEntity);
            
            _doorEntities.Add(doorEntity, door);
        }
        
        var doorsEntities = world.Filter<DoorComponent>().End();
        var buttonsEntities = GetButtonsEntities(world);

        var counter = 0;
        foreach (var entity in doorsEntities)
        {
            ref var doorComponent = ref doorsPool.Get(entity);
            doorComponent.linkedButtonEntity = buttonsEntities[counter];
            doorComponent.doorEntity = entity;

            counter++;

            if (counter >= buttonsEntities.Count)
                counter--;
        }
    }

    public void Run(IEcsSystems systems)
    {
        var world = systems.GetWorld();
        var doorsEntities = world.Filter<DoorComponent>().End();
        var doorsPool = world.GetPool<DoorComponent>();

        foreach (var entity in doorsEntities)
        {
            var doorComponent = doorsPool.Get(entity);

            if (doorComponent.isOpening)
            {
                var door = _doorEntities[doorComponent.doorEntity];
                var doorCurrentPosition = door.GetPosition();
                var doorTargetPosition = doorCurrentPosition + Vector3.up * HiddenDoorYPos;
                var position = Vector3.MoveTowards(doorCurrentPosition, doorTargetPosition, 
                        HidingSpeed * Time.deltaTime);
                
                door.SetPosition(position);
            }
        }
    }

    private List<int> GetButtonsEntities(EcsWorld world)
    {
        var list = new List<int>();

        foreach (var entity in world.Filter<ButtonComponent>().End())
            list.Add(entity);
        
        return list;
    }
}
