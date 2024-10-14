using System.Collections;
using TMPro;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{

    private float countdownTime = 600f;

    [SerializeField]
    private TextMeshProUGUI timerText; 

    [SerializeField]
    private CanvasGroup canvasGroup; 

    // Fade settings
    [SerializeField]
    private float fadeDuration = 5.0f; 

    [SerializeField]
    private float initialDelay = 5f; 

    [SerializeField]
    private float lingerDuration = 5f;


    private void Start()
    {
       // Start with an empty text and hidden timer
        timerText.text = "";
        canvasGroup.alpha = 0;
        
        // Wait for the initial delay before starting the timer
        StartCoroutine(InitialDelayBeforeStart());
    }

    private IEnumerator InitialDelayBeforeStart()
    {
        // Wait for the initial delay before showing the 10:00 mark
        yield return new WaitForSeconds(initialDelay);

        // Set the timer to 10:00 and fade it in
        timerText.text = "Total Memory Loss in: 10:00";
        yield return StartCoroutine(FadeInTimer());

        // Linger on the 10-minute mark for a few seconds
        yield return new WaitForSeconds(lingerDuration);

        // Start the countdown
        StartCoroutine(UpdateTimer());

        // Linger on the 10-minute mark for a few seconds
        yield return new WaitForSeconds(lingerDuration);

        // Fade out
        yield return StartCoroutine(FadeOutTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(countdownTime / 60);
            int seconds = Mathf.FloorToInt(countdownTime % 60);

            // Update the timer text
            timerText.text = $"Total Memory Loss in: {minutes:00}:{seconds:00}";

            // Trigger fading at key moments
            //if (countdownTime <= timef && !hasFadedInForMidway)
            //{
                //StartCoroutine(FadeInTimer());
                //hasFadedInForMidway = true;
            //}


            yield return null;
        }

        // Timer has ended, you can trigger game over or other events here
    }

    private IEnumerator FadeInTimer()
    {
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0, 1, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 1;

    }

    private IEnumerator FadeOutTimer()
    {
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0;
    }
}
