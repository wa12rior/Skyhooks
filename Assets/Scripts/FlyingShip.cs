using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class FlyingShip : MonoBehaviour
{

    public float speed = 4f;

    public Camera mainCam;
    public float cameraOffsetX;
    public float cameraOffsetY = 4;
    public float cameraTransitionSpeed = 0.8f; 

    private Vector3 position;
    private AudioSource audioSrc;

    bool _isOnTrigger = false;
    private GameObject objInTrigger;
    private GameObject backFire;


    private float orbitingSide;
    public bool IsOnTrigger {
        get {
            return _isOnTrigger;
        }
        private set {
            _isOnTrigger = value;
        }
    }

    void Start()
    {
        //Get audio source component.
        audioSrc = GameObject.FindGameObjectWithTag("LoseAudio").GetComponent<AudioSource>();
        backFire = GameObject.FindGameObjectWithTag("BackFire");
        backFire.SetActive(false);
    }

    void FixedUpdate()
    {
        // We check if player is in the gravity field.
        if (_isOnTrigger) {
            // Calculate the angle of rotation of the ship.
            float[] from = { transform.position.x, transform.position.y };
            float[] to = { objInTrigger.gameObject.transform.position.x, objInTrigger.gameObject.transform.position.y };
            // rotating player when enters into gravity field
            transform.rotation = Quaternion.Euler(Vector3.forward * calculateAngle(from, to, orbitingSide));
            // We rotate around planet.
            transform.RotateAround(objInTrigger.transform.position, new Vector3(0, 0, 1), orbitingSide * speed);
            // Change camera offset in order to see the rest of the map(next planet to hop).
            cameraOffsetY = 2;
            // And move camera.
            moveCameraSmoothlyTo(mainCam, objInTrigger, cameraTransitionSpeed);
            // Check if user wants to leave the orbit
            for (int i = 0; i < Input.touchCount; ++i) {
                if (Input.GetTouch(i).phase == TouchPhase.Began) {
                    // We disable rotating around and gravity effect from the planet
                    _isOnTrigger = false;
                    PointEffector2D pe = objInTrigger.GetComponent<PointEffector2D>();
                    pe.enabled = false;

                    // Backfire animation
                    StartCoroutine(handleBackFire());
                }
            }
        } else {
            // Camera stays on the same offset.
            cameraOffsetY = 4;
            
            // Here we are moving ship and following it with camera.
            transform.Translate(0, speed * Time.deltaTime, 0);
            moveCameraSmoothlyTo(mainCam, gameObject, cameraTransitionSpeed);
        }

    }

    void decreaseSpeed(){
        speed -= 0.4f;
    }
 
    private IEnumerator handleBackFire() {
        backFire.SetActive(true);
        // process pre-yield
        yield return new WaitForSeconds (0.25f);
        // process post-yield
        backFire.SetActive(false);
    }

    void moveCameraSmoothlyTo(Camera camera, GameObject destObj, float speed) {
        Vector3 newPosition = new Vector3(destObj.transform.position.x + cameraOffsetX, destObj.transform.position.y + cameraOffsetY, -10);
        camera.transform.position = Vector3.Lerp (
            camera.transform.position,
            newPosition,
            Time.deltaTime * speed
        );
    }

    private void OnTriggerExit2D(Collider2D other) {
        CancelInvoke();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        switch(other.gameObject.tag) {
            case "Planet":
                if (other.gameObject.name == "PlanetEarth") {
                    GameManager gameM = new GameManager();
                    gameM.setGameUI(GameObject.FindGameObjectWithTag("Canvas").GetComponent<UnityEngine.Canvas>());
                    gameM.toggleInGameUI();
                    GameObject.FindGameObjectWithTag("YouWin").GetComponent<UnityEngine.UI.Text>().text = "You Win!";
                    GameObject.FindGameObjectWithTag("RestartButton").SetActive(false);
                    GameObject.FindGameObjectWithTag("SettingsButton").SetActive(false);
                    Time.timeScale = 0;
                } else {
                    // Game Over here.
                    audioSrc.Play();
                    Time.timeScale = 0;  
                }
                break;
            case "Gravity Field":
                // Calculate the angle of rotation of the ship.
                float[] from = { transform.position.x, transform.position.y };
                float[] to = { other.gameObject.transform.position.x, other.gameObject.transform.position.y };
                speed += 0.5f;
                InvokeRepeating("decreaseSpeed", 1.0f, 1.0f);
                
                // Should be equal to -1 or 1 regardless of dividing with abs function of the same value.
                orbitingSide = calculateOrbitingLine(from, to);
                orbitingSide = orbitingSide/Mathf.Abs(orbitingSide);
                
                // We check if pe is enabled. Then we active rotating by isOnTrigger = true.
                PointEffector2D pe = other.gameObject.GetComponent<PointEffector2D>();
                pe.enabled = pe.enabled ? pe.enabled : true;
                _isOnTrigger = true;
                objInTrigger = other.gameObject;

                break;
        }
    }

    float calculateAngle(float[] from, float[] to, float orbS) {
        // We calculate perpendicular line and returning the a.
        // y = ax + b  =>  y = -ax + b (perpendicular)
        Vector2 perp = Vector2.Perpendicular(new Vector2(to[0] - from[0], to[1] - from[1]));
        
        int fullAngle = 360;
        int orbAngle = 0;

        if (orbS == 1) {
            orbAngle += 180;
        }

        if (from[1] < to[1]) {
            return Vector2.Angle(perp, Vector2.up) + orbAngle;
        } else {
            return fullAngle - Vector2.Angle(perp, Vector2.up) + orbAngle;
        }
    }

    float calculateOrbitingLine(float[] from, float[] to) {
        // We simply return a from y = -ax + b
        return (to[1] - from[1])/(to[0] - from[0]);
    }
}
