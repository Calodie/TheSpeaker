using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace KeyboardMan2D
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public bool Paused = false;

        public PlayerController PlayerController { get; private set; }

        /// <summary>
        /// 当前控制玩家
        /// </summary>
        [Header("当前控制玩家")]
        public UnitMovable player;

        public UnityEvent startEvent;

        public void Pause()
        {
            Paused = true;
        }

        public void Resume()
        {
            Paused = false;
        }

        public void LoadScene(string sceneName)
        {
            StartCoroutine(LoadSceneCoro(sceneName));
        }

        private IEnumerator LoadSceneCoro(string sceneName)
        {
            // 返回主菜单则清空任务列表
            if (sceneName == "MainMenu")
            {
                QuestManager.instance.Initialize();
            }

            UIDialog.instance.Initialize();

            AudioManager.instance.SetBGM(AudioDictionary.instance.FindAudioClipWithName("Normal"));

            SceneManager.LoadScene(sceneName);
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            if (player)
            {
                player._unitStatsManager.Initialize();
                SceneInitializer sceneInitializer = FindObjectOfType<SceneInitializer>();
                if (sceneInitializer)
                {
                    player.transform.position = sceneInitializer.transform.position;
                }
            }
        }

        private void Awake()
        {
            if (instance)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void Start()
        {
            PlayerController = new PlayerController();
            startEvent.Invoke();
        }

        private void Update()
        {
            FindPlayer();
            UpdatePlayer();
        }

        private void FindPlayer()
        {
            if (!player || player.Killed)
            {
                List<Unit> units = Unit.FindUnitsWithTag("Player");
                if (units.Count > 0 && units[0] != null && !units[0].Killed)
                {
                    player = units[0] as UnitMovable;
                    DontDestroyOnLoad(player.gameObject);
                }
            }
        }

        private void UpdatePlayer()
        {
            PlayerController.Update(player);

            /*
            if (player)
            {
                player._unitStatsManager._uIStats.BarHp = UIIngame.instance.playerBarHp;
            }*/
        }
    }
}
