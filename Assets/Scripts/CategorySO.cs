using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CategorySO", menuName = "ScriptableObjects/Category/CategorySO")]
public class CategorySO : ScriptableObject
{
   public BuildingCategory category;
   public List<SubCategorySO> subCategories;
}
