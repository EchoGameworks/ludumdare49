using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boulder : IHoldable
{
    float dropDampening = 300f;
    float Health = 50f;
    public GameObject DestroyedAnimation;

    void Update()
    {
        if (followPosition != null)
        {
            this.transform.position = followPosition.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float impulse = 0F;
        foreach (ContactPoint2D point in collision.contacts)
        {
            impulse += point.normalImpulse;
        }

        float impact = impulse / Time.fixedDeltaTime / dropDampening;
        if (collision.gameObject.tag == "Player") impact *= 1.5f;
        TakeDamage(impact);
        //Debug.Log(impact);
    }

    private void TakeDamage(float dmg)
    {
        Health -= dmg;
        if (Health < 0f)
        {
            GameObject explosionGO = Instantiate(DestroyedAnimation, this.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)), null);
            explosionGO.GetComponent<SpriteRenderer>().color = Helpers.GetElementColor(ElementTypes.Earth);
            Destroy(explosionGO, 0.4f);
            Destroy(gameObject);            
        }

    }
}
