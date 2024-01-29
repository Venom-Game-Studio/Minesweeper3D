using Fabwelt.Common.Enums;
using UnityEngine;

namespace Fabwelt.Common
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;

        public LevelDifficulty levelDifficulty;

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
        }
    }
}