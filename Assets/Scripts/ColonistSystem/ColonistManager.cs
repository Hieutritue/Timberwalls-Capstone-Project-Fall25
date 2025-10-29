using System;
using System.Collections.Generic;
using DefaultNamespace.TaskSystem;
using UnityEngine;

namespace DefaultNamespace.ColonistSystem
{
    public class ColonistManager : MonoSingleton<ColonistManager>
    {
        [field: SerializeField] public List<Colonist> Colonists { get; set; }

        public Action<Colonist> OnColonistAdded;
        public Action<Colonist> OnColonistRemoved;


        private void Start()
        {
            OnColonistAdded += TaskPriorityMatrix.Instance.AddRow;
            OnColonistRemoved += TaskPriorityMatrix.Instance.RemoveRow;
            
            TaskPriorityMatrix.Instance.Setup();
        }


        public void AddColonist(Colonist colonist)
        {
            if (!Colonists.Contains(colonist))
            {
                Colonists.Add(colonist);
                OnColonistAdded?.Invoke(colonist);
            }
        }

        public void RemoveColonist(Colonist colonist)
        {
            if (Colonists.Contains(colonist))
            {
                Colonists.Remove(colonist);
                OnColonistRemoved?.Invoke(colonist);
            }
        }
    }
}