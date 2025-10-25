using System;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace DefaultNamespace.ColonistSystem
{
    public class ColonistManager : MonoSingleton<ColonistManager>
    {
        private List<Colonist> _colonists { get; set; } = new List<Colonist>();
        
        public List<Colonist> Colonists => _colonists;
        public Action<Colonist> OnColonistAdded;
        public Action<Colonist> OnColonistRemoved;
        
        public void AddColonist(Colonist colonist)
        {
            if (!_colonists.Contains(colonist))
            {
                _colonists.Add(colonist);
                OnColonistAdded?.Invoke(colonist);
            }
        }
        public void RemoveColonist(Colonist colonist)
        {
            if (_colonists.Contains(colonist))
            {
                _colonists.Remove(colonist);
                OnColonistRemoved?.Invoke(colonist);
            }
        }
    }
}