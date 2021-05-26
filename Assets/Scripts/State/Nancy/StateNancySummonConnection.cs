using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D.UnitState
{
    public class StateNancySummonConnection : StateBase
    {
        public GameObject link;

        public float linkWidth;

        public float healPerS;

        public Vector3 sourceOffset;

        private Unit nancy;

        private float healTick = 0;

        internal override void StateStart()
        {
            base.StateStart();

            nancy = Unit.FindUnitWithName("女巫");

            link.SetActive(true);
        }

        internal override void StateUpdate()
        {
            base.StateUpdate();

            ConnectNHeal();
        }

        internal override void StateEnd()
        {
            base.StateEnd();

            link.SetActive(false);
        }

        private void ConnectNHeal()
        {
            if (!nancy)
            {
                Close(null);
                _unitMovable.Die();
                return;
            }

            Vector3 targetPos = nancy.transform.position;
            Vector3 sourcePos = transform.position + sourceOffset;
            link.transform.position = (targetPos + sourcePos) / 2;
            link.transform.rotation = Quaternion.LookRotation(Vector3.forward, targetPos - sourcePos);
            link.transform.localScale = new Vector3(linkWidth * Random.Range(0.8f, 1.2f), Vector3.Distance(targetPos, sourcePos), 1);

            healTick += Time.deltaTime;
            if (healTick >= 0.5f)
            {
                healTick = 0;
                nancy._unitStatsManager.Heal(healPerS / 2);
            }
        }
    }
}

