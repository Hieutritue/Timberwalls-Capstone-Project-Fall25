using DefaultNamespace.PlaceableInstances;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class RoomEffect : ScriptableObject
    {
        public abstract void Apply(RoomPlaceableInstance roomPlaceableInstance);
        public abstract void Remove(RoomPlaceableInstance roomPlaceableInstance);
    }
}