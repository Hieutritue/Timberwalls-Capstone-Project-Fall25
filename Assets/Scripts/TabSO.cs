using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TabSO", menuName = "ScriptableObjects/Tab/TabSO")]
public class TabSO : ScriptableObject
{
   public string tabName;
   public List<TabCategorySO> tabCategories;
}
