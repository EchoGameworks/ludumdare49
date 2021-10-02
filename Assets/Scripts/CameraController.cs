using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public float shakeDurationMax = 1f;
    public float shakeDuration;
    public float shakeMagnitude = 1f;
    public float dampingSpeed = 0.3f;
    private Vector3 initialPosition;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        initialPosition = this.transform.position;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            transform.localPosition = initialPosition;
        }
    }

    public void ShakeScreen(float force)
    {
        float clampedForce = Mathf.Clamp(force, 0f, 20f);
        float rescaledTime = scale(0f, 25f, 0.01f, 0.2f, clampedForce);
        float rescaledForce = scale(0f, 25f, 0.01f, 0.065f, clampedForce);
        shakeMagnitude = rescaledForce;
        shakeDuration = rescaledTime;
    }

    public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
    {

        float OldRange = (OldMax - OldMin);
        float NewRange = (NewMax - NewMin);
        float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

        return (NewValue);
    }
}
