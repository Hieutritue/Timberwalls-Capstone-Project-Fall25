using TMPro;
using UnityEngine;

public class Subcategory : MonoBehaviour
{
    [SerializeField] private GameObject itemContainer;
    [SerializeField] private TextMeshProUGUI subcategoryName;

    public GameObject GetItemContainer()
    {
        return itemContainer;
    }

    public TextMeshProUGUI GetSubcategoryName()
    {
        return subcategoryName;
    }
}
