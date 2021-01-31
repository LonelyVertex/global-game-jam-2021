using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] GameObject preGameUI;
        [SerializeField] GameObject gameOverUI;
        [SerializeField] GameObject gameCompleteUI;
        [SerializeField] GameObject creditsUI;
        [SerializeField] GameObject backgroundUI;
        [SerializeField] GameObject gameplayUI;
        [SerializeField] CoverController cover;
        [SerializeField] Text bestTimeText;
        [SerializeField] Text currentTimeText;
        [SerializeField] TextMeshProUGUI gameTime;

        [Inject] GameManager gameManager;

        void Start()
        {
            OpenMenu();
        }

        void Update()
        {
            gameTime.text = TimeToString(gameManager.CurrentTime);
        }

        public void StartGame()
        {
            Subscribe();
            gameManager.StartGame();
        }

        public void OpenMenu()
        {
            HideAll();
            backgroundUI.SetActive(true);
            preGameUI.SetActive(true);

            var bestTime = PlayerPrefs.GetFloat("bestTime", Mathf.Infinity);

            if (bestTime < Mathf.Infinity)
            {
                bestTimeText.text = TimeToString(bestTime);
            }
            else
            {
                bestTimeText.text = "--:--";
            }
        }

        public void OpenCredits()
        {
            HideAll();
            backgroundUI.SetActive(true);
            creditsUI.SetActive(true);
        }

        public void CloseCredits()
        {
            HideAll();
            backgroundUI.SetActive(true);
            preGameUI.SetActive(true);
        }

        public void QuitGame()
        {
            Debug.Log("Application Quit");
            Application.Quit();
        }

        void Subscribe()
        {
            gameManager.OnGameStart += OnGameStart;
            gameManager.OnLevelComplete += OnLevelComplete;
            gameManager.OnGameComplete += OnGameComplete;
            gameManager.OnGameOver += OnGameOver;
        }

        void Unsubscribe()
        {
            gameManager.OnGameStart -= OnGameStart;
            gameManager.OnLevelComplete -= OnLevelComplete;
            gameManager.OnGameComplete -= OnGameComplete;
            gameManager.OnGameOver -= OnGameOver;
        }

        void HideAll()
        {
            preGameUI.SetActive(false);
            gameOverUI.SetActive(false);
            gameCompleteUI.SetActive(false);
            creditsUI.SetActive(false);
            backgroundUI.SetActive(false);
            gameplayUI.SetActive(false);
        }


        void OnGameStart()
        {
            HideAll();
            gameplayUI.SetActive(true);
            cover.FadeOut();
        }

        void OnLevelComplete()
        {
            cover.FadeIn();
        }

        void OnGameOver()
        {
            cover.FadeIn();
            Unsubscribe();

            Invoke(nameof(ShowGameOverUI), CoverController.Duration);
        }

        void ShowGameOverUI()
        {
            HideAll();

            backgroundUI.SetActive(true);
            gameOverUI.SetActive(true);
        }

        void OnGameComplete()
        {
            Unsubscribe();
            HideAll();

            backgroundUI.SetActive(true);
            gameCompleteUI.SetActive(true);
            currentTimeText.text = TimeToString(gameManager.GameCompleteTime);
        }

        static string TimeToString(float time)
        {
            var mins = (int) time / 60;
            var secs = (int) time % 60;
            var extraZero = secs < 10 ? "0" : "";
            return $"{mins}:{extraZero}{secs}";
        }
    }
}
