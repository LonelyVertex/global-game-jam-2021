using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoverController : MonoBehaviour
{
    public const float Duration = 2;

    [SerializeField] Image image;

    Color targetColor;
    float currentLerp;

    public void FadeOut()
    {
        gameObject.SetActive(true);
        image.color = Color.black;
        targetColor = Color.clear;
        currentLerp = 0;
        Invoke(nameof(Disable), Duration);
    }

    public void FadeIn()
    {
        gameObject.SetActive(true);
        image.color = Color.clear;
        targetColor = Color.black;
        currentLerp = 0;
        Invoke(nameof(Disable), Duration);
    }

    void Update()
    {
        if (currentLerp <= 1)
        {
            image.color = Color.Lerp(image.color, targetColor, currentLerp);
            currentLerp += Time.deltaTime / Duration;
        }
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}