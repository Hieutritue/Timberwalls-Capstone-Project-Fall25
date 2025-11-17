using System;
using System.Collections.Generic;
using DefaultNamespace.General;

namespace DefaultNamespace.ResearchSystem
{
    public class ResearchManager : MonoSingleton<ResearchManager>
    {
        public Dictionary<PlaceableSO,bool> UnlockedBuildings = new();

        private void Start()
        {
            InitializeUnlockedBuildings();
        }

        private void InitializeUnlockedBuildings()
        {
            var allBuildings = DataTable.Instance.BuildingsCollectionSo.AllBuildings;
            foreach (var building in allBuildings)
            {
                UnlockedBuildings[building] = true; // TODO: Initially, all buildings are locked
            }
        }
    }
}