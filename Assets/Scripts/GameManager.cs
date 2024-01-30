using UnityEngine;
using Fabwelt.Common.Enums;
using Fabwelt.Managers.Scriptable;
using TMPro;

namespace Fabwelt.Common
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [SerializeField] TMP_Text versionText;

        public LevelCatalog LevelCatalog;
        public ColorScheme ColorScheme;

        public LevelDifficulty levelDifficulty;
        public static Level SelectedLevel { get; set; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                if (instance != this)
                {
                    Destroy(this.gameObject);
                    return;
                }
            }

            DontDestroyOnLoad(gameObject);

            versionText.text = Application.version;
        }
    }
}