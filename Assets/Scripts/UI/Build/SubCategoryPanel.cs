using System.Collections.Generic;
using System.Linq;
using DefaultNamespace.ResearchSystem;
using TMPro;
using UnityEngine;

namespace DefaultNamespace.UI.Build
{
    public class SubCategoryPanel : MonoBehaviour
    {
        public BuildingSubCategory SubCategory;
        [SerializeField] private TMP_Text _subCategoryText;
        [SerializeField] private BuildingUI _buildingUIPrefab;
        private List<BuildingUI> _buildingUIs = new List<BuildingUI>();
        
        public void Initialize(BuildingSubCategory subCategory)
        {
            SubCategory = subCategory;
            _subCategoryText.text = StringTools.SplitCamelCase(subCategory.ToString());
            LoadBuildings();

            // foreach (var placeable in placeables)
            // {
            //     GameObject buildingUIGameObject = Instantiate(BuildMenuManager.Instance.buildingUIPrefab, transform);
            //     BuildingUI buildingUI = buildingUIGameObject.GetComponent<BuildingUI>();
            //     buildingUI.Init(placeable);
            //     _buildingUIs.Add(buildingUI);
            // }
            gameObject.SetActive(false);
        }
        
        public void LoadBuildings()
        {
            // remove all children first
            foreach (var buildingUI in _buildingUIs)
            {
                Destroy(buildingUI.gameObject);
            }
            var placeables = ResearchManager.Instance.UnlockedBuildings.Where(kvp => kvp.Key.SubCategory == SubCategory && kvp.Value)
                .Select(kvp => kvp.Key).ToList();
            foreach (var placeable in placeables)
            {
                var buildingUI = Instantiate(_buildingUIPrefab, transform);
                buildingUI.Init(placeable);
                _buildingUIs.Add(buildingUI);
            }
        }
    }
}