using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager instance;
    public Volume volume;
    private Bloom bloom;

    void Awake()
    {
        //Singleton
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        volume.profile.TryGet(out bloom);
    }

    public void SplashBloom()
    {
        var seq = LeanTween.sequence();
        seq.append(
            LeanTween.value(gameObject, bloom.threshold.value, 0.5f, 1f)
                .setOnUpdate((float i) => { bloom.threshold.value = i; })
                .setEaseInOutCirc());
        seq.append(0.5f);
        seq.append(
            LeanTween.value(gameObject, bloom.threshold.value, 2f, 2f)
            .setOnUpdate((float i) => { bloom.threshold.value = i; })
            .setEaseInOutCubic());
    }
}
