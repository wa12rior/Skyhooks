using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootLasers : MonoBehaviour
{
    public GameObject laserPrefab;
    public int lasersCount = 1;
    public float respawnTime = 4.0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(lasers());
    }

    private void spawnLaser() {
        GameObject l = Instantiate(laserPrefab) as GameObject;
        l.transform.position = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    IEnumerator lasers() {
        while(true) {
            for (int i = 0; i < lasersCount; i++) {
                spawnLaser();
                yield return new WaitForSeconds(0.25f);
            }
            yield return new WaitForSeconds(respawnTime);
        }
    }
}
