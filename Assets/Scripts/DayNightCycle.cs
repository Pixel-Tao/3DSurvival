using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DayNightCycle : MonoBehaviour
{
    [Range(0f, 1f)] public float time;
    public float fullDayLength;
    public float startTime = 0.4f;

    private float timeRate;
    public Vector3 noon; // Vector 90, 0, 0

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingItensityMultiplier;
    public AnimationCurve reflectionItensityMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        timeRate = 1f / fullDayLength;
        time = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1f;
        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightingItensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionItensityMultiplier.Evaluate(time);
    }

    private void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);
        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f)) * noon * 4f;
        lightSource.color = gradient.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if (lightSource.intensity == 0 && go.activeInHierarchy)
        {
            go.SetActive(false);
        }
        else if (lightSource.intensity > 0 && !go.activeInHierarchy)
        {
            go.SetActive(true);
        }
    }
}