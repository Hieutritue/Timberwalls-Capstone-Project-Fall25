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

    //colonist tasks
    public const string building = "building";
    public const string casino = "casino";
    public const string chicken_coop = "chicken_coop";
    public const string cleaning = "cleaning";
    public const string cooking = "cooking";
    public const string demolition = "demolition";
    public const string eating = "eating";
    public const string fixing = "fixing";
    public const string medicine = "medicine";
    public const string mining = "mining";
    public const string planting = "planting";
    public const string poop = "poop";
    public const string pressing_buttons = "pressing_buttons";
    public const string sleeping = "sleeping";
    public const string typing = "typing";
    public const string washing_tap = "washing_tap";
    public const string wood_chopping_1 = "wood_chopping_1";
    public const string wood_chopping_2 = "wood_chopping_2";
    public const string wood_chopping_3 = "wood_chopping_3";
    public const string wood_chopping_4 = "wood_chopping_4";
    public const string wood_chopping_5 = "wood_chopping_5";
    public const string dancing = "dancing";
    public const string fishing = "fishing";
    public const string sick_person = "sick_person";
   // public const string UI_clicking_sound = "click_heavy";
   // public const string UI_clicking_sound = "click_heavy";

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
    public const string ost_04 = "ost_04";
    public const string ost_05 = "ost_05";

    //enemy sounds
    public const string attack_sound = "laser_shot";

    //misc use in class
    public const string FIXING_WORk = "FIXING_WORK";

    public static readonly List<string> day_ambiences = new List<string>() { ambiance_01, ambiance_02 };
    public static readonly List<string> night_ambiences = new List<string>() { night_ambience_1, night_ambience_2, night_ambience_3 };
    public static readonly List<string> night_osts = new List<string>() { ost_01, ost_02, ost_03, ost_04, ost_05 };

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

    /// <summary>
    /// Generic shuffle function that works with any list of strings
    /// </summary>
    public static string Shuffle(List<string> sourceList, List<string> stored_ambs = null, List<string> valid_ambs = null)
    {
        if (sourceList == null || sourceList.Count == 0) return null;

        if (stored_ambs == null || stored_ambs.Count == sourceList.Count)
            return sourceList[UnityEngine.Random.Range(0, sourceList.Count)];

        // Initialize valid_ambs if null
        if (valid_ambs == null)
        {
            valid_ambs = new List<string>();
        }

        foreach (string s in sourceList)
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

    // -------------------------
    //  Animation → Loop Sound Mapping
    // -------------------------
    private static readonly Dictionary<string, string> ANIM_TO_LOOP_SOUND = new()
    {
        // Machines
        { ColonistAnimationString.PRESSING_BUTTON, pressing_buttons },
        { ColonistAnimationString.TYPING, typing },

        // Resource Gathering
        { ColonistAnimationString.BREAKING_RESOURCE, demolition },

        // Cooking
        { ColonistAnimationString.COOKING, cooking },

        // Farming / Animals
        { ColonistAnimationString.FEEDING_CHICKEN, chicken_coop },
        { ColonistAnimationString.PLANTING, planting },
        { ColonistAnimationString.FISHING, planting }, //missing(?)

        // Medical
        { ColonistAnimationString.SITTING_SICK, sick_person }, //missing(?)
        { ColonistAnimationString.LAYING_SICK, sick_person }, //missing(?)

        // Living
        { ColonistAnimationString.SLEEPING, sleeping },
        { ColonistAnimationString.EATING, eating },
        { ColonistAnimationString.SIT_POOPING, poop },
        { ColonistAnimationString.SQUAT_POOPING, poop },

        // Entertainment
        { ColonistAnimationString.DANCING, dancing }, //missing(?)
        { ColonistAnimationString.PLAYING_POKER, casino },

        // Water
        { ColonistAnimationString.WASHING_TAP, washing_tap },
        { ColonistAnimationString.BATHING, washing_tap }, //missing(?)
        { ColonistAnimationString.SPINNING, washing_tap },//missing(?)

        //misc
        { ColonistAnimationString.BUILDING_WORK, building },
        { ColonistAnimationString.CLEANING, cleaning },
        { ColonistAnimationString.BREAKING_WORK, demolition },
        { FIXING_WORk, fixing },
        //{ ColonistAnimationString.BUILDING_WORK, building },
        //{ ColonistAnimationString.BUILDING_WORK, building },
    };

    /// <summary>
    /// Returns the loop sound name for a given animation trigger.
    /// If no sound exists, returns null.
    /// </summary>
    public static string GetLoopSoundForAnimation(string animTrigger)
    {
        if (string.IsNullOrEmpty(animTrigger))
            return null;

        if (ANIM_TO_LOOP_SOUND.TryGetValue(animTrigger, out string sound))
            return sound;

        return null;
    }
}