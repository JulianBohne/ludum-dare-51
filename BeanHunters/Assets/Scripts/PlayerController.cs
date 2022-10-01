using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float jetpackForce = 1.0f;
    public float maxSpeed = 20f;

    private Rigidbody2D rb;
    private LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        rb.AddForce(input * jetpackForce);

        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);

        lr.SetPosition(0, transform.position);
    }
}
