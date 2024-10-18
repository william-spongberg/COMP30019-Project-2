using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SprintSlider : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private CanvasGroup canvasGroup;
    private Coroutine fadeCoroutine; // Store current fade coroutine

    [SerializeField]
    private float fadeDuration = 1.0f;

    public void SetStamina(float stamina)
    {
        slider.value = stamina;

        // Check if stamina is full, and if so, start fading out
        if (stamina >= slider.maxValue)
        {
            FadeOut();
        }
    }

    public void SetMaxStamina(float stamina)
    {
        slider.maxValue = stamina;
        slider.value = stamina;

    }

     // Call this to start fading out
    public void FadeOut()
    {
        // Stop any ongoing fade-in coroutine to prevent overlap
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0;

         // Reset the coroutine reference
        fadeCoroutine = null;
    }

    // Call this to start fading in
    public void FadeIn()
    {
         // Stop any ongoing fade-out coroutine to prevent overlap
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        fadeCoroutine = StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        float startAlpha = canvasGroup.alpha;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1;

        // Reset the coroutine reference
        fadeCoroutine = null;
    }
    
}
