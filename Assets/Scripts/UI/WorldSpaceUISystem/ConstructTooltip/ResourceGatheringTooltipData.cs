using BuildingSystem;
using DefaultNamespace.TaskSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.UI.WorldSpaceUISystem.ConstructTooltip
{
    public class ResourceGatheringTooltipData : FurnitureTooltipData
    {
        public TaskType TaskType;
        public List<ResourceWithAmount> Consumption;
        public List<ResourceWithAmount> OutputResource;
    }
}
