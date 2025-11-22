using DefaultNamespace;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.ColonistSystem.UI.Colonist_Selection;
using DefaultNamespace.ScheduleSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;

public class ColonistSpawnManager : MonoSingleton<ColonistSpawnManager>
{
    [Header("Spawn Chance Settings")]
    [SerializeField] private int minDaysBetweenSpawns = 2;
    [SerializeField] private int maxDaysBetweenSpawns = 5;
    [Range(0, 100)] private int successCriteria = 50;

    private void OnEnable()
    {
        GameTimeManager.Instance.OnDayChanged += HandleDayChange;
    }

    private void OnDisable()
    {
        GameTimeManager.Instance.OnDayChanged -= HandleDayChange;
    }

    private void HandleDayChange(int day)
    {
        // prevent spawns at contactPoints = 0
        if (ResourceManager.Instance.Get(ResourceSystem.ResourceType.ContactPoint) <= 0)
        {
            Debug.Log("No contact points available, skipping colonist spawn check.");
            return;
        }

        if (TrySpawnOpportunity())
        {
            ShowColonistSelection();
        }
    }

    private bool TrySpawnOpportunity()
    {
        int score = Mathf.Clamp(successCriteria, 0, 100);
        float successFactor = 0.5f + (score / 100f) * 1.5f;
        float baseDays = Random.Range(minDaysBetweenSpawns, maxDaysBetweenSpawns);
        float interval = baseDays / successFactor;

        float dailyProbability = 1f / interval;

        return Random.value < dailyProbability;
    }

    private int GetWeightedRandomTier(int contactPoints)
    {
        if (contactPoints <= 0)
            return -1;

        contactPoints = Mathf.Clamp(contactPoints, 1, 200);

        float w0 = Mathf.Exp(-contactPoints / 40f);
        float w1 = Mathf.Exp(-(contactPoints - 50) / 40f);
        float w2 = Mathf.Exp(-(contactPoints - 100) / 40f);
        float w3 = Mathf.Exp(-(contactPoints - 150) / 40f);

        float sum = w0 + w1 + w2 + w3;
        w0 /= sum; w1 /= sum; w2 /= sum; w3 /= sum;

        float r = Random.value;

        if (r < w0) return 0;
        r -= w0;
        if (r < w1) return 1;
        r -= w1;
        if (r < w2) return 2;
        return 3;
    }

    private void ShowColonistSelection()
    {
        int contactPoints = ResourceManager.Instance.Get(ResourceSystem.ResourceType.ContactPoint);

        // pool excludes colonists already in the world
        var pool = ColonistManager.Instance.BuildAvailablePool();

        int tierA = GetWeightedRandomTier(contactPoints);
        int tierB = GetWeightedRandomTier(contactPoints);
        int tierC = GetWeightedRandomTier(contactPoints);

        ColonistSO cA = GetRandomColonistOfTier(tierA, pool);
        ColonistSO cB = GetRandomColonistOfTier(tierB, pool);
        ColonistSO cC = GetRandomColonistOfTier(tierC, pool);

        ColonistSelectionPanel.Instance.ShowSpawnChoices();
        ColonistSelectionPanel.Instance.SetColonists(cA, cB, cC);
    }

    private ColonistSO GetRandomColonistOfTier(int desiredTier, List<ColonistSO> pool)
    {
        if (desiredTier < 0 || pool.Count == 0)
            return null;

        // try to find colonist of desired tier or lower
        for (int tier = desiredTier; tier >= 0; tier--)
        {
            var candidates = pool.Where(c => c.Tier == tier).ToList();
            if (candidates.Count > 0)
            {
                ColonistSO chosen = candidates[Random.Range(0, candidates.Count)];
                pool.Remove(chosen);
                return chosen;
            }
        }

        // Emergency fallback
        ColonistSO fallback = pool[Random.Range(0, pool.Count)];
        pool.Remove(fallback);
        return fallback;
    }

    [Button]
    public void TriggerSpawnEvent()
    {
        HandleDayChange(0);
    }
}
