using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float rotationAngle;
    void FixedUpdate()
    {
        transform.Rotate(0, 0, rotationAngle);
    }

}
