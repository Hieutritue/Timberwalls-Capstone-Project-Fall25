using System.Collections.Generic;
using BuildingSystem;
using DefaultNamespace.ColonistSystem;
using DefaultNamespace.General;
using ResourceSystem.Storage;
using UnityEngine;

namespace DefaultNamespace.TaskSystem
{
    public class ResourceGatheringTask : AProgressTask
    {
        private TaskType _taskType;
        public ResourceGatheringTask(Building building, TaskType taskType) : base(building, taskType)
        {
        }
        
        public bool ResourceReachedMaxCapacity()
        {
            var resourceType = ((ResourceGatheringFurniture)_building).GatheringFurnitureSo.OutputResource[0].Resource.ResourceType;
            var maxCapacity = ResourceStorageManager.Instance.GetMaxCapacityForResourceType(resourceType);
            
            return ResourceManager.Instance.Get(resourceType) >= maxCapacity;
        }
        
        public bool NotEnoughResourceRequiredToProduce()
        {
            var gatheringFurniture = (ResourceGatheringFurniture)_building;
            foreach (var input in gatheringFurniture.GatheringFurnitureSo.Consumption)
            {
                var currentAmount = ResourceManager.Instance.Get(input.Resource.ResourceType);
                if (currentAmount < input.Amount)
                    return true;
            }

            return false;
        }

        public override float TotalProgress(Colonist colonist)
        {
            if (_building is ResourceGatheringFurniture gatheringFurniture)
            {
                var skillLevel = colonist.ColonistSo.Skills[gatheringFurniture.GatheringFurnitureSo.TaskType.SkillForTask()];
                return FormulaCollection.ProgressPerFrameBasedOnSkillLevel(
                    gatheringFurniture.GatheringFurnitureSo.BaseTimeToProduce,
                    skillLevel,
                    colonist.TaskCompletionSpeedMultiplier);
            }

            return 0;
        }

        public override void RewardComplete()
        {
            if (_building is ResourceGatheringFurniture gatheringFurniture)
            {
                gatheringFurniture.Work();
                // gatheringFurniture.CreateTask();
            }
        }

        public override void UpdateProgress(Colonist colonist)
        {
            SetColonistPosition(colonist);
            base.UpdateProgress(colonist);
        }
        
        
        private void SetColonistPosition(Colonist colonist)
        {
            if (Building is ResourceGatheringFurniture furniture)
            {
                colonist.transform.position = furniture.ActionPoint.position;
                colonist.transform.LookAt(furniture.ActionPoint.position + furniture.ActionPoint.forward);
            }
        }
        
        public override void ColonistStartWork(Colonist colonist)
        {
            // TODO: Animation
            colonist.animator.SetTrigger(ColonistAnimationString.WORKING);
            colonist.animator.SetTrigger(ColonistAnimationString.FURNITURE_WORK);
            var tag = _building.tag;
            var animString = FurnitureTag.GetAnimStringBaseOnFurniture(tag);
            if(!string.IsNullOrEmpty(animString))
            colonist.animator.SetTrigger(animString);
            else
            {
                Debug.LogError("No Anim String Found For" + tag);
            }
        }

        public override void ColonistStopWork(Colonist colonist)
        {
            // TODO: Animation
            colonist.animator.SetTrigger(ColonistAnimationString.EXIT_WORKING);
        }
        
    }

    public static class FurnitureTag
    {
        //tags
        public static readonly string BATTERY_FACTORY = "Battery Factory";
        public static readonly string CHICKEN_COOP = "Chicken Coop";
        public static readonly string COPPER_FURNACE = "Copper Furnace";
        public static readonly string COPPER_MINE = "Copper Mine";
        public static readonly string CORN_FIELD = "Corn Field";
        public static readonly string CRYO_PLANT  = "Cryo Plant";
        public static readonly string ELECTRIC_STOVE = "Electric Stove";
        public static readonly string ELECTRONICS_LAB =  "Electronics Lab";
        public static readonly string FISH_TANK = "Fish Tank";
        public static readonly string GAS_STOVE = "Gas Stove";
        public static readonly string HIGH_TEMP_FURNACE = "High Temp Furnace";
        public static readonly string IRON_FURNACE = "Iron Furnace";
        public static readonly string IRON_MINE = "Iron Mine";
        public static readonly string MED_X = "Med-X";
        public static readonly string NIOBIUM_MINE = "Niobium Mine"; 
        public static readonly string OIL_PUMP = "Oil Pump"; 
        public static readonly string POLYMER_PRESS = "Polymer Press"; 
        public static readonly string REFINERY = "Refinery"; 
        public static readonly string STONE_FARM = "Stone Farm"; 
        public static readonly string WOOD_FARM = "Wood Farm"; 
        public static readonly string WOOD_STOVE = "Wood Stove"; 
        public static readonly string RESEARCH_MACHINE = "Research Machine"; 
        public static readonly string SUPER_RESEARCH_MACHINE = "Super Research Machine"; 
        public static readonly string SUPER_DUPER_RESEARCH_MACHINE = "Super Duper Research Machine";
        public static readonly string PATIENT_CARE_CHAIR = "Patient Care Chair";
        public static readonly string POKER_TABLE = "Poker Table";
        public static readonly string SPEAKER = "Speaker";
        public static readonly string MED_BED = "Med Bed";
        public static readonly string BED = "Bed";
        public static readonly string DINING_TABLE = "Dining Table";
        public static readonly string SIT_TOILET = "Sit Toilet";
        public static readonly string SQUAT_TOILET = "Squat Toilet";
        public static readonly string WATER_TAP = "Water Tap";
        public static readonly string BATHTUB = "Bathtub";

        //animation accordinngly
        public static readonly Dictionary<string, string> ANIMATION_HASHMAP = new Dictionary<string, string>()
        {
            //typing anim
            {BATTERY_FACTORY, ColonistAnimationString.PRESSING_BUTTON},
            {CRYO_PLANT, ColonistAnimationString.PRESSING_BUTTON},
            {ELECTRONICS_LAB, ColonistAnimationString.PRESSING_BUTTON},
            {HIGH_TEMP_FURNACE, ColonistAnimationString.PRESSING_BUTTON},
            {MED_X, ColonistAnimationString.PRESSING_BUTTON},
            {POLYMER_PRESS, ColonistAnimationString.PRESSING_BUTTON},
            {RESEARCH_MACHINE, ColonistAnimationString.PRESSING_BUTTON},
            {SUPER_RESEARCH_MACHINE, ColonistAnimationString.PRESSING_BUTTON},
            {SUPER_DUPER_RESEARCH_MACHINE, ColonistAnimationString.PRESSING_BUTTON},
            {COPPER_FURNACE, ColonistAnimationString.PRESSING_BUTTON},
            {IRON_FURNACE, ColonistAnimationString.PRESSING_BUTTON},
            {OIL_PUMP, ColonistAnimationString.PRESSING_BUTTON},
            {REFINERY, ColonistAnimationString.PRESSING_BUTTON},
            
            //breaking anim
            {STONE_FARM, ColonistAnimationString.BREAKING_RESOURCE},
            {WOOD_FARM, ColonistAnimationString.BREAKING_RESOURCE},
            {COPPER_MINE, ColonistAnimationString.BREAKING_RESOURCE},
            {IRON_MINE, ColonistAnimationString.BREAKING_RESOURCE},
            
            //cooking anim
            {ELECTRIC_STOVE, ColonistAnimationString.COOKING},
            {GAS_STOVE, ColonistAnimationString.COOKING},
            {WOOD_STOVE, ColonistAnimationString.COOKING},
            
            //other seperate anim
            {CHICKEN_COOP, ColonistAnimationString.FEEDING_CHICKEN},
            {CORN_FIELD, ColonistAnimationString.PLANTING},
            {FISH_TANK, ColonistAnimationString.FISHING},
            {NIOBIUM_MINE, ColonistAnimationString.CARRYING_BIG_OBJECT},
            {PATIENT_CARE_CHAIR, ColonistAnimationString.SITTING_SICK},
            {MED_BED, ColonistAnimationString.LAYING_SICK},
            {BED, ColonistAnimationString.SLEEPING},
            {SPEAKER, ColonistAnimationString.DANCING},
            {POKER_TABLE, ColonistAnimationString.PLAYING_POKER},
            {DINING_TABLE, ColonistAnimationString.EATING},
            {SIT_TOILET, ColonistAnimationString.SIT_POOPING},
            {SQUAT_TOILET, ColonistAnimationString.SQUAT_POOPING},
            {WATER_TAP, ColonistAnimationString.WASHING_TAP},
            {BATHTUB, ColonistAnimationString.BATHING},
        };
        
        public static string GetAnimStringBaseOnFurniture(string tag)
        {
            foreach (var item in ANIMATION_HASHMAP)
            {
                if (item.Key == tag) return item.Value;
            }

            return null;
        }
    }
}