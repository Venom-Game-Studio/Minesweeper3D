using TMPro;
using System;
using UnityEngine;
using Fabwelt.Common;
using UnityEngine.UI;
using Fabwelt.Common.Enums;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace Fabwelt.UI
{
    public class MainMenu : MonoBehaviour
    {
        public TMP_Dropdown difficultyDropdown;
        public Button playBtn, exitBtn;

        private void Start()
        {
            List<string> list = new List<string>(Enum.GetNames(typeof(LevelDifficulty)));

            difficultyDropdown.ClearOptions();
            difficultyDropdown.AddOptions(list);

            playBtn.onClick.RemoveAllListeners();
            exitBtn.onClick.RemoveAllListeners();

            playBtn.onClick.AddListener(OnClickPlay);
            exitBtn.onClick.AddListener(OnClickExit);
        }

        void OnClickPlay()
        {
            GameManager.instance.levelDifficulty = (LevelDifficulty)Enum.Parse(typeof(LevelDifficulty), difficultyDropdown.value.ToString());
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        void OnClickExit()
        {
            Application.Quit();
        }
    }
}