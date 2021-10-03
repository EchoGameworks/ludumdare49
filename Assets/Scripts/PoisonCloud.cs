using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonCloud : MonoBehaviour
{

    private GameObject goblinGO;
    private Goblin goblin;

    private void Start()
    {
        LeanTween.delayedCall(Random.Range(3f, 8f), () => DestroySelf());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            goblinGO = collision.gameObject;
            goblin = goblinGO.GetComponent<Goblin>();
            goblin.AddPoison(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == goblinGO)
        {
            if(goblin != null)
            {
                goblin.RemovePoison(this);
            }
        }
    }

    private void DestroySelf()
    {
        if (goblin != null)
        {
            goblin.RemovePoison(this);
        }
        LeanTween.scale(gameObject, Vector3.zero, 0.5f).setEaseInOutCirc()
            .setOnComplete(() => FinalDeath()
                );
    }

    private void FinalDeath()
    {
        if(this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }
}
