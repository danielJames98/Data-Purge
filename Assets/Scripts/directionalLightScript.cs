using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class directionalLightScript : MonoBehaviour
{
    public Light lightComponent;
    public float changeRate;
    public float pauseTime;
    public float maxAngle;
    public float minAngle;
    public float colourMin;
    public float colourMax;
    public float intensityMin;
    public float intensityMax;
    public float shadowStrengthMin;
    public float shadowStrengthMax;

    void Start()
    {
        lightComponent = this.GetComponent<Light>();
        setVariables();             
        StartCoroutine(ColorChangeRoutine());
        StartCoroutine(RotationChangeRoutine());
        StartCoroutine(IntensityChangeRoutine());
        StartCoroutine(ShadowStrengthChangeRoutine());
    }

    public void setVariables()
    {
        this.transform.eulerAngles = new Vector3(UnityEngine.Random.Range(minAngle, maxAngle), 0, UnityEngine.Random.Range(-minAngle, maxAngle));
        lightComponent.color = new Color32(System.Convert.ToByte(UnityEngine.Random.Range(colourMin, colourMax)), 
            System.Convert.ToByte(UnityEngine.Random.Range(colourMin, colourMax)), 
            System.Convert.ToByte(UnityEngine.Random.Range(colourMin, colourMax)), 255);
        lightComponent.intensity = UnityEngine.Random.Range(intensityMin, intensityMax);
        lightComponent.shadowStrength = UnityEngine.Random.Range(shadowStrengthMin, shadowStrengthMax);
    }



    private IEnumerator ColorChangeRoutine()
    {
        while (true)
        {
            var startColor = lightComponent.color;
            var endColor = new Color32(System.Convert.ToByte(UnityEngine.Random.Range(colourMin, colourMax)), 
                System.Convert.ToByte(UnityEngine.Random.Range(colourMin, colourMax)), 
                System.Convert.ToByte(UnityEngine.Random.Range(colourMin, colourMax)), 255);

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * changeRate;
                lightComponent.color = Color.Lerp(startColor, endColor, t);
                yield return null;
            }

            yield return new WaitForSeconds(pauseTime);
        }
    }

    private IEnumerator RotationChangeRoutine()
    {
        while (true)
        {
            var startRot = this.transform.eulerAngles;
            var endRot = new Vector3(UnityEngine.Random.Range(minAngle, maxAngle), 0, UnityEngine.Random.Range(minAngle, maxAngle));

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * changeRate;
                this.transform.eulerAngles = new Vector3(Mathf.LerpAngle(startRot.x, endRot.x, t), Mathf.LerpAngle(startRot.y, endRot.y, t), Mathf.LerpAngle(startRot.z, endRot.z, t));                
                yield return null;
            }

            yield return new WaitForSeconds(pauseTime);
        }
    }

    private IEnumerator IntensityChangeRoutine()
    {
        while (true)
        {
            var startFloat = lightComponent.intensity;
            var endFloat = UnityEngine.Random.Range(intensityMin, intensityMax);

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * changeRate;
                lightComponent.intensity =Mathf.Lerp(startFloat, endFloat, t);
                yield return null;
            }

            yield return new WaitForSeconds(pauseTime);
        }
    }

    private IEnumerator ShadowStrengthChangeRoutine()
    {
        while (true)
        {
            var startFloat = lightComponent.shadowStrength;
            var endFloat = UnityEngine.Random.Range(shadowStrengthMin, shadowStrengthMax);

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * changeRate;
                lightComponent.shadowStrength = Mathf.Lerp(startFloat, endFloat, t);
                yield return null;
            }

            yield return new WaitForSeconds(pauseTime);
        }
    }
}
