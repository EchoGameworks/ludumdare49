using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private float damage = 5f;
    private float movementSpeed = 6f;

    private void Start()
    {
        gameObject.transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one, 0.3f).setEaseInOutCirc();
    }

    private void Update()
    {
        transform.position += transform.up * Time.deltaTime * movementSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //print("colliding");
        Beaker b = collision.gameObject.GetComponent<Beaker>();
        Goblin g = collision.gameObject.GetComponent<Goblin>();
        if (b != null)
        {
            b.TakeDamage(damage, false);
        }
        else if(g != null)
        {
            print("giving fire damage to goblin");
            g.TakeDamage(damage);
        }
        Destroy(this.gameObject);
    }

}
