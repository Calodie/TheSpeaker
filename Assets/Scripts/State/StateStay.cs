using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D.UnitState
{
    public class StateStay : StateBase
    {
        /// <summary>
        /// 后继state
        /// </summary>
        private IEnumerable<StateBase> nextStates;

        /// <summary>
        /// 静止时长
        /// </summary>
        private float stayTick = 0;

        /// <summary>
        /// 初始化静止state
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="nextState"></param>
        public void Initialize(float duration, IEnumerable<StateBase> nextStates)
        {
            stayTick = duration;
            this.nextStates = nextStates;
        }

        internal override void StateUpdate()
        {
            base.StateUpdate();

            Tick();
        }

        private void Tick()
        {
            stayTick -= Time.deltaTime;
            if (stayTick <= 0)
            {
                Close(nextStates);
            }
        }
    }
}

