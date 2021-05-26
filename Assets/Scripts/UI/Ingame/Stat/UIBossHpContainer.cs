using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KeyboardMan2D
{
    public class UIBossHpContainer : MonoBehaviour
    {
        private List<UIBossHp> bossHps = new List<UIBossHp>();

        /// <summary>
        /// 新增一个boss血条
        /// </summary>
        /// <param name="bossName"></param>
        /// <returns></returns>
        public UIBossHp CreateBossHp(string bossName)
        {
            UIBossHp bossHp =
                Instantiate(ResourceLoader.Load("Prefabs/UI/UIBossHpPrefab") as GameObject, transform).GetComponent<UIBossHp>();
            bossHp.Initialize(bossName);
            bossHps.Add(bossHp);
            return bossHp;
            // bossHp._rectTransform.anchoredPosition = Vector2.down * 100 * (questButtons.Count - 1);
        }

        private void Update()
        {
            Locate();
        }

        private void Locate()
        {
            bossHps.RemoveAll(BossHpDestroyed);

            for (int i = 0; i < bossHps.Count; i++)
            {
                bossHps[i]._rectTransform.anchoredPosition = Vector2.up * 100 * i;
            }
        }

        private bool BossHpDestroyed(UIBossHp bossHp)
        {
            return !bossHp;
        }
    }
}
