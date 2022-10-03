using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetpackShake : MonoBehaviour
{

    private bool shake;
    public float magnitude = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        shake = false;
    }

    public void startShake()
    {
        shake = true;
    }

    public void stopShake()
    {
        shake = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (shake)
        {
            float xOffset = Random.Range(-0.5f, 0.5f) * magnitude;
            float yOffset = Random.Range(-0.5f, 0.5f) * magnitude;

            transform.localPosition = new Vector3(xOffset, yOffset, 0);
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
    }
}
