using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    private GameObject pickupEffect;

    private static float followForce = 13;
    private static float followSize = 2f;
    private static float pickupSize = 0.25f;

    private Rigidbody2D rb;
    private CircleCollider2D coll;
    private GameObject follow = null;
    private Rigidbody2D followRb;

    private InventoryManager invManager;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<CircleCollider2D>();
        coll.radius = followSize;

        invManager = GameObject.Find("Inventory").GetComponent<InventoryManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(follow is not null)
        {
            rb.AddForce((followRb.velocity - rb.velocity) * followForce/4);
            rb.AddForce((follow.transform.position - transform.position) * followForce);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(follow is null)
            {
                follow = other.gameObject;
                followRb = other.attachedRigidbody;
                coll.radius = pickupSize;
            }
            else
            {
                invManager.addItem(tag, 1);
                Instantiate(pickupEffect, transform.position, Quaternion.identity);
                FindObjectOfType<AudioManager>().play("Pickup");
                Destroy(gameObject);
            }
        }
    }
}
