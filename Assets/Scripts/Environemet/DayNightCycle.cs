using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Transform sun;

    [Header("Cycle Settings")]
    public float timeOfDay = 1350;
    public float cycleDuration = 2700f;
    public float dayStartTime = 750f;
    public float dayEndTime = 2150f;

    public float cycleSpeed = 1f;

    [Header("Lighting Settings")]
    public float dayTimeSunIntensity = 2.1f;
    public float nightTimeSunIntensity = 0;

    public float dayTimeAmbientIntensity = 1f;
    public float nightTimeAbientIntensity = 0.15f;

    public float intensityChangeSpeed = 1f;
    
    [HideInInspector] public bool isNightTime;
    public Material skybox;
    public Color dayTimeColor;
    public Color nightTimeColor;


    private void Start()
    {
        if (!isNightTime)
            sun.GetComponentInChildren<Light>().intensity = dayTimeSunIntensity;
        else
            sun.GetComponentInChildren<Light>().intensity = nightTimeSunIntensity;
    }

    private void Update()
    {
        Color currentTintColor = skybox.GetColor("_Tint");
        Color dayTintColor = Color.Lerp(currentTintColor, dayTimeColor, intensityChangeSpeed * Time.deltaTime);
        Color nightTintColor = Color.Lerp(currentTintColor, nightTimeColor, intensityChangeSpeed * Time.deltaTime);
        if (!isNightTime)
        {
            sun.GetComponentInChildren<Light>().intensity = Mathf.Lerp(sun.GetComponentInChildren<Light>().intensity, dayTimeSunIntensity, intensityChangeSpeed * Time.deltaTime);
            RenderSettings.ambientIntensity = Mathf.Lerp(RenderSettings.ambientIntensity, dayTimeAmbientIntensity, intensityChangeSpeed * Time.deltaTime);
            if(skybox != null)
                skybox.SetColor("_Tint", dayTintColor);
        }
        else
        {
            sun.GetComponentInChildren<Light>().intensity = Mathf.Lerp(sun.GetComponentInChildren<Light>().intensity, nightTimeSunIntensity, intensityChangeSpeed * Time.deltaTime);
            RenderSettings.ambientIntensity = Mathf.Lerp(RenderSettings.ambientIntensity, nightTimeAbientIntensity, intensityChangeSpeed * Time.deltaTime);
            if (skybox != null)
                skybox.SetColor("_Tint", nightTintColor);
        }

        if (timeOfDay > cycleDuration)
            timeOfDay = 0;


        if (timeOfDay > dayStartTime && timeOfDay < dayEndTime)
            timeOfDay += cycleSpeed * Time.deltaTime;
        else
            timeOfDay += (cycleSpeed * 2) * Time.deltaTime;

        UpdateLighting();

    }

    public void UpdateLighting()
    {
        sun.localRotation = Quaternion.Euler((timeOfDay * 360 / cycleDuration), 0, 0);

        if (timeOfDay < dayStartTime || timeOfDay > dayEndTime)
            isNightTime = true;
        else
            isNightTime = false;
    }
}
