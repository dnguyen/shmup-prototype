using UnityEngine;
using System.Collections;

public class Scene : MonoBehaviour {

	public GameObject mapTile;
	public GameObject enemy;

	private int minXBound = 0, maxXBound = 100, minYBound = 0, maxYBound = 100;

	private float lastSpawn = 0;
	private int enemyCount = 0;
	private float ENEMY_SPAWN_RATE = 0.5f;
	private int MAX_ENEMIES = 50;
	private int MAP_HEIGHT = 8;
	private int MAP_WIDTH = 8;

	// Use this for initialization
	void Start () {
		Screen.showCursor = true;
		//InvokeRepeating ("Spawn", ENEMY_SPAWN_RATE, ENEMY_SPAWN_RATE);
	}
	
	// Update is called once per frame
	void Update () {
	}

	void Spawn() {
		if (enemyCount < MAX_ENEMIES) {
			int x = Random.Range (minXBound, maxXBound);
			int y = Random.Range (minYBound, maxYBound);
			Instantiate(enemy, new Vector3(x, y, 0), Quaternion.identity);
			enemyCount++;
		}
	}
}
