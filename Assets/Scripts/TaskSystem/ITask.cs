using System;
using DefaultNamespace.ColonistSystem;
using UnityEngine;

namespace DefaultNamespace.TaskSystem
{
    public interface ITask
    {
        Transform Transform { get; }
        void Create();
        void Complete();
        void UpdateProgress(Colonist colonist);
        Action OnComplete { get; set; }
    }
}