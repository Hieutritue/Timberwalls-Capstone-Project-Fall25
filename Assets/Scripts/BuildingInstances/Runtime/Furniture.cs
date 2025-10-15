using System.Collections.Generic;
using _Scripts.StateMachine;
using BuildingSystem.States;
using UnityEngine;
using DefaultNamespace.ColonistSystem;
using UnityEngine.Serialization;

namespace BuildingSystem
{
    public class Furniture : MonoBehaviour
    {
        [Header("Data Reference")]
        public FurnitureSO FurnitureSo;
        
        private StateMachine _stateMachine = new StateMachine();
        private ConstructingFurnitureState _constructingFurnitureState;
        private IdleFurnitureState _idleFurnitureState;
        private WorkingFurnitureState _workingFurnitureState;

        public virtual void Initialize(FurnitureSO furnitureSO)
        {
            FurnitureSo = furnitureSO;
            
            InitStateMachine();
        }

        private void InitStateMachine()
        {
            _constructingFurnitureState = new ConstructingFurnitureState(this);
            _idleFurnitureState = new IdleFurnitureState(this);
            _workingFurnitureState = new WorkingFurnitureState(this);
            _stateMachine.Initialize(_constructingFurnitureState);
        }

        public float GetActualBuildTime(float engineeringLevel)
        {
            return FurnitureSo.BaseBuildTime * (1 - engineeringLevel * 0.05f);
        }
    }
}