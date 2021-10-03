using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloudManager : MonoBehaviour
{

    public GameObject prefabPoisonCloud;
    private BoxCollider2D bc2d;

    private void Start()
    {
        
        Destroy(this.gameObject, 10f);
    }

    public void SpawnCloud(Score score)
    {
        bc2d = GetComponent<BoxCollider2D>();
        bc2d.size = new Vector2(1 + score.Value / 6f, 1 + score.Value / 4f);

        for (int i = 0; i <= score.Value; i++)
        {
            LeanTween.delayedCall(Random.Range(0f, 1f), () =>
            {
                Vector3 point = new Vector3(
                    Random.Range(bc2d.bounds.min.x, bc2d.bounds.max.x),
                    Random.Range(bc2d.bounds.min.y, bc2d.bounds.max.y),
                    Random.Range(bc2d.bounds.min.z, bc2d.bounds.max.z)
                        );
                GameObject bGO = Instantiate(prefabPoisonCloud, point, Quaternion.identity, this.transform);
                bGO.transform.localScale = Vector3.zero;
                LeanTween.scale(bGO, Vector3.one, 0.5f).setEaseInOutCirc();
            });

            
        }        
    }
}
