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
    private float boostTimeLeft;

    private Rigidbody2D rb;
    private LineRenderer lr;

    private bool attached;
    private Vector3 attPoint;

    private AudioManager audioPlayer;
    private bool jetpacking;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        attached = false;
        attPoint = new Vector3();
        lr.positionCount = 0;
        audioPlayer = FindObjectOfType<AudioManager>();
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
            if (!jetpacking)
            {
                audioPlayer.play("Jetpack");
            }
            jetpacking = true;
        }
        else
        {
            if (jetpacking)
            {
                audioPlayer.stop("Jetpack");
                audioPlayer.play("JetpackEnd");
            }
            jetpacking = false;
        }

        float boostForce = (boostTimeLeft > 0) ? boostStrength : 0;

        rb.AddForce(input * (jetpackForce + boostForce));

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        if(boostTimeLeft > 0)
        {
            boostTimeLeft -= Time.deltaTime;
        }


        if (attached)
        {
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, (Vector3)attPoint);

            rb.AddForce((attPoint - transform.position) * grappleStrength);
        }
    }
}
