using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public GameObject aroundObject;
    public float speed;

    void FixedUpdate()
    {
        transform.RotateAround(aroundObject.transform.position, new Vector3(0, 0, 1), speed);
    }
}
