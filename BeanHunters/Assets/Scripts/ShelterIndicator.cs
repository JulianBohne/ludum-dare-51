using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShelterIndicator : MonoBehaviour
{
    private ShelterManager sm;
    // Start is called before the first frame update
    void Start()
    {
        sm = FindObjectOfType<ShelterManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = sm.closestShelter(transform.position) - transform.position;
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
    }
}
