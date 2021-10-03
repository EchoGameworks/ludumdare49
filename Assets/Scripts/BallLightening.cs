using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLightening : MonoBehaviour
{
    GameObject player;
    float startingMoveSpeed = 0;
    float accelerationAmount = 0.005f;
    float moveSpeed;
    private float scoreMulti = 1f;

    float damage = 10f;

    private bool Prep = true;
    Collider2D c2d;

    float timer;
    float timerMax = 6f;

    bool isDestroyed = false;
    // Start is called before the first frame update
    void Start()
    {
        c2d = GetComponent<Collider2D>();
        c2d.enabled = false;
        timer = timerMax;
        player = GameObject.FindGameObjectWithTag("Player");
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, Vector3.one, 0.3f);
        LeanTween.moveLocalY(gameObject, 3f, 1f).setOnComplete(() =>
            {
                c2d.enabled = true;
                Prep = false;
            });       
        moveSpeed = startingMoveSpeed;
    }

    public void SetScoreMulti(int val)
    {
        if (val > 10) val = 10;
        scoreMulti = val;
    }

    // Update is called once per frame
    void Update()
    {
        if (Prep) return;
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if(!isDestroyed)
        {
            DestroySelf();
        }
        moveSpeed += accelerationAmount * scoreMulti / 6f;
        if (moveSpeed > 5f) moveSpeed = 5f;   
        //print("moveSpeed: " + moveSpeed);
        //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position), rotationSpeed * Time.deltaTime);
        //transform.right = player.transform.position - transform.position;
        //transform.position += transform.forward * Time.deltaTime * moveSpeed;
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, step);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Goblin g = collision.gameObject.GetComponent<Goblin>();
        if (g != null)
        {
            g.TakeDamage(damage);
            Destroy(this.gameObject);
        }        
    }

    private void DestroySelf()
    {
        isDestroyed = true;
        LeanTween.scale(gameObject, Vector3.zero, 1f).setEaseInOutCirc()
            .setOnComplete(() => Destroy(this.gameObject));
    }
}
