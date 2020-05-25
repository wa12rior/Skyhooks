using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lasers : MonoBehaviour
{
    [SerializeField]
    private float speed = 10.0f;
    // Start is called before the first frame update
    [Range(-1,1)]
    public int vertical = 1;
    [Range(-1,1)]
    public int horizontal = 1;
    private Rigidbody2D rg2D;
    void Start()
    {
        rg2D = this.GetComponent<Rigidbody2D>();
        rg2D.velocity = new Vector2(speed * horizontal, speed * vertical);
    }

    private void OnBecameInvisible() {
        Destroy(this.gameObject);
    }

}
