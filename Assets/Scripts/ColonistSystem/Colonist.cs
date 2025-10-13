using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace DefaultNamespace.ColonistSystem
{
    public class Colonist : MonoBehaviour
    {
        public ColonistSO ColonistSo;
        private Dictionary<StatType, StatRuntime> _statDict = new();

        private void Start()
        {
            InitData();
        }

        private void InitData()
        {
            foreach (var stat in ColonistSo.Stats)
                _statDict[stat.StatType] = new StatRuntime(stat);
        }
    }
}