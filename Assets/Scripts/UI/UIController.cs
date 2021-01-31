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

        [Inject] GameManager gameManager;
        
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
            Unsubscribe();
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
        }
    }
}
