using UnityEngine;
using System.Collections;

public class Element : MonoBehaviour
{
    public float easingCoeff = -8.0f;
    public float easingCoeffShrink = -8.0f;

    public float noiseSpeed = 1.0f;
    public float noiseAmount = 0.1f;

    float targetScale;
    float currentScale;
    float noiseBase;

    void Awake ()
    {
        currentScale = 0.1f;
        transform.localScale = Vector3.one * currentScale;
        easingCoeff *= Random.Range (0.95f, 1.0f);
    }

    void Update ()
    {
        currentScale = targetScale - (targetScale - currentScale) * Mathf.Exp (easingCoeff * Time.deltaTime); 
        transform.localScale = Vector3.one * (currentScale * (1.0f + noiseAmount * Mathf.PerlinNoise (Time.time * noiseSpeed, noiseBase)));

        if (currentScale < 0.01f)
            Destroy (gameObject);
    }

    public void ApplyMidiMessage (MidiMessage message)
    {
        targetScale = message.data2 * 1.5f / 128;
        noiseBase = message.data1;
    }

    public void StartShrink ()
    {
        targetScale = 0.0f;
        easingCoeff = easingCoeffShrink;
    }
}