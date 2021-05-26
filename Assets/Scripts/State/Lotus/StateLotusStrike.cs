using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D.UnitState
{
    public class StateLotusStrike : StateWithCd
    {
        /// <summary>
        /// 下一state
        /// </summary>
        [Header("下一state")]
        public StateBase nextState;

        /// <summary>
        /// 突袭时长
        /// </summary>
        [Header("突袭时长")]
        public float strikeDuration;

        /// <summary>
        /// 隐形过程时长
        /// </summary>
        [Header("隐形过程时长")]
        public float fadeDuration;

        /// <summary>
        /// 隐形可见度
        /// </summary>
        [Header("隐形可见度")]
        public float fadeAlpha;

        private SpriteRenderer[] spriteRenderers;

        private float stikeTick;

        private float fadeTick;

        internal override void Awake()
        {
            base.Awake();

            spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        }

        internal override void StateStart()
        {
            base.StateStart();
            stikeTick = 0;
            fadeTick = 0;
        }

        internal override void StateUpdate()
        {
            base.StateUpdate();
            Tick();
            Strike();
        }

        internal override void StateEnd()
        {
            base.StateEnd();

            foreach (SpriteRenderer spriteRenderer in spriteRenderers)
            {
                Color color = spriteRenderer.color;
                color.a = 1;
                spriteRenderer.color = color;
            }

            _unitMovable._unitStatsManager.moveSpeed = _unitMovable._unitStatsManager._unitInitStats.moveSpeed;
        }

        private void Tick()
        {
            stikeTick += Time.deltaTime;
            fadeTick += Time.deltaTime;
        }

        private void Strike()
        {
            if (fadeTick <= fadeDuration)
            {
                float alpha = 1 - (1 - fadeAlpha) * (fadeTick / fadeDuration);
                foreach (SpriteRenderer spriteRenderer in spriteRenderers)
                {
                    Color color = spriteRenderer.color;
                    color.a = alpha;
                    spriteRenderer.color = color;
                }
            }

            _unitMovable._unitStatsManager.moveSpeed = _unitMovable._unitStatsManager._unitInitStats.moveSpeed * 5;

            if (stikeTick > strikeDuration)
            {
                CloseCD(new StateBase[] { nextState });
            }
        }
    }
}

