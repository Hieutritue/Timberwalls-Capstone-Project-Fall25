using System;
using System.Collections.Generic;
using DefaultNamespace.General;

namespace DefaultNamespace.ResearchSystem
{
    public class ResearchManager : MonoSingleton<ResearchManager>
    {
        public Dictionary<ResearchSO, bool> UnlockedResearch = new();
        public Dictionary<PlaceableSO, bool> UnlockedBuildings = new();

        private void Start()
        {
            // TODO: Set ngược lại khi load game
            DataTable.Instance.BuildingsCollectionSo.AllBuildings.ForEach(b =>
            {
                UnlockedBuildings[b] = !b.InitiallyUnlocked;
            });
        }

        public event Action<ResearchSO> OnResearchUnlocked;

        public bool IsUnlocked(ResearchSO research)
        {
            return UnlockedResearch.TryGetValue(research, out bool val) && val;
        }

        public bool CanUnlock(ResearchSO research)
        {
            foreach (var pre in research.prerequisites)
            {
                if (!IsUnlocked(pre))
                    return false;
            }

            foreach (var costEntry in research.Costs)
            {
                if (ResourceManager.Instance.Get(costEntry.Resource.ResourceType) < costEntry.Amount)
                    return false;
            }

            return true;
        }

        public bool Unlock(ResearchSO research)
        {
            if (!CanUnlock(research))
                return false;

            foreach (var costEntry in research.Costs)
            {
                ResourceManager.Instance.Set(costEntry.Resource.ResourceType, ResourceManager.Instance.Get(costEntry.Resource.ResourceType) - costEntry.Amount);
            }

            UnlockedResearch[research] = true;

            // mark buildings unlocked
            foreach (var b in research.unlocksBuildings)
                UnlockedBuildings[b] = true;

            OnResearchUnlocked?.Invoke(research);
            return true;
        }
    }
}