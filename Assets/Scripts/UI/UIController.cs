using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] GameObject preGameUI;
        [SerializeField] GameObject gameOverUI;
        [SerializeField] GameObject gameplayUI;
        [SerializeField] CoverController cover;

        [Inject] GameManager gameManager;
        
        public void StartGame()
        {
            gameManager.OnGameStart += OnGameStart;
            gameManager.OnGameOver += OnGameOver;
            gameManager.OnLevelComplete += OnLevelComplete;
            
            gameManager.StartGame();
        }

        void OnGameStart()
        {
            preGameUI.SetActive(false);
            gameOverUI.SetActive(false);
            gameplayUI.SetActive(true);
            cover.FadeOut();
        }

        void OnLevelComplete()
        {
            cover.FadeIn();
        }

        void OnGameOver()
        {
            gameManager.OnGameStart -= OnGameStart;
            gameManager.OnGameOver -= OnGameOver;
            gameOverUI.SetActive(true);
            gameplayUI.SetActive(false);
        }
    }
}
