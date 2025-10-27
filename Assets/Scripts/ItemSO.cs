using UnityEngine;

[CreateAssetMenu(fileName = "ItemSO", menuName = "ScriptableObjects/Item/ItemSO")]
public class ItemSO : ScriptableObject
{
   public GameObject itemPrefab;
   public string name;
   public string description;
}
