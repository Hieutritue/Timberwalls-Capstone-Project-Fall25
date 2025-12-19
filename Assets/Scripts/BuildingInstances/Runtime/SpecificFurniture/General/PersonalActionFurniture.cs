using System;
using System.Collections.Generic;
using DefaultNamespace.TaskSystem;
using TaskSystem.Tasks.SpecificTask.PersonalTask;
using UnityEngine;

namespace BuildingSystem
{
    public class PersonalActionFurniture : Furniture, ITaskCreator
    {
        public TaskType TaskType;
        public List<Transform> ActionPoints;
        public PersonalActionFurnitureSo PersonalActionFurnitureSo => (PersonalActionFurnitureSo)PlaceableSo;
        [SerializeField] private SoundSource vfx_source;
        [SerializeField] private string loopSound;
        private bool is_loop = false;

        public override void Start()
        {
            base.Start();
            Animator = GetComponent<Animator>();
        }

        public override void TransitionToIdle()
        {
            if (vfx_source != null && is_loop)
            {
                is_loop = false;
                vfx_source.StopImmediate();
            }
            base.TransitionToIdle();
            if (Animator)
                Animator.SetBool(BuildingAnimationString.IS_ACTIVE, false);
            // else
            //     Debug.LogWarning("No animator found for" + this.name);

        }

        public override void TransitionToWorking()
        {
            _stateMachine.TransitionTo(_workingBuildingState);
            if (vfx_source != null && !is_loop)
            {
                is_loop = true;
                vfx_source.Play(loopSound, fadeIn: false, fadeOut: false, crossfade: true);
            }
            if (Animator)
                Animator.SetBool(BuildingAnimationString.IS_ACTIVE, true);
        }

        public void CreateTask()
        {
            ActionPoints.ForEach(ap =>
            {
                switch (TaskType)
                {
                    case TaskType.Sleeping:
                        AddTask(new SleepingTask(this, ap, TaskType));
                        break;
                    case TaskType.Eating:
                        AddTask(new EatingTask(this, ap, TaskType));
                        break;
                    case TaskType.Pooping:
                        AddTask(new PooTask(this, ap, TaskType));
                        break;
                    case TaskType.Playing:
                        AddTask(new PlayingTask(this, ap, TaskType));
                        break;
                    case TaskType.Washing:
                        AddTask(new WashingTask(this, ap, TaskType));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            });
        }
    }
}