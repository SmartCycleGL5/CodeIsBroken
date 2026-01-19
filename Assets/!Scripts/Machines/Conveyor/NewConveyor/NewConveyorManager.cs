using System;
using System.Collections.Generic;
using UnityEngine;


namespace CodeIsBroken.Machines
{
    public class NewConveyorManager : MonoBehaviour
    {
        public static NewConveyorManager instance;
        
        List<NewConveyor> conveyors = new List<NewConveyor>();

        private void Start()
        {
            instance = this;
            Tick.OnTick += UpdateTick;
        }

        public void AddConveyor(NewConveyor conveyor)
        {
            conveyors.Add(conveyor);
        }

        void ResetConveyors()
        {
            for(int i = 0; i < conveyors.Count; i ++)
            {
                conveyors[i].ResetState();
            }
        }

        void UpdateTick()
        { 
            ResetConveyors();  

            for(int i = 0; i < conveyors.Count; i ++)
            {
                conveyors[i].UpdateTick();
            }    
        }
    }
}
