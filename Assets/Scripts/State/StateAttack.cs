using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D.UnitState
{
    public class StateAttack : StateWithCd
    {
        /// <summary>
        /// 下一state
        /// </summary>
        [Header("下一state")]
        public StateBase nextState;

        internal override void StateStart()
        {
            base.StateStart();
            _unitMovable._attacker.StartAttack(playerTarget.position - transform.position);
        }

        internal override void StateUpdate()
        {
            base.StateUpdate();

            EndAttack();
        }

        private void EndAttack()
        {
            if (!_unitMovable._animator.GetBool("IsAttacking"))
            {
                CloseCD(new StateBase[] { nextState });
            }
        }
    }
}

