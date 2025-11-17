using System;
using System.Collections.Generic;

namespace DefaultNamespace.ResearchSystem
{
    public class ResearchManager : MonoSingleton<ResearchManager>
    {
        public Dictionary<ResearchSO, bool> UnlockedResearch = new();
        public Dictionary<PlaceableSO, bool> UnlockedBuildings = new();

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

            // resource check here

            return true;
        }

        public bool Unlock(ResearchSO research)
        {
            if (!CanUnlock(research))
                return false;

            // resource deduction here

            UnlockedResearch[research] = true;

            // mark buildings unlockable
            foreach (var b in research.unlocksBuildings)
                UnlockedBuildings[b] = true;

            OnResearchUnlocked?.Invoke(research);
            return true;
        }
    }
}