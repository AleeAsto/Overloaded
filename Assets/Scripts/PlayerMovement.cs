using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidad = 10f;
    public float velocidadEscalera = 4f;

    private Animator anim;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    private float movX;
    private float movY;
    private bool enEscalera;
    public Collider2D pisoSuperiorCollider;

    public float exitCooldown = 0.25f;
    private float exitCooldownUntil = 0f;

    private float gravedadOriginal;

    void Awake()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        gravedadOriginal = rb.gravityScale;
    }

    void Update()
    {
        movX = Input.GetAxisRaw("Horizontal");
        movY = Input.GetAxisRaw("Vertical");

        if (movX != 0)
            sr.flipX = movX < 0;

        if (enEscalera && Mathf.Abs(movX) > 0.1f)
        {
            StartExitLadder();
        }

        bool isWalking = !enEscalera && Mathf.Abs(movX) > 0.1f;
        anim.SetBool("Walk", isWalking);
    }

    void FixedUpdate()
    {
        if (enEscalera)
        {
            rb.gravityScale = 0f;
            rb.linearVelocity = new Vector2(0f, movY * velocidadEscalera);
        }
        else
        {
            rb.gravityScale = gravedadOriginal;
            rb.linearVelocity = new Vector2(movX * velocidad, rb.linearVelocity.y);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Escalera"))
        {
            if (pisoSuperiorCollider != null)
                pisoSuperiorCollider.enabled = false;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Escalera")) return;

        if (Time.time < exitCooldownUntil)
            return;

        if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) <= 0.1f)
        {
            enEscalera = true;
            pisoSuperiorCollider.enabled = false;
        }
        else
        {
            enEscalera = false;
            pisoSuperiorCollider.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Escalera"))
        {
            if (pisoSuperiorCollider != null)
                pisoSuperiorCollider.enabled = true;

            enEscalera = false;
        }
    }

    void StartExitLadder()
    {
        enEscalera = false;
        if (pisoSuperiorCollider != null)
            pisoSuperiorCollider.enabled = true;

        exitCooldownUntil = Time.time + exitCooldown;
    }
}
