using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {


    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.name != "Background") {
            if (!other.name.Contains("boundary")) {
                //Debug.Log (this.tag + " colliding with " + other.tag);
            }
            Destroy(this.gameObject);
        }
        if (other.name == "Player") {
            var player = other.GetComponent<Player>();
            player.health--;
        }

        if (this.tag == "player_bullet" && other.tag == "enemy") {
            Scene game = FindObjectOfType<Scene>();
            game.KillEnemy();
            Destroy(other.gameObject);
        }
    }
}
