using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
    private float SPEED = 6f;
    private float BULLET_SPEED = 20.0f;
    private float FIRE_DELAY = 0.20f; // Time between each shot
    private float MUZZLE_FLASH_LIFE_TIME = 0.025f;
    private float RELOAD_TIME = 1f;
    public int MAX_BULLETS = 30;
    public int MAX_HEALTH = 100;
    private float lastShot = 0.0f;
    private bool isReloading = false;

    // Screen shake vars
    private Vector3 originalCameraPosition;
    private float shakeAmount = 0;

    public int currentBulletCount;
    public int health;

    public Camera mainCamera;
    public Scene game;
    public GameObject firePosition;
    public GameObject bullet;
    public GameObject muzzleFlash;
    public GameObject crosshair;

    // Use this for initialization
    void Start() {
        currentBulletCount = MAX_BULLETS;
        health = MAX_HEALTH;
    }

    // Update is called once per frame
    void Update() {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(moveHorizontal, moveVertical);

        Vector3 playerPos_world = transform.position;
        Vector3 mousePos_screen = Input.mousePosition;
        Vector3 mousePos_world = Camera.main.ScreenToWorldPoint(mousePos_screen);
        Vector3 mousePos = new Vector3(mousePos_world.x, mousePos_world.y, playerPos_world.z);
        Vector3 direction = playerPos_world - mousePos;

        this.rigidbody2D.velocity = move * SPEED;
        transform.rotation = Quaternion.LookRotation(transform.forward, -direction);

        // Handle reloading
        if (Input.GetKeyDown(KeyCode.R)) {  
            if (!isReloading) {
                StartCoroutine("Reload");
            }
        }

        if (shakeAmount > 0) {
            shakeAmount -= 0.2f;
        }
        // Handle firing bullets
        if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)) && lastShot >= FIRE_DELAY && currentBulletCount > 0 && !isReloading) {

            // Instantiate a new bullet and give it the same rotation as the player and starting position
            // as the empty child firePosition game object
            GameObject clone = Instantiate(bullet, firePosition.transform.position, Quaternion.identity) as GameObject;
            clone.transform.rotation = transform.rotation;

            // Calculate velocity vector by using the current rotation of the player
            Quaternion rotDir = Quaternion.AngleAxis(clone.transform.rotation.eulerAngles.z, Vector3.right);
            Vector3 ldir = rotDir * Vector3.forward;

            clone.rigidbody2D.velocity = new Vector2(
                ldir.normalized.y * BULLET_SPEED,
                ldir.normalized.z * BULLET_SPEED);

            HandleEffects(firePosition.transform.position, clone.transform.rotation);
            currentBulletCount--;
            lastShot = 0;
        }

        // Auto reload once we reach 0 bullets
        if (currentBulletCount == 0 && !isReloading) {
            StartCoroutine("Reload");
        }

        lastShot += Time.deltaTime;
    }


    void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log ("Player colliding:" + other.name);

    }

    void HandleEffects(Vector2 flashPosition, Quaternion flashRotation) {
        GameObject muzzleClone = Instantiate(muzzleFlash, flashPosition, Quaternion.identity) as GameObject;
        muzzleClone.transform.rotation = flashRotation;
        Destroy(muzzleClone, MUZZLE_FLASH_LIFE_TIME);
    }

    IEnumerator Reload() {
        Debug.Log("Reloading");
        isReloading = true;
        yield return new WaitForSeconds(RELOAD_TIME);
        Debug.Log("Done reloading");
        currentBulletCount = MAX_BULLETS;
        isReloading = false;
    }

    public void TakeDamage() {
        health--;
        if (health <= 0) {
            game.HandleGameOver();
        }
    }
}
