using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OriginIndicator : MonoBehaviour
{
    void Update()
    {
        Vector3 dir = - transform.position;
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.back);
    }
}
