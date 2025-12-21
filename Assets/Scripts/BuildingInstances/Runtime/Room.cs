using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.StateMachine;
using BuildingSystem.RoomStates;
using BuildingSystem.SpecificRoom;
using DefaultNamespace;
using DefaultNamespace.PlaceableInstances;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BuildingSystem
{
    public class Room : Building
    {
        protected RoomPlaceableInstance _roomPlaceableInstance;
        [ReadOnly] public SpecificRoomSo CurrentSpecificRoomSo;
        [SerializeField] private List<SpecificRoomModel> _specificRoomModels;

        public void InitRoom()
        {
            _roomPlaceableInstance = GetComponent<RoomPlaceableInstance>();
            _roomPlaceableInstance.OnAddFurnitureToRoom += OnContainingItemsAdded;
        }

        private void OnContainingItemsAdded(Furniture furniture)
        {
            furniture.OnConstructed += EvaluateRoomSpecifics;
            furniture.OnConstructed += () => furniture.ContainingRoom = this;
            furniture.OnDemolished += EvaluateRoomSpecifics;
        }

        private void EvaluateRoomSpecifics()
        {
            StartCoroutine(DelayCheckRoomFurniture());
            _specificRoomModels.ForEach(r => StartCoroutine(DelayCheckRoomRequirement(r)));
        }

        IEnumerator DelayCheckRoomFurniture()
        {
            yield return new WaitForSeconds(1);
            CheckRoomFurniture();
        }

        IEnumerator DelayCheckRoomRequirement(SpecificRoomModel r)
        {
            yield return new WaitForSeconds(0.2f);
            CheckRoomRequirements(r);
        }

        public virtual void CheckRoomFurniture()
        {
        }

        private void CheckRoomRequirements(SpecificRoomModel r)
        {
            var meetsRequirements = true;
            var requiredSubCategories = r.SpecificRoomSo.RequiredFurnitureSubCategories;
            var forbiddenSubCategories = r.SpecificRoomSo.MustNotHaveFurnitureSubCategories;
            var containedItems = _roomPlaceableInstance.ContainedItems;
            // Check required sub-categories
            requiredSubCategories.ForEach(rs =>
            {
                meetsRequirements &= containedItems.Exists(i =>
                    !i.Building.IsUnderConstruction() && i.PlaceableSo.SubCategory == rs);
            });
            // Check forbidden sub-categories
            forbiddenSubCategories.ForEach(fs =>
            {
                meetsRequirements &= !containedItems.Exists(i =>
                    !i.Building.IsUnderConstruction() && i.PlaceableSo.SubCategory == fs);
            });
            if (meetsRequirements)
            {
                r.RoomGameObject.SetActive(true);

                if (CurrentSpecificRoomSo && CurrentSpecificRoomSo != r.SpecificRoomSo)
                {
                    _specificRoomModels.First(sr => sr.SpecificRoomSo == CurrentSpecificRoomSo).RoomGameObject
                        .SetActive(false);
                }

                CurrentSpecificRoomSo = r.SpecificRoomSo;
            }
            else
            {
                r.RoomGameObject.SetActive(false);
                if (CurrentSpecificRoomSo == r.SpecificRoomSo)
                {
                    CurrentSpecificRoomSo = null;
                }
            }
        }
    }

    [Serializable]
    public class SpecificRoomModel
    {
        public SpecificRoomSo SpecificRoomSo;
        public GameObject RoomGameObject;
    }
}