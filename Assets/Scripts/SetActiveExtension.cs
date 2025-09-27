using System.Collections;
using UnityEngine;

public static class SetActiveExtension
{
    const float changeSecondsTemplate = 0.5f;
    public static IEnumerator Zoom(GameObject gameObject, bool toActive, float changeSeconds = changeSecondsTemplate)
    {
        RectTransform rt = gameObject.GetComponent<RectTransform>();

        Vector3 originalScale = rt.localScale;
        float elapsedTime = 0f;

        if (toActive)
        {
            rt.localScale = Vector3.zero;
            gameObject.SetActive(true);

            while (elapsedTime < changeSeconds)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / changeSeconds);

                // float easedT = Mathf.SmoothStep(0f, 1f, t);
                float easedT = 1 - (1 - t) * (1 - t);

                rt.localScale = Vector3.Lerp(Vector3.zero, originalScale, easedT);
                yield return null;
            }

            rt.localScale = originalScale;
        }
        else
        {
            Vector3 startScale = originalScale;

            while (elapsedTime < changeSeconds)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / changeSeconds);

                // float easedT = Mathf.SmoothStep(0f, 1f, t);
                // float easedT = 1 - (1 - t) * (1 - t);
                float easedT = t * t;


                rt.localScale = Vector3.Lerp(startScale, Vector3.zero, easedT);
                yield return null;
            }

            rt.localScale = Vector3.zero;
            gameObject.SetActive(false);

            rt.localScale = originalScale;
        }
    }
    public static IEnumerator Fade(GameObject gameObject, bool toActive, float changeSeconds = changeSecondsTemplate)
    {
        yield return null;
    }
    public static IEnumerator Slide(GameObject gameObject, bool toActive, float changeSeconds = changeSecondsTemplate)
    {
        yield return null;
    }
}