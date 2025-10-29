using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SubCategorySO", menuName = "ScriptableObjects/Category/SubCategorySO")]
public class SubCategorySO : ScriptableObject
{
    public string subCategoryName;
    public List<PlaceableSO> items;
}
