using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Goblin : MonoBehaviour
{
    Controls ctrl;
    Animator anim;
    GameManager gameManager;

    public float Health;
    private float damageDampening = 5000f;

    private SpriteRenderer spriteRend;
    [SerializeField] private float m_MoveSpeed = 6.5f;
    [SerializeField] private float m_JumpForce = 85f;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .01f;
    [SerializeField] private bool m_AirControl = true;
    [SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private Transform m_GroundCheck;

    const float k_GroundedRadius = .3f;
    [SerializeField] private bool m_Grounded;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;
    private Vector3 m_Velocity = Vector3.zero;
    private float move;
    private bool jump;
    private Vector3 previousPosition;

    public float drag_Grounded = 1f;
    public float drag_Jump = 1f;
    public float fallMultiplier = 3.2f;
    public float ThrowPower = 35f;

    [SerializeField]
    private LayerMask layerObstacle;
    [SerializeField]
    private Transform holdPositionTransform;
    public GameObject HeldItem;
    private Rigidbody2D HeldItemRB2D;
    private IHoldable HeldItemScript;

    [SerializeField]
    private List<PoisonCloud> poisonList;

    private float coldTimerMax;
    private float coldTimer;
    private float coldSlow = 0.5f;
    private float coldSlowJump = 0.7f;

    void Start()
    {
        Health = 100f;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        gameManager = GameManager.instance;
        ctrl = gameManager.ctrl;
        ctrl.Player.Move.performed += Move_performed;
        ctrl.Player.Jump.performed += Jump_performed;
        ctrl.Player.Action.performed += Action_performed;
        ctrl.Player.Move.canceled += Move_canceled;
        poisonList = new List<PoisonCloud>();
    }

    private void OnDisable()
    {
        ctrl.Player.Move.performed -= Move_performed;
        ctrl.Player.Jump.performed -= Jump_performed;
        ctrl.Player.Action.performed -= Action_performed;
        ctrl.Player.Move.canceled -= Move_canceled;
    }

    private void OnDestroy()
    {
        ctrl.Player.Move.performed -= Move_performed;
        ctrl.Player.Jump.performed -= Jump_performed;
        ctrl.Player.Action.performed -= Action_performed;
        ctrl.Player.Move.canceled -= Move_canceled;
    }

    private void Move_canceled(InputAction.CallbackContext obj)
    {
        move = 0f;
    }

    private void Action_performed(InputAction.CallbackContext obj)
    {
        if(HeldItem != null)
        {
            //print("throw");
            Throw();
        }
        else
        {
            //print("pickup");
            Pickup();
        }
    }

    private void Jump_performed(InputAction.CallbackContext obj)
    {
        //print("trying to jump");
        jump = true;
    }

    private void Move_performed(InputAction.CallbackContext obj)
    {
        
        float vMove = obj.ReadValue<float>();
        //print("trying to move:" + vMove);
        move = vMove;
    }

    private void Pickup()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 1f, layerObstacle);
        if (hit.collider != null)
        {
            HeldItemRB2D = hit.collider.gameObject.GetComponent<Rigidbody2D>();
            if(HeldItemRB2D != null)
            {
                HeldItem = HeldItemRB2D.gameObject;
                HeldItemScript = HeldItem.GetComponent<IHoldable>();
                HeldItemRB2D.isKinematic = true;
                LeanTween.move(HeldItem, holdPositionTransform, 0.3f).setEaseInOutCirc()
                    .setOnComplete(() => HeldItemScript.followPosition = holdPositionTransform);
                //HeldItem.transform.parent = this.transform;
                anim.SetBool("IsHolding", true);
                //print("picked up:" + HeldItem);
            }            
        }
    }

    private void Throw()
    {
        if (HeldItem != null) {
            HeldItem = HeldItemRB2D.gameObject;
            HeldItemRB2D.isKinematic = false;
            anim.SetBool("IsHolding", false);
            HeldItemScript.followPosition = null;
            HeldItemScript.wasThrown = true;
            if (m_FacingRight && move != 0)
            {
                HeldItemRB2D.AddForce((gameObject.transform.up - gameObject.transform.right) * ThrowPower, ForceMode2D.Impulse);
            }
            else if(move != 0)
            {
                HeldItemRB2D.AddForce((gameObject.transform.up + gameObject.transform.right) * ThrowPower, ForceMode2D.Impulse);
            }
            else
            {
                HeldItemRB2D.AddForce((gameObject.transform.up) * ThrowPower, ForceMode2D.Impulse);
            }
            HeldItem = null;
            HeldItemRB2D = null;
            HeldItemScript = null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Dungeon") return;
        float impulse = 0F;
        foreach (ContactPoint2D point in collision.contacts)
        {
            if(point.normal.y < 0)
            {
                //print(point.normal);
                impulse += point.normalImpulse;
            }
            
        }

        float impact = impulse / Time.fixedDeltaTime / damageDampening;
        TakeDamage(impact);
        //Debug.Log(impact);
    }

    public void Freeze(Score score)
    {
        coldTimerMax = score.Value * 1f;
        coldTimer = coldTimerMax;
        spriteRend.color = new Color(0.6f, .45f, 1f, 1f);
    }

    public void TakeDamage(float dmg)
    {
        Health -= dmg;
        //print("took damage??" + dmg);
        if (dmg > 0.5f)
        {
            AudioManager.instance.PlaySound(AudioManager.SoundEffects.TakeDamage);
            CameraController.instance.ShakeScreen(dmg * 2f);
        }
        HUDManager.instance.UpdateHealth(Health);
        if (Health <= 0f)
        {
            GameManager.instance.GameOver();
            //print("dead");
        }
    }

    public void AddPoison(PoisonCloud pc)
    {
        poisonList.Add(pc);
    }

    public void RemovePoison(PoisonCloud pc)
    {
        poisonList.Remove(pc);
    }

    private void Update()
    {
        if (poisonList.Count > 0)
        {
            //print("poison Count: " + poisonList.Count);
            TakeDamage(1 / 800f / Time.deltaTime);
        }
        if (coldTimer > 0f)
        {
            coldTimer -= Time.deltaTime;
        }
        else
        {
            spriteRend.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                //if (!wasGrounded) AudioManager.instance.PlaySound(AudioManager.SoundEffects.CharacterLanding);
            }
        }


        Move();
    }

    public void Move()
    {

        float finalMove = (coldTimer > 0f) ? move * coldSlow : move;


        if (m_Grounded || m_AirControl)
        {
            //if (!m_Grounded) finalMove = move * 0.4f;
            // Move the character by finding the target velocity
            Vector3 targetVelocity = new Vector2(finalMove * m_MoveSpeed, m_Rigidbody2D.velocity.y);

            // And then smoothing it out and applying it to the character
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

            // If the input is moving the player right and the player is facing left...
            if (finalMove < 0 && !m_FacingRight)
            {
                Flip();
            }
            else if (finalMove > 0 && m_FacingRight)
            {
                Flip();
            }


            anim.SetFloat("Speed", Mathf.Abs(targetVelocity.x));


        }

        if (m_Rigidbody2D.velocity.y < 0)
        {
            anim.SetBool("IsFalling", true);
            anim.SetBool("IsJumping", false);
        }

        previousPosition = this.transform.position;


        if (m_Grounded && jump)
        {
            // Add a vertical force to the player.
            jump = false;
            m_Grounded = false;
            if(coldTimer > 0)
            {
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce * coldSlowJump), ForceMode2D.Impulse);
            }
            else
            {
                m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce), ForceMode2D.Impulse);
            }
            
            anim.SetBool("IsJumping", true);

        }

        if (m_Grounded)
        {
            anim.SetBool("IsFalling", false);
        }
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

}
