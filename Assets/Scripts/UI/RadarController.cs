using System;
using System.Collections.Generic;
using Generator;
using Player.State;
using Resources;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
    public class RadarController : MonoBehaviour
    {
        [Inject]
        ResourceBoxGenerator resourceBoxGenerator;

        [Inject]
        PlayerStateComponent playerStateComponent;

        [Inject]
        GameManager gameManager;

        [SerializeField]
        RectTransform radarRectTransform;

        [SerializeField]
        RectTransform sonarRectTransform;

        [SerializeField]
        GameObject dotPrefab;

        [SerializeField]
        Transform dotContainer;

        // TODO: Move to game settings
        const float RADAR_DISTANCE = 30.0f;
        const float TERTILE_RADAR_DISTANCE = RADAR_DISTANCE / 3;

        // TODO: Move to game settings
        [SerializeField]
        float rotationSpeed = 0.1f;

        Vector3 newRotation;

        Dictionary<ResourceBox, GameObject> radarDots = new Dictionary<ResourceBox, GameObject>();
        Dictionary<ResourceBox, RectTransform> radarDotsRectTransforms = new Dictionary<ResourceBox, RectTransform>();
        Dictionary<ResourceBox, Image> radarDotsImages = new Dictionary<ResourceBox, Image>();

        void Start()
        {
            gameManager.OnGameStart += Reinitialize;
            resourceBoxGenerator.OnBoxCollected += RemoveBoxDot;
        }

        void OnDestroy()
        {
            gameManager.OnGameStart -= Reinitialize;
            resourceBoxGenerator.OnBoxCollected -= RemoveBoxDot;
        }

        void OnEnable()
        {
            Reinitialize();
        }

        void Reinitialize()
        {
            ClearResourceBoxPools();
            foreach (var resourceBox in resourceBoxGenerator.ResourceBoxes) {
                AddBoxDot(resourceBox);
            }
        }

        void ClearResourceBoxPools()
        {
            foreach (var radarDotInstance in radarDots.Values) {
                Destroy(radarDotInstance);
            }
            radarDots.Clear();
            radarDotsRectTransforms.Clear();
            radarDotsImages.Clear();
        }

        void AddBoxDot(ResourceBox box)
        {
            if (!radarDots.ContainsKey(box)) {
                var dotInstance = Instantiate(dotPrefab, dotContainer.transform);
                var dotRectTransform = dotInstance.GetComponent<RectTransform>();
                var dotImage = dotInstance.GetComponent<Image>();
                radarDots.Add(box, dotInstance);
                radarDotsRectTransforms.Add(box, dotRectTransform);
                radarDotsImages.Add(box, dotImage);
            }
        }

        void RemoveBoxDot(ResourceBox box)
        {
            GameObject dotInstance;
            if (radarDots.TryGetValue(box, out dotInstance)) {
                Destroy(dotInstance);
                radarDots.Remove(box);
            }

            if (radarDotsImages.ContainsKey(box)) {
                radarDotsImages.Remove(box);
            }

            if (radarDotsRectTransforms.ContainsKey(box)) {
                radarDotsRectTransforms.Remove(box);
            }
        }

        void Update()
        {
            newRotation = sonarRectTransform.rotation.eulerAngles;
            newRotation.z += (rotationSpeed * Time.deltaTime);
            sonarRectTransform.rotation = Quaternion.Euler(newRotation);

            var resrouceBoxes = radarDots.Keys;
            var playerPosition = playerStateComponent.GetPlayerPosition();
            playerPosition.y = 0;
            foreach (var resourceBox in resrouceBoxes) {
                GameObject dotInstance;
                if (radarDots.TryGetValue(resourceBox, out dotInstance)) {
                    var normalizedBoxPosition = resourceBox.transform.position;
                    normalizedBoxPosition.y = 0;
                    var distance = Vector3.Distance(playerPosition, normalizedBoxPosition);

                    if (distance < RADAR_DISTANCE) {
                        dotInstance.SetActive(true);
                        var direction = normalizedBoxPosition - playerPosition;
                        DrawDotOnRadar(resourceBox, distance, new Vector3(direction.x, direction.z, 0));
                    } else {
                        dotInstance.SetActive(false);
                    }
                }
            }
        }

        void DrawDotOnRadar(ResourceBox resourceBox, float distance, Vector3 direction)
        {
            var percentageDistance = distance / RADAR_DISTANCE;
            var dotPosition = direction.normalized * ((radarRectTransform.rect.width / 2) * percentageDistance);
            RectTransform dotRectTransform;
            if (radarDotsRectTransforms.TryGetValue(resourceBox, out dotRectTransform)) {
                dotRectTransform.anchoredPosition = dotPosition;
            }

            Image dotImage;
            if (radarDotsImages.TryGetValue(resourceBox, out dotImage)) {
                if (distance <= 3 * TERTILE_RADAR_DISTANCE && distance > 2 * TERTILE_RADAR_DISTANCE) {
                    dotImage.CrossFadeColor(Color.red, 0.2f, true, true);
                }
                if (distance <= 2 * TERTILE_RADAR_DISTANCE && distance > TERTILE_RADAR_DISTANCE) {
                    dotImage.CrossFadeColor(Color.yellow, 0.2f, true, true);
                }
                if (distance <= TERTILE_RADAR_DISTANCE) {
                    dotImage.CrossFadeColor(Color.green, 0.2f, true, true);
                }
            }
        }

        void OnDrawGizmos()
        {
            if (playerStateComponent != null) {
                var playerPosition = playerStateComponent.GetPlayerPosition();
                Gizmos.color = Color.white;
                Gizmos.DrawWireSphere(playerPosition, RADAR_DISTANCE);
            }
        }
    }
}
