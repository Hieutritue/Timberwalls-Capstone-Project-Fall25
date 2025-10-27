using System;
using TMPro;
using UnityEngine;

public class ItemWindowManager : MonoBehaviour
{
    [SerializeField] private GameObject categoryLayerPrefab;
    [SerializeField] private GameObject categoryItemContainer;
    [SerializeField] private TextMeshProUGUI categoryName;
    [SerializeField] private TextMeshProUGUI tabName;
    public static ItemWindowManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void GenerateItems(TabSO tabClicked)
    {
        if(!gameObject.activeInHierarchy) gameObject.SetActive(true);
        tabName.text = tabClicked.tabName;
        if (categoryItemContainer.transform.childCount <= 0)
        {
            foreach (var category in tabClicked.tabCategories)
            {
                categoryName.SetText(category.categoryName);
                foreach (var item in category.items)
                {
                    Instantiate(item.itemPrefab, categoryItemContainer.transform);
                }
            }
        }
    }
}
