using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TabCategorySO", menuName = "ScriptableObjects/Tab/TabCategorySO")]
public class TabCategorySO : ScriptableObject
{
    public string categoryName;
    public List<ItemSO> items;
}
