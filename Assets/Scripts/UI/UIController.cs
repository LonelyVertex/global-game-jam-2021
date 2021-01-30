using UnityEngine;
using Zenject;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] GameObject preGameUI;
        [SerializeField] GameObject gameOverUI;
        
        [Inject] GameManager gameManager;
        
        public void StartGame()
        {
            gameManager.OnGameStart += OnGameStart;
            gameManager.OnGameOver += OnGameOver;
            
            gameManager.StartGame();
        }

        void OnGameStart()
        {
            preGameUI.SetActive(false);
            gameOverUI.SetActive(false);
        }

        void OnGameOver()
        {
            gameManager.OnGameStart -= OnGameStart;
            gameManager.OnGameOver -= OnGameOver;
            gameOverUI.SetActive(true);
        }
    }
}
