using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float jetpackForce;
    public float maxSpeed;
    public float grappleStrength;
    public CooldownUI grappleCooldown;
    public CooldownUI boostCooldown;

    public float boostTime;
    public float boostStrength;

    public float dustAmount = 1.0f;
    public ParticleSystem dust;

    public Sprite down;
    public Sprite left;
    public Sprite right;
    public Sprite up;

    public float bobHeight = 0.1f;
    public float bobSpeed = 1f;

    private float boostTimeLeft;

    private Rigidbody2D rb;
    private LineRenderer lr;

    private bool attached;
    private Vector3 attPoint;

    private AudioManager audioPlayer;
    private bool jetpacking;

    private Vector2 inputDir = new Vector2();
    private Transform spriteTransform;
    private SpriteRenderer spriteRenderer;

    private Transform shadowTransform;
    private SpriteRenderer shadowRenderer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        attached = false;
        attPoint = new Vector3();
        lr.positionCount = 0;
        audioPlayer = FindObjectOfType<AudioManager>();
        spriteTransform = transform.Find("PlayerSprite");
        spriteRenderer = spriteTransform.GetComponent<SpriteRenderer>();


        shadowTransform = transform.Find("ShadowSprite");
        shadowRenderer = shadowTransform.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (attached)
            {
                grappleCooldown.enableCooldown();
                grappleCooldown.tryToUse();
                lr.positionCount = 0;
                attached = false;
            }
            else
            {
                grappleCooldown.disableCooldown();
                if (grappleCooldown.tryToUse())
                {
                    lr.positionCount = 2;
                    Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    worldPosition.z = 0.1f;
                    attPoint = worldPosition;
                    attached = true;

                    audioPlayer.play("Hit");
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (boostCooldown.tryToUse())
            {
                audioPlayer.play("JetpackStart");
                boostTimeLeft = boostTime;
            }
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (input.magnitude > 0.1)
        {
            inputDir = input;
            if (!jetpacking)
            {
                audioPlayer.play("Jetpack");
            }
            jetpacking = true;
        }
        else
        {
            input *= 0;
            if (jetpacking)
            {
                audioPlayer.stop("Jetpack");
                audioPlayer.play("JetpackEnd");
            }
            jetpacking = false;
        }


        rb.AddForce(input * jetpackForce);

        if(boostTimeLeft > 0)
        {
            boostTimeLeft -= Time.deltaTime;
            rb.AddForce(rb.velocity.normalized * boostStrength);
        }

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        if (inputDir.x > 0.7)
            spriteRenderer.sprite = right;
        else if (inputDir.x < -0.7)
            spriteRenderer.sprite = left;
        else if (inputDir.y > 0)
            spriteRenderer.sprite = up;
        else spriteRenderer.sprite = down;

        float bobValue = Mathf.Sin(Time.unscaledTime * bobSpeed);
        spriteTransform.localPosition = new Vector3(0, bobValue * bobHeight, 0);
        shadowRenderer.color = new Color32(0, 0, 0, (byte)(50-(bobValue + 1)*10));

        #pragma warning disable CS0618 // Type or member is obsolete
        dust.emissionRate = Mathf.Clamp((float)(rb.velocity.magnitude - 7) * dustAmount, 0, float.MaxValue);
        #pragma warning restore CS0618 // Type or member is obsolete

        if (attached)
        {
            lr.SetPosition(0, spriteTransform.position);
            lr.SetPosition(1, (Vector3)attPoint);

            rb.AddForce((attPoint - shadowTransform.position) * grappleStrength);
        }
    }
}
