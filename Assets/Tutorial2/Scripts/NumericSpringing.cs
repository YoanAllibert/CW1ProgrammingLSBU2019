using System.Collections;
using UnityEngine;

public class NumericSpringing : MonoBehaviour
{
    [SerializeField] private Vector3 modifScale;
    [SerializeField] private float duration;

    private Vector3 originalScale;
    private static float twoPI = 2f * Mathf.PI;

    private void Start()
    {
        originalScale = transform.localScale;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            transform.localScale = Vector3.Scale(transform.localScale, modifScale);
            StartCoroutine(EaseScaleVector(transform, transform.localScale, originalScale, duration));
        }
    }

    IEnumerator EaseScaleVector(Transform trans, Vector3 startScale, Vector3 endScale, float duration)
    {
        float t = 0f;
        while (t < duration)
        {
            Vector3 newOne = new Vector3(
                ElasticEase(t, startScale.x, endScale.x - startScale.x, duration),
                ElasticEase(t, startScale.y, endScale.y - startScale.y, duration),
                ElasticEase(t, startScale.z, endScale.z - startScale.z, duration)
                );

            trans.localScale = newOne;
            yield return null;
            t += Time.deltaTime;
        }

        trans.localScale = endScale;
    }

    /*  t - the current time position(a value from 0 -> d)
        b - the initial value
        c - the amount of change(end - b)
        d - the total amount of time the ease should take 
    */

    public static float ElasticEase(float t, float b, float c, float d)
    {
        float s;
        if (t == 0f) return b;
        if ((t /= d) == 1) return b + c;

        s = (d * 0.3f) / 4;
        return (c * (float)Mathf.Pow(2, -10 * t) * (float)Mathf.Sin((t * d - s) * twoPI / (d * 0.3f)) + c + b);
    }
}
