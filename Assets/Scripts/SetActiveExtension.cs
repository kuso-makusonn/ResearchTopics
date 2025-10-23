using System.Collections;
using UnityEngine;

public static class SetActiveExtension
{
    const float changeSecondsTemplate = 0.3f;
    public static IEnumerator Zoom(GameObject gameObject, bool toActive, float changeSeconds = changeSecondsTemplate)
    {
        RectTransform rt = gameObject.GetComponent<RectTransform>();

        Vector3 originalScale = rt.localScale;
        float elapsedTime = 0f;

        if (toActive)
        {
            rt.localScale = Vector3.zero;
            gameObject.SetActive(true);

            while (elapsedTime < (changeSeconds * 0.9f))
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / (changeSeconds * 0.9f));

                // float easedT = Mathf.SmoothStep(0f, 1f, t);
                float easedT = 1 - (1 - t) * (1 - t);

                rt.localScale = Vector3.Lerp(Vector3.zero, originalScale * 1.1f, easedT);
                yield return null;
            }

            while (elapsedTime < changeSeconds)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / changeSeconds);

                // float easedT = Mathf.SmoothStep(0f, 1f, t);
                float easedT = t * t;

                rt.localScale = Vector3.Lerp(originalScale * 1.1f, originalScale, easedT);
                yield return null;
            }

            rt.localScale = originalScale;
        }
        else
        {
            while (elapsedTime < (changeSeconds * 0.1))
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / (changeSeconds * 0.1f));

                // float easedT = Mathf.SmoothStep(0f, 1f, t);
                float easedT = 1 - (1 - t) * (1 - t);

                rt.localScale = Vector3.Lerp(originalScale, originalScale * 1.1f, easedT);
                yield return null;
            }

            while (elapsedTime < changeSeconds)
            {
                elapsedTime += Time.deltaTime;
                float t = Mathf.Clamp01(elapsedTime / changeSeconds);

                // float easedT = Mathf.SmoothStep(0f, 1f, t);
                // float easedT = 1 - (1 - t) * (1 - t);
                float easedT = t * t;


                rt.localScale = Vector3.Lerp(originalScale * 1.1f, Vector3.zero, easedT);
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