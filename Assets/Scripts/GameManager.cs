using TMPro;
using System;
using UnityEngine;
using Fabwelt.Common.Enums;
using Fabwelt.Managers.Scriptable;

namespace Fabwelt.Common
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        [SerializeField] TMP_Text versionText;

        public LevelCatalog LevelCatalog;
        public ColorScheme ColorScheme;

        internal LevelDifficulty levelDifficulty;
        internal static Level SelectedLevel { get; set; }

        internal static bool isGameOver = false;
        static GameState gameState = GameState.none;
        internal static GameState GameState
        {
            get { return gameState; }
            set
            {
                gameState = value;
                if (gameState == GameState.End)
                    isGameOver = true;

                GameStateChanged(gameState);
            }
        }
        public static event Action<GameState> GameStateChanged = delegate { };

        public static void UpdateTileFlag(TilePrefabData data) => TileFlagUpdate(data);
        public static event Action<TilePrefabData> TileFlagUpdate = delegate { };

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