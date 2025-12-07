using System;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using UnityEngine;

public class GlobalSoundNameHolder : MonoBehaviour
{
    private static GlobalSoundNameHolder _instance;
    public static GlobalSoundNameHolder Instance
    {
        get
        {
            if (_instance == null)
            {
                // Try to find existing
                _instance = FindFirstObjectByType<GlobalSoundNameHolder>();

                // No registry in scene → create one
                if (_instance == null)
                {
                    GameObject obj = new GameObject("GlobalSoundNameHolder");
                    _instance = obj.AddComponent<GlobalSoundNameHolder>();
                    DontDestroyOnLoad(obj);
                }
            }

            return _instance;
        }
    }

    //UI clicking
    public const string UI_clicking_sound = "click_heavy";
    public const string UI_clicking_sound_2 = "click_light";
    public const string UI_clicking_sound_3 = "r_click";
    public const string UI_clicking_sound_4 = "900_click";

    //ost and ambiance
    public const string ambiance_01 = "ambiance_01";
    public const string ambiance_02 = "ambiance_02";
    public const string night_ambience_1 = "night_ambience_1";
    public const string night_ambience_2 = "night_ambience_2";
    public const string night_ambience_3 = "night_ambience_3";
    public const string ost_01 = "ost_01";
    public const string ost_02 = "ost_02";
    public const string ost_03 = "ost_03";

    public static readonly List<string> day_ambiences = new List<string>() { ambiance_01, ambiance_02 };
    public static readonly List<string> night_ambiences = new List<string>() { night_ambience_1, night_ambience_2, night_ambience_3 };

    public static string shuffle_day_ambiences(List<string> stored_ambs = null, List<string> valid_ambs = null)
    {
        if (stored_ambs == null || stored_ambs.Count == day_ambiences.Count)
            return day_ambiences[UnityEngine.Random.Range(0, day_ambiences.Count)];

        // Initialize valid_ambs if null
        if (valid_ambs == null)
        {
            valid_ambs = new List<string>();
        }

        foreach (string s in day_ambiences)
        {
            bool has = false;
            foreach (string a in stored_ambs)
            {
                if (s == a) has = true;
            }

            if (!has) valid_ambs.Add(s);
        }

        if (valid_ambs.Count == 0) return null;

        return valid_ambs[UnityEngine.Random.Range(0, valid_ambs.Count)];
    }

    public static string shuffle_night_ambiences(List<string> stored_ambs = null, List<string> valid_ambs = null)
    {
        if (stored_ambs == null || stored_ambs.Count == night_ambiences.Count) return night_ambiences[UnityEngine.Random.Range(0, night_ambiences.Count)];

        // Initialize valid_ambs if null
        if (valid_ambs == null)
        {
            valid_ambs = new List<string>();
        }

        foreach (string s in night_ambiences)
        {
            bool has = false;
            foreach (string a in stored_ambs)
            {
                if (s == a) has = true;
            }

            if (!has) valid_ambs.Add(s);
        }

        if (valid_ambs.Count == 0) return null;

        return valid_ambs[UnityEngine.Random.Range(0, valid_ambs.Count)];
    }
}