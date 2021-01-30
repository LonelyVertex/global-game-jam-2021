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
            gameManager.StartGame();
            preGameUI.SetActive(false);
            gameOverUI.SetActive(false);

            gameManager.OnGameOver += OnGameOver;
        }

        void OnGameOver()
        {
            gameManager.OnGameOver -= OnGameOver;
            gameOverUI.SetActive(true);
        }
    }
}
