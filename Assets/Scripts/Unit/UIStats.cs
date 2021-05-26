using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KeyboardMan2D
{
    public class UIStats : MonoBehaviour
    {
        /// <summary>
        /// 对应单位
        /// </summary>
        [HideInInspector]
        public Unit _unit;

        /// <summary>
        /// 是否为玩家的状态UI
        /// </summary>
        public bool isPlayer;

        /// <summary>
        /// 是否为boss血条UI
        /// </summary>
        public bool isBoss;

        /// <summary>
        /// 该unit的大小
        /// </summary>
        public Vector2 unitSize;

        /// <summary>
        /// 受伤显示时长
        /// </summary>
        public float showDuration = 5;

        private UIBar barHp;
        /// <summary>
        /// 血条
        /// </summary>
        public UIBar BarHp
        {
            get
            {
                if (barHp)
                {
                    return barHp;
                }

                if (isPlayer)
                {
                    barHp = UIIngame.instance.playerBarHp;
                }
                else
                {
                    GameObject uIBarPrefab = ResourceLoader.Load("Prefabs/UI/UIBarPrefab") as GameObject;
                    barHp = Instantiate(uIBarPrefab, UIIngame.instance.instantTrans, false).GetComponent<UIBar>();
                }

                return barHp;
            }

            set => barHp = value;
        }

        private UIBar barMp;
        /// <summary>
        /// 能量条
        /// </summary>
        public UIBar BarMp
        {
            get
            {
                if (barMp)
                {
                    return barMp;
                }

                if (isPlayer)
                {
                    barMp = UIIngame.instance.playerBarMp;
                }

                return barMp;
            }

            set => barMp = value;
        }

        private UIBossHp bossHp;
        /// <summary>
        /// Boss血条
        /// </summary>
        public UIBossHp BossHp
        {
            get
            {
                if (bossHp)
                {
                    return bossHp;
                }

                bossHp = UIIngame.instance.bossHpContainer.CreateBossHp(_unit.unitName);
                return bossHp;
            }

            set => bossHp = value;
        }

        private float tickBarHp = 0;

        private bool destroyed = false;

        private void Awake()
        {
            _unit = GetComponent<Unit>();
        }

        private void Update()
        {
            FollowUI();
            LiteUI();
        }

        private void FollowUI()
        {
            if (isPlayer || destroyed || _unit._unitStatsManager.immortal)
            {
                return;
            }

            Vector3 rePoint1 = transform.position + Vector3.left * unitSize.x / 2;
            Vector3 rePoint2 = transform.position + Vector3.right * unitSize.x / 2;
            Vector3 cenPoint = transform.position + Vector3.up * unitSize.y / 2;

            Camera camera = CameraController.instance._camera;

            float width = Vector2.Distance(camera.WorldToScreenPoint(rePoint1), camera.WorldToScreenPoint(rePoint2));

            BarHp._rectTransform.position = camera.WorldToScreenPoint(cenPoint);
            BarHp._rectTransform.sizeDelta = new Vector2(width, BarHp._rectTransform.sizeDelta.y);
        }

        private void LiteUI()
        {
            if (isPlayer || destroyed || _unit._unitStatsManager.immortal)
            {
                return;
            }

            tickBarHp = Mathf.Max(0, tickBarHp - Time.deltaTime);
            BarHp.gameObject.SetActive(tickBarHp > 0);
        }

        public void SetHp(float hp, float maxHp, bool hurt)
        {
            if(destroyed)
            {
                return;
            }

            BarHp.SetValue(hp, maxHp);

            if (isBoss)
            {
                BossHp.SetHp(hp, maxHp);
            }

            if (hurt)
            {
                tickBarHp = showDuration;
            }
        }

        public void SetMp(float mp, float maxMp)
        {
            if (destroyed)
            {
                return;
            }

            if (maxMp <= 0)
            {
                return;
            }

            BarMp.SetValue(mp, maxMp);
        }

        public void Remove()
        {
            if (isPlayer || destroyed)
            {
                return;
            }

            destroyed = true;
            if(barHp)
            {
                Destroy(barHp.gameObject);
            }

            if (isBoss && bossHp)
            {
                Destroy(bossHp.gameObject);
            }
        }

        private void OnDestroy()
        {
            Remove();
        }
    }
}
