using UnityEngine;
using System.Collections;

public class EnemyDeath : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Rigidbody2D[] children = this.GetComponentsInChildren<Rigidbody2D>();
        Debug.Log ("Children rigidbodies:" + children.Length);
        foreach (Rigidbody2D rigidbody in children) {
            rigidbody.AddForce (new Vector2(Random.Range (-1,1), Random.Range (-1,1)).normalized * 55.0f);
            rigidbody.rotation = Random.Range (0,360);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
