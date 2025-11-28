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

    private int _nextSpawnDay = -1;

    private void Start()
    {
        GameTimeManager.Instance.OnDayChanged += HandleDayChange;
    }

    private void HandleDayChange(int day)
    {
        if (day >= _nextSpawnDay)
        {
            if (ResourceManager.Instance.Get(ResourceSystem.ResourceType.ContactPoint) > 0)
            {
                if (_nextSpawnDay > 0)
                {
                    Debug.Log($"Triggering colonist selection event on day {day}");
                    ShowColonistSelection();
                }

                ScheduleNextSpawn(day);
            }
            else
            {
                Debug.Log("Recruitment event scheduling skipped.");
            }
        }
        else
        {
            Debug.Log($"Spawn not due yet. Day {day}, next spawn at day {_nextSpawnDay}");
        }
    }

    private void ScheduleNextSpawn(int currentDay)
    {
        int score = Mathf.Clamp(successCriteria, 0, 100);

        // map score 0–100 to factor 0.5–2
        float successFactor = 0.5f + (score / 100f) * 1.5f;

        float scaledMin = minDaysBetweenSpawns / successFactor;
        float scaledMax = maxDaysBetweenSpawns / successFactor;

        int daysToNext = Mathf.RoundToInt(Random.Range(scaledMin, scaledMax));

        daysToNext = Mathf.Max(daysToNext, 1);

        _nextSpawnDay = currentDay + daysToNext;

        Debug.Log($"Next colonist spawn scheduled for day: {_nextSpawnDay} (in {daysToNext} days)");
    }

    private int GetWeightedRandomTier(int contactPoints)
    {
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
        if (pool == null || pool.Count == 0)
            return null;

        int contactPoints = ResourceManager.Instance.Get(ResourceSystem.ResourceType.ContactPoint);

        while (true)
        {
            var candidates = pool.Where(c => c.Tier == desiredTier).ToList();

            if (candidates.Count > 0)
            {
                ColonistSO chosen = candidates[Random.Range(0, candidates.Count)];
                pool.Remove(chosen);
                return chosen;
            }

            desiredTier = GetWeightedRandomTier(contactPoints);
        }
    }


    [Button]
    public void TriggerSpawnEvent()
    {
        ShowColonistSelection();
    }
}

