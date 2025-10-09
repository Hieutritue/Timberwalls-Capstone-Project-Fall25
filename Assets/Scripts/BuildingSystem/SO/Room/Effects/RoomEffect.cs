using DefaultNamespace.PlaceableInstances;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class RoomEffect : ScriptableObject
    {
        public abstract void Apply(RoomInstance roomInstance);
        public abstract void Remove(RoomInstance roomInstance);
    }
}