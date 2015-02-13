using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

    public enum EnemyState {
        PATROL = 0,
        CHASE = 1,
        AVOID = 2
    };

    private float MOVE_SPEED = 100f;
    private float FIRE_DELAY = 0.25f;
    private float BULLET_SPEED = 10.0f;
    private float lastShot = 0;
    private float lastDirectionChange = 0;
    private float CHANGE_DIRECTION_DELAY = 1.0f;
    private Vector2 moveDirection;
    private EnemyState state;
    private bool firing;

    public GameObject player;
    public GameObject bullet;
    public GameObject firePosition;
    public int maxHealth;
    public int health;

    // Use this for initialization
    void Start() {
        moveDirection = Vector2.zero;
        //state = (EnemyState) Random.Range (0, 1);
        state = EnemyState.PATROL;
        player = GameObject.FindWithTag("Player");

        health = maxHealth;
    }

    void Update() {
        if (lastShot >= FIRE_DELAY && firing) {
            FireBullet();
            lastShot = 0;
        }

        Move();

        this.rigidbody2D.velocity = moveDirection.normalized * MOVE_SPEED * Time.deltaTime * -1;

        lastShot += Time.deltaTime;

    }

    void FireBullet() {
        // Instantiate a new bullet and give it the same rotation as the enemy and starting position
        // as the empty child firePosition game object
        GameObject clone = Instantiate(bullet, firePosition.transform.position, Quaternion.identity) as GameObject;
        clone.transform.rotation = transform.rotation;

        // Calculate velocity vector by using the current rotation of the player
        Quaternion rotDir = Quaternion.AngleAxis(clone.transform.rotation.eulerAngles.z, Vector3.right);
        Vector3 ldir = rotDir * Vector3.forward;
        clone.rigidbody2D.velocity = new Vector2(
            ldir.normalized.y * BULLET_SPEED,
            ldir.normalized.z * BULLET_SPEED);
    }

    void Move() {
        switch (state) {
            case EnemyState.PATROL:
                Patrol();
                break;
            case EnemyState.CHASE:
                Chase();
                break;
            case EnemyState.AVOID:
            default:
                break;
        }
    }

    void Patrol() {
        float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);

        // Change direction in intervals
        if (lastDirectionChange >= CHANGE_DIRECTION_DELAY) {
            float randDirection = Random.Range(0, 360);
            Quaternion newRotation = Quaternion.AngleAxis(randDirection, Vector3.right);
            Vector3 direction = newRotation * Vector3.forward;
            //Debug.Log ("Switching directions:" + direction);
            transform.rotation = Quaternion.LookRotation(transform.forward, -new Vector2(direction.y, direction.z));

            moveDirection = new Vector2(direction.normalized.y, direction.normalized.z);
            lastDirectionChange = 0;
        }

        // Once player is within sight of enemy, start chasing player
        if (distanceFromPlayer < 10) {
            this.state = EnemyState.CHASE;
        }

        lastDirectionChange += Time.deltaTime;
    }

    void Chase() {
        float distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);

        // Point enemy towards player
        Vector3 direction = this.transform.position - player.transform.position;
        this.transform.rotation = Quaternion.LookRotation(transform.forward, -direction.normalized);

        // As long as enemy is 8 distance away from player,
        // don't fire, just keep chasing. Once enemy is within range, stop chasing and fire at player.
        if (distanceFromPlayer > 8) {
            moveDirection = new Vector2(direction.normalized.x, direction.normalized.y);
            firing = false;
        }
        else {
            firing = true;
        }

        // If player gets more than 10 distance away from the enemy, switch to patrol state.
        if (distanceFromPlayer > 10) {
            this.state = EnemyState.PATROL;
        }

    }

}
