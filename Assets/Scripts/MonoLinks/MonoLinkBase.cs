using Leopotam.EcsLite;
using UnityEngine;

namespace MonoLinks
{
    public abstract class MonoLinkBase : MonoBehaviour
    {
        public abstract void Make(ref int entity, EcsWorld world);
    }
}