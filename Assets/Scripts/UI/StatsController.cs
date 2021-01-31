using Player.Energy;
using UnityEngine;
using Zenject;

namespace UI
{
    public class StatsController : MonoBehaviour
    {

        [Inject]
        EnergyHandler energyHandler;

        [Inject]
        GameManager gameManager;

        [SerializeField]
        RectTransform energyFillRectTransform;

        [SerializeField]
        RectTransform resourcesFillRectTransform;

        float energyPercentage = 100.0f;
        float resourcePercentage = 100.0f;

        void Start()
        {
            Reinitialize();
        }

        void Reinitialize()
        {
            energyPercentage = 0;
            resourcePercentage = 0;
        }

        void SetRectSize(RectTransform rectTransform, float percentage)
        {
            var anchoredPosition = rectTransform.anchoredPosition;
            var x = rectTransform.rect.width - (percentage * rectTransform.rect.width);

            anchoredPosition.x = x;
            rectTransform.anchoredPosition = anchoredPosition;
        }

        void Update()
        {
            energyPercentage = energyHandler.GetEnergyPercentage();
            resourcePercentage = gameManager.GetBoxPercentage();

            SetRectSize(energyFillRectTransform, energyPercentage);
            SetRectSize(resourcesFillRectTransform, resourcePercentage);
        }
    }
}
