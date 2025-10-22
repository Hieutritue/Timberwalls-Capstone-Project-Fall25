using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ResearchTree", fileName = "ResearchTree")]
public class ResearchTreeSO : ScriptableObject
{
    public ResearchCategory category;
    public List<ResearchSO> researches = new();
}
