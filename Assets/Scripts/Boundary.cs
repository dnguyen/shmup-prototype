using UnityEngine;
using System.Collections;

public class Boundary : MonoBehaviour {
	public GameObject wall;

	// Use this for initialization
	void Start () {
		float wallHeight = wall.renderer.bounds.size.y;
		float wallWidth = wall.renderer.bounds.size.x;

		if (this.name == "TopBoundary") {
			float start = transform.position.x - transform.lossyScale.x / 2;
			GameObject clone = Instantiate (wall, new Vector3 (start, transform.position.y, 0), Quaternion.identity) as GameObject;
			for (float x = start; x <= collider2D.bounds.size.x; x+= wallWidth) {
				clone = Instantiate (wall, new Vector3 (x, transform.position.y, 0), Quaternion.identity) as GameObject;
			}
		} else if (this.name == "BottomBoundary") {
			float start = 0;
			GameObject clone = Instantiate (wall, new Vector3 (start, transform.position.y, 0), Quaternion.identity) as GameObject;
			for (float x = start; x <= collider2D.bounds.size.x; x+= wallWidth) {
				clone = Instantiate (wall, new Vector3 (x, transform.position.y, 0), Quaternion.identity) as GameObject;
			}
		} else if (this.name == "LeftBoundary") {
			float start = transform.position.y - transform.lossyScale.y / 2;
			GameObject clone = Instantiate (wall, new Vector3 (this.transform.position.x, start, 0), Quaternion.identity) as GameObject;
			for (float y = start; y <= transform.lossyScale.y; y += wallHeight) {
				clone = Instantiate (wall, new Vector3(this.transform.position.x, y, 0), Quaternion.identity) as GameObject;
			}
		} else if (this.name == "RightBoundary") {
			float start = transform.position.y - transform.lossyScale.y / 2;
			GameObject clone = Instantiate (wall, new Vector3 (transform.position.x, start, 0), Quaternion.identity) as GameObject;
			for (float y = start; y <= transform.lossyScale.y; y += wallHeight) {
				clone = Instantiate (wall, new Vector3(this.transform.position.x, y, 0), Quaternion.identity) as GameObject;
			}
		}


	}
	
	// Update is called once per frame
	void Update () {

	}
}
