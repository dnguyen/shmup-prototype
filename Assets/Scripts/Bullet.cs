using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.name != "Background") {
			if (!other.name.Contains ("boundary")) {
				//Debug.Log ("bullet colliding with " + other.name);
			}
			Destroy (this.gameObject);
		}
		if (other.name == "Player") {
			var player = other.GetComponent<AbusePlayerController>();
			player.health--;
			Debug.Log ("Player health:" + player.health);
		}
	}
}
