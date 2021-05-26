using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D.UnitState
{
    [RequireComponent(typeof(StateStay))]
    public class StateWithCd : StateBase
    {
        /// <summary>
        /// 对应静止state
        /// </summary>
        [HideInInspector]
        public StateStay _stateStay;

        /// <summary>
        /// 冷却
        /// </summary>
        public float cd;

        internal override void Awake()
        {
            base.Awake();
            _stateStay = GetComponent<StateStay>();
        }

        /// <summary>
        /// 有冷却的切换state
        /// </summary>
        public void CloseCD(IEnumerable<StateBase> statesAfterCd)
        {
            if (cd <= 0)
            {
                Close(statesAfterCd);
            }
            else
            {
                _stateStay.Initialize(cd, statesAfterCd);
                Close(new StateBase[] { _stateStay });
            }
        }
    }
}

