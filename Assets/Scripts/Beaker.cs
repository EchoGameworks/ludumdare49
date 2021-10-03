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

    private float dropDampening = 150f;

    [SerializeField]
    GameObject DestroyedAnimation;

    public Transform BeakerListContainer;
    public GameObject prefabPoisonCloudManager;
    public GameObject prefabBallLightening;

    public bool IsInCombo = false;

    private float sleepTimer;

     void Awake()
    {
        Element = (ElementTypes)Random.Range(0, 5); //0,4
        SetElement(Element);
        Health = 100f;
        DamageStatus = DamagedAmount.None;
        GlassDamageSmall.SetActive(false);
        GlassDamageLarge.SetActive(false);
        
        rb2d = GetComponent<Rigidbody2D>();        
    }

    public void SetElement(ElementTypes et)
    {
        
        waveSR.color = Helpers.GetElementColor(Element);
    }
    
    void Update()
    {
        waveAnim.SetBool("IsFalling", rb2d.velocity.y < -1);
        if(followPosition != null)
        {
            this.transform.position = followPosition.position;
        }

        if (wasThrown)
        {
            if (rb2d.velocity.magnitude > 0.3f)
            {
                //Still moving a lot
                sleepTimer = 0.5f;
            }
            else if (sleepTimer < 0f)
            {
                //print("isSleeping");
                wasThrown = false;
            }
            else
            {
                sleepTimer -= Time.deltaTime;
            }
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
        IHoldable iHold = collision.gameObject.GetComponent<IHoldable>();
        if (collision.gameObject.tag == "Player")
        {
            TakeDamage(impact * 1.6f, true);           
        }
        else if(iHold != null)
        {
            TakeDamage(impact, true);
        }
        else
        {
            TakeDamage(impact, false);
        }
        //Debug.Log(impact);
    }

    public void TakeDamage(float dmg, bool isPlayer)
    {
        if (IsInCombo) return;
        Health -= dmg;
        if (dmg > 10f)
        {
            int soundNum = Random.Range(0, 3);
            if(soundNum == 0)
            {
                AudioManager.instance.PlaySound(AudioManager.SoundEffects.GlassTink);
            }
            else if(soundNum == 1)
            {
                AudioManager.instance.PlaySound(AudioManager.SoundEffects.GlassTink2);
            }
            else
            {
                AudioManager.instance.PlaySound(AudioManager.SoundEffects.GlassTink3);
            }
            
        }
        if (Health < 0f)
        {
            CreateCombo(isPlayer);
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

    private void CreateCombo(bool isPlayer)
    {
        //print("creating combo: " + Element.ToString());

        
        IsInCombo = true;
        List<Beaker> BeakersConnected = new List<Beaker>();
        BeakersConnected.Add(this);
        TouchingBeakers(BeakersConnected);
       
        //print("Beakers Touching: " + BeakersConnected.Count);

        Score score = new Score(Element, BeakersConnected.Count);
        if(isPlayer) HUDManager.instance.UpdateScore(score);
        for (int i = BeakersConnected.Count - 1; i > 0; i--)
        {
            BeakersConnected[i].PlayBreak();
        }


        //print("should be invoking reaction: " + Element);
        switch (score.Element)
        {
            case ElementTypes.Fire:               
                FireballManager.instance.SpawnFireballs(score);                
                break;
            case ElementTypes.Electricity:
                GameObject blGO = Instantiate(prefabBallLightening, this.transform.position, Quaternion.identity, null);
                BallLightening bl = blGO.GetComponent<BallLightening>();
                bl.SetScoreMulti(score.Value);
                break;
            case ElementTypes.Earth:
                BoulderSpawner.instance.SpawnBoulders(score);
                break;
            case ElementTypes.Ice:
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<Goblin>().Freeze(score);
                AudioManager.instance.PlaySound(AudioManager.SoundEffects.Freeze);
                break;
            case ElementTypes.Poison:
                GameObject pcmGO = Instantiate(prefabPoisonCloudManager, this.transform.position, Quaternion.identity, null);
                PoisonCloudManager pcm = pcmGO.GetComponent<PoisonCloudManager>();
                pcm.SpawnCloud(score);
                AudioManager.instance.PlaySound(AudioManager.SoundEffects.Poison);
                break;
        }

        PlayBreak();
    }

    public void TouchingBeakers(List<Beaker> connectedBeakers)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.position, 1.07f);
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
            AudioManager.instance.PlaySound(AudioManager.SoundEffects.GlassBreak);
            GameObject explosionGO = Instantiate(DestroyedAnimation, this.transform.position, Quaternion.Euler(0f, 0f, Random.Range(0.0f, 360.0f)), null);
            explosionGO.GetComponent<SpriteRenderer>().color = Helpers.GetElementColor(Element);            
        }

        Destroy(this.gameObject);
        

    }
}
