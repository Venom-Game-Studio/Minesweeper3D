using TMPro;
using UnityEngine;
using Fabwelt.Common;
using System.Collections;
using Fabwelt.Common.Enums;
using Fabwelt.Managers.Scriptable;

namespace Fabwelt.UI
{
    public class GameUiManager : MonoBehaviour
    {
        public static GameUiManager Instance;

        [SerializeField] TMP_Text timerText;
        [SerializeField] TMP_Text flagsText;

        int timeTaken = 0;
        [SerializeField] int wrongFlagPlacedCount = 0;

        Coroutine timerCoroutine = null;

        private void Awake()
        {
            Instance = this;
        }
        private void OnEnable()
        {
            GameManager.GameStateChanged += GameStateChanged;
            TilePrefabData.WrongFlagPlaced += () => { wrongFlagPlacedCount++; };
        }
        private void OnDisable()
        {
            GameManager.GameStateChanged -= GameStateChanged;
        }

        private void Start()
        {
            wrongFlagPlacedCount = 0;
            timerText.text = 0.ToString("D4");
            UpdateFlageCount(GameManager.SelectedLevel.mineCount);
        }

        private void GameStateChanged(GameState _state)
        {
            switch (_state)
            {
                case GameState.Start:
                    if (timerCoroutine != null)
                    {
                        StopCoroutine(timerCoroutine);
                        timerCoroutine = null;
                    }
                    else
                        timerCoroutine = StartCoroutine(TimerCoroutine(GameManager.SelectedLevel.score.MaxTime));
                    break;

                case GameState.End:

                    if (timerCoroutine != null)
                    {
                        StopCoroutine(timerCoroutine);
                        timerCoroutine = null;
                    }

                    int score = CalculateFinalScore(GameManager.SelectedLevel);
                    Debug.Log(score);
                    break;

                case GameState.none:
                    break;
            }
        }

        int CalculateFinalScore(Level _level)
        {
            int timeFactor = _level.score.MaxTime - timeTaken;
            int finalScore = _level.score.baseScore + timeFactor - wrongFlagPlacedCount * GameManager.instance.LevelCatalog.minePanaltyPoint;

            return finalScore;
        }

        IEnumerator TimerCoroutine(int _maxTime)
        {
            timeTaken = 0;

            while (timeTaken < _maxTime)
            {
                timeTaken++;
                timerText.text = timeTaken.ToString("D4");

                yield return new WaitForSeconds(1f);
            }

            timerCoroutine = null;
        }

        public void UpdateFlageCount(int _count)
        {
            flagsText.text = _count.ToString("D3");
        }
    }
}