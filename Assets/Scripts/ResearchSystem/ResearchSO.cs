using UnityEngine;

public enum ResearchCategory { Living, Science, Resource, Defense, Base }

[CreateAssetMenu(menuName = "ScriptableObjects/ResearchSO", fileName = "ResearchSO")]
public class ResearchSO : ScriptableObject
{
    [Header("Basic Info")]
    public string researchId;  // id convention: "research_[cetegory]_[name]", in camel case
    public string researchName;
    [TextArea] public string description;
    public Sprite icon;
    public ResearchCategory category;
    public int cost = 1;

    [Header("Prerequisites")]
    public ResearchSO[] prerequisites;

    [Header("Unlock State (runtime)")]
    [HideInInspector] public bool isUnlocked;
}
