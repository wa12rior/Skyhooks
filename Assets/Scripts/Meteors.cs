using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteors : MonoBehaviour
{
    public AudioSource audioSrc;
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player") {
            audioSrc.Play();
            Time.timeScale = 0;  
        }
    }

}
