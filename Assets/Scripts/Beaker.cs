using Constants;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beaker : IHoldable
{
    public float Health = 100f;
    public ElementTypes Element;
    public DamagedAmount DamageStatus;
    
    [SerializeField]
    private SpriteRenderer waveSR;
    [SerializeField]
    private Animator waveAnim;
    [SerializeField]
    private GameObject GlassDamageSmall;
    [SerializeField]
    private GameObject GlassDamageLarge;
    public Rigidbody2D rb2d;

    private float dropDampening = 200f;

    [SerializeField]
    GameObject DestroyedAnimation;

    public Transform BeakerListContainer;
    

    public bool IsInCombo = false;

     void Awake()
    {
        Element = (ElementTypes)Random.Range(0, 4);
        waveSR.color = Helpers.GetElementColor(Element);        
        Health = 100f;
        DamageStatus = DamagedAmount.None;
        GlassDamageSmall.SetActive(false);
        GlassDamageLarge.SetActive(false);
        
        rb2d = GetComponent<Rigidbody2D>();        
    }

    
    void Update()
    {
        waveAnim.SetBool("IsFalling", rb2d.velocity.y < -1);
        if(followPosition != null)
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
        TakeDamage(impact);
        //Debug.Log(impact);
    }

    private void TakeDamage(float dmg)
    {
        if (IsInCombo) return;
        Health -= dmg;
        if(Health < 0f)
        {
            CreateCombo();
        }
        else if(Health > 30f && Health <= 65f)
        {
            DamageStatus = DamagedAmount.Small;
            GlassDamageSmall.SetActive(true);
        }
        else if(Health <= 30f)
        {
            DamageStatus = DamagedAmount.Large;
            GlassDamageSmall.SetActive(false);
            GlassDamageLarge.SetActive(true);
        }
    }

    private void CreateCombo()
    {
        print("creating combo: " + Element.ToString());

        
        IsInCombo = true;
        List<Beaker> BeakersConnected = new List<Beaker>();
        BeakersConnected.Add(this);
        TouchingBeakers(BeakersConnected);
       
        print("Beakers Touching: " + BeakersConnected.Count);

        Score score = new Score(Element, BeakersConnected.Count);
        HUDManager.instance.UpdateScore(score);
        for (int i = BeakersConnected.Count - 1; i > 0; i--)
        {
            BeakersConnected[i].PlayBreak();
        }

        PlayBreak();
    }

    public void TouchingBeakers(List<Beaker> connectedBeakers)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, 1.1f);
        foreach(Collider2D c2D in colliders)
        {
            Beaker b = c2D.GetComponent<Beaker>();
            if (b != null)
            {
                if(b.Element == this.Element && !connectedBeakers.Contains(b))
                {
                    connectedBeakers.Add(b);
                    b.TouchingBeakers(connectedBeakers);
                }
            }
        }   
    }

    public void PlayBreak()
    {
        if (DestroyedAnimation != null)
        {
            GameObject explosionGO = Instantiate(DestroyedAnimation, this.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)), null);
            explosionGO.GetComponent<SpriteRenderer>().color = Helpers.GetElementColor(Element);
            Destroy(explosionGO, 0.5f);
        }
        Destroy(this.gameObject);
    }
}
