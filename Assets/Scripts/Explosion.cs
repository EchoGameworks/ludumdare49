using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one, 0.3f).setEaseInOutCirc()
            .setOnComplete(() => LeanTween.scale(gameObject, Vector3.one * 3f, 0.5f).setLoopPingPong(1))
            .setOnComplete(() => Destroy(this.gameObject));
    }

    private void Update()
    {
        this.transform.Rotate(new Vector3(0f, 0f, Random.Range(0f, 20f)));
    }


}
