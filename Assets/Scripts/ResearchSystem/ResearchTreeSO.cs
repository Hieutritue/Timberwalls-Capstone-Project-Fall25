using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Research/ResearchTree", fileName = "ResearchTreeSO")]
public class ResearchTreeSO : ScriptableObject
{
    public ResearchCategory category;
    public List<ResearchSO> researches = new();
}
