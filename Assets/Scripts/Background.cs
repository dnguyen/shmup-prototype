using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {
	public GameObject backgroundTile;

	// Use this for initialization
	void Start () {
		float width = this.collider2D.bounds.size.x;
		float height = this.collider2D.bounds.size.y;
		float tileWidth = backgroundTile.renderer.bounds.size.x;
		float tileHeight = backgroundTile.renderer.bounds.size.y;

		for (float x = 0; x <= width; x+= tileWidth) {
			for (float y = 0; y <= height; y += tileHeight) {
				GameObject clone = Instantiate (backgroundTile, new Vector3(x, y, 20), Quaternion.identity) as GameObject;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
