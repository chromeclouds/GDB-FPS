using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class popUpFlashEffect : MonoBehaviour
{
    [SerializeField] Image border;
    [SerializeField] float flashRate;
    void Start()
    {
    }

    private void OnEnable()
    {
        startFlash();
    }
    // Update is called once per frame
    void Update()
    {

    }

    void startFlash()
    {
        Color start = border.color;
        start.a = 0;
        Color end = border.color;
        end.a = 1;
        StartCoroutine(flash(start, end));
    }

    IEnumerator flash(Color start, Color end)
    {
        while(true)
        {
            // Found some code here while researching how to do this. https://discussions.unity.com/t/how-do-i-fade-a-object-in-out-over-time/601728 User StarManta
            // Adjusted to go from 0 to 1 transparence and back over and over to simulate flashing
            for (float t = 0f; t < flashRate; t += Time.deltaTime)
            {
                float normalizedTime = t / flashRate;

                border.color = Color.Lerp(start, end, normalizedTime);

                yield return null;
            }
            border.color = end;

            for (float t = 0f; t < flashRate; t += Time.deltaTime)
            {
                float normalizedTime = t / flashRate;

                border.color = Color.Lerp(end, start, normalizedTime);

                yield return null;
            }
            border.color = start;
        }
    }
}
