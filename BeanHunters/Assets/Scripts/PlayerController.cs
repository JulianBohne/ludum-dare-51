using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float jetpackForce;
    public float maxSpeed;
    public float grappleStrength;
    public CooldownUI grappleCooldown;
    public CooldownUI boostCooldown;
    public TextMeshProUGUI enterShelterText;

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

    private ShelterManager sm;

    private CameraShake camShake;


    private bool pausedGame = false;
    public TextMeshProUGUI pausedLabel;

    public TextMeshProUGUI enterPodText;

    public enum State
    {
        SHELTER,
        OUTSIDE
    }

    [HideInInspector]
    public State currentState = State.OUTSIDE;

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

        sm = FindObjectOfType<ShelterManager>();

        camShake = FindObjectOfType<CameraShake>();

        pausedLabel.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pausedGame = !pausedGame;
            pausedLabel.enabled = pausedGame;
            audioPlayer.play("Pause");
            Time.timeScale = pausedGame ? 0.0f : 1.0f;
        }
        switch (currentState)
        {
            case State.OUTSIDE:
                updateOutside();
                break;
            case State.SHELTER:
                updateShelter();
                break;
        }
    }

    void updateOutside()
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
                    worldPosition.z = -1f;
                    attPoint = worldPosition;
                    attached = true;

                    StartCoroutine(camShake.Shake(0.1f, 0.1f));
                    audioPlayer.play("Hit");
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (boostCooldown.tryToUse())
            {
                audioPlayer.play("JetpackStart");
                StartCoroutine(camShake.Shake(boostTime, 0.1f));
                boostTimeLeft = boostTime;
            }
        }

        if (boostTimeLeft > 0)
        {
            boostTimeLeft -= Time.deltaTime;
            inputDir = rb.velocity.normalized;
            rb.AddForce(inputDir * boostStrength);
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

        

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        if (inputDir.x > 0.7)
            spriteRenderer.sprite = right;
        else if (inputDir.x < -0.7)
            spriteRenderer.sprite = left;
        else if (inputDir.y > 0)
            spriteRenderer.sprite = up;
        else spriteRenderer.sprite = down;

        float bobValue = Mathf.Sin(Time.time * bobSpeed);
        spriteTransform.localPosition = new Vector3(0, bobValue * bobHeight, 0);
        shadowRenderer.color = new Color32(0, 0, 0, (byte)(50 - (bobValue + 1) * 10));

#pragma warning disable CS0618 // Type or member is obsolete
        dust.emissionRate = Mathf.Clamp((float)(rb.velocity.magnitude - 7) * dustAmount, 0, float.MaxValue);
#pragma warning restore CS0618 // Type or member is obsolete

        if (enterPodText.enabled)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.LoadScene("PodScene");
            }
        }

        if (sm.isOverShelter(transform.position))
        {
            enterShelterText.enabled = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                enterShelter();
            }
        }
        else
        {
            enterShelterText.enabled = false;
        }

        if (attached)
        {
            lr.SetPosition(0, spriteTransform.position);
            lr.SetPosition(1, (Vector3)attPoint);

            rb.AddForce((attPoint - shadowTransform.position) * grappleStrength);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Pod")
        {
            enterPodText.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Pod")
        {
            enterPodText.enabled = false;
        }
    }

    void updateShelter()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            exitShelter();
        }
    }

    void enterShelter()
    {
        Vector3 shelterEntry = sm.closestShelter(transform.position) + Vector3.down * 0.5f;
        shelterEntry.z = transform.position.z;
        transform.position = shelterEntry;
        rb.velocity = Vector2.zero;

        lr.positionCount = 0;
        attached = false;

        disable();

        enterShelterText.SetText("Press 'E' to exit shelter");

        currentState = State.SHELTER;
    }

    void exitShelter()
    {
        enable();

        enterShelterText.SetText("Press 'E' to enter shelter");

        currentState = State.OUTSIDE;
    }

    private void disable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        audioPlayer.stop("Jetpack");

        GetComponent<CapsuleCollider2D>().enabled = false;
        lr.enabled = false;

        rb.isKinematic = true;
    }

    private void enable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        GetComponent<CapsuleCollider2D>().enabled = true;
        lr.enabled = true;

        rb.isKinematic = false;
    }
}
