using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderSpawner : MonoBehaviour
{
    public static BoulderSpawner instance;
    public GameObject prefabBoulder;

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

    }

    public void SpawnBoulders(Score score)
    {
        float delayTime = 0f;
        for (int i = 0; i <= score.Value; i++)
        {
            LeanTween.delayedCall(delayTime,
                () =>
                {
                    GameObject bGO = Instantiate(prefabBoulder, this.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)), this.transform);
                    bGO.transform.localScale = Vector3.zero;
                    LeanTween.scale(bGO, Vector3.one, 0.3f).setEaseInOutCirc();
                    bGO.GetComponent<Rigidbody2D>().AddForce(bGO.transform.up * 100f, ForceMode2D.Impulse);
                    AudioManager.instance.PlaySound(AudioManager.SoundEffects.Boulder);
                }
                );
            delayTime += 0.3f;
        }
    }
}
