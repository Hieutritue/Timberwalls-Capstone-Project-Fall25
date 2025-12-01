using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.General;
using DefaultNamespace.TaskSystem;
using UnityEngine;
using Random = System.Random;

namespace ShieldSystem
{
    public class ShieldGenerator : MonoBehaviour
    {
        public List<Transform> ProgressPoints;
        [SerializeField] private Renderer _shieldRenderer;
        [SerializeField] private ShieldMaintainabilityLevelSo[] _shieldMaintainabilitySos;
        [SerializeField] private List<Material> _shieldMaterials;

        [SerializeField] private Vector3Int _origin;
        [SerializeField] private Vector2Int _size;

        private ShieldMaintainabilityLevelSo _currentMaintainabilitySo;
        public int FixerSkillCount { get; set; }

        public List<FixingTask> ActiveTasks { get; set; }
            = new List<FixingTask>();

        private void Start()
        {
            BuildingSystemManager.Instance.PlacementSystem.GetGridData(PlaceableType.Room).RegisterData(_origin,_size);
            BuildingSystemManager.Instance.PlacementSystem.GetGridData(PlaceableType.Furniture).RegisterData(_origin,_size);
            // Shuffle progress points to distribute tasks evenly
            ProgressPoints.Shuffle();
            DefaultNamespace.ShieldSystem.ShieldSystem.Instance.ShieldWall.OnCurrentHealthChanged += CheckTasks;
            DefaultNamespace.ShieldSystem.ShieldSystem.Instance.ShieldWall.OnMaxHealthChanged += CheckTasks;
        }

        private void CheckTasks(float shieldCurrentHp, float shieldMaxHp)
        {
            if (shieldCurrentHp < shieldMaxHp)
            {
                if (ActiveTasks.Count > 0)
                    return;
                for (var i = 0; i < ProgressPoints.Count; i++)
                {
                    ActiveTasks.Add(new FixingTask(this, ProgressPoints[i], TaskType.Fixing));
                }
            }
            else
            {
                foreach (var task in ActiveTasks)
                {
                    task.RemoveTask();
                }

                ActiveTasks.Clear();
            }
        }

        private float _timer;

        private void Update()
        {
            if (FixerSkillCount <= 0)
                return;
            _timer += Time.deltaTime;
            if (_timer >= 1f)
            {
                _timer = 0f;
                DefaultNamespace.ShieldSystem.ShieldSystem.Instance.ShieldWall.CurrentHealth +=
                    FormulaCollection.GetShieldRecoveryRate(
                        _currentMaintainabilitySo.BaseRecoverySpeed,
                        FixerSkillCount);
            }
        }


        public void SetShieldMaintainabilityLevel(int level)
        {
            if (level < 0 || level >= _shieldMaintainabilitySos.Length)
            {
                Debug.LogError("Invalid shield maintainability level: " + level);
                return;
            }

            _currentMaintainabilitySo = _shieldMaintainabilitySos[level];

            SetShieldMaterial(_shieldMaterials[level]);
        }

        public void SetShieldMaterial(Material material)
        {
            if (_shieldRenderer != null)
            {
                _shieldRenderer.material = material;
            }
            else
            {
                Debug.LogError("Shield Renderer is not assigned.");
            }
        }
    }
}