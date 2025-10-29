using System;
using System.Collections;
using TMPro;
using UnityEngine;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEngine.UI;

public class ItemWindowManager : MonoBehaviour
{
    [SerializeField] private GameObject subcategoryLayer;
    [SerializeField] private GameObject subcategoryPrefab;
    [SerializeField] private TextMeshProUGUI categoryName;
    public static ItemWindowManager Instance;
    private readonly float _originalItemWindowHeight = 200f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void GenerateItems(CategorySO categoryClicked)
    {
        if (!gameObject.activeInHierarchy)
            gameObject.SetActive(true);

        RectTransform rectTransform = GetComponent<RectTransform>();

        //Reset previous subcategories
        foreach (Transform child in subcategoryLayer.transform)
        {
            Destroy(child.gameObject);
        }

        //Reset height to original
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _originalItemWindowHeight);
        categoryName.text = StringTools.SplitCamelCase(categoryClicked.category.ToString());
        //Generate new subcategories
        if (categoryClicked.subCategories.Count > 0)
        {
            foreach (var subCategory in categoryClicked.subCategories)
            {
                var newSubCategory = Instantiate(subcategoryPrefab, subcategoryLayer.transform);
                var subcategoryComp = newSubCategory.GetComponent<Subcategory>();

                GameObject subcategoryItemContainer = subcategoryComp.GetItemContainer();
                TextMeshProUGUI subcategoryName = subcategoryComp.GetSubcategoryName();
                subcategoryName.SetText(subCategory.subCategoryName);

                //Generate items
                foreach (var item in subCategory.items)
                {
                    var newItem = Instantiate(item.Prefab, subcategoryItemContainer.transform);
                    if (item.Icon != null)
                        newItem.GetComponent<Image>().sprite = item.Icon;
                }

                //Resize subcategory height (keep your logic)
                RectTransform subcategoryPrefabRect = newSubCategory.GetComponent<RectTransform>();
                RectTransform subcategoryItemContainerRect = subcategoryItemContainer.GetComponent<RectTransform>();

                LayoutRebuilder.ForceRebuildLayoutImmediate(subcategoryItemContainerRect);

                float subcategoryPrefabHeight = subcategoryPrefabRect.rect.height;
                float newSubcategoryPrefabHeight = subcategoryPrefabHeight + subcategoryItemContainerRect.rect.height;

                subcategoryPrefabRect.SetSizeWithCurrentAnchors(
                    RectTransform.Axis.Vertical,
                    newSubcategoryPrefabHeight
                );
            }
        }

        //Recalculate layout height properly after Unity updates UI
        StartCoroutine(RecalculateLayoutHeight(rectTransform));
    }

    private IEnumerator RecalculateLayoutHeight(RectTransform rectTransform)
    {
        // Wait until end of frame to allow Unity’s layout system to update everything
        yield return new WaitForEndOfFrame();

        RectTransform subcategoryLayerRectTransform = subcategoryLayer.GetComponent<RectTransform>();
        LayoutRebuilder.ForceRebuildLayoutImmediate(subcategoryLayerRectTransform);

        float totalHeight = _originalItemWindowHeight + subcategoryLayerRectTransform.rect.height;
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, totalHeight);
    }
}