using UnityEngine;
using System.Collections;

public class Scene : MonoBehaviour {

    public int currentLevel = 0;
    public int currentSpawnRate;
    private int START_SPAWN_RATE = 5;
    private float SPAWN_RATE_GROWTH = 0.2f;

    public int SpawnRate {
        get {
            return currentSpawnRate + (int)(currentSpawnRate * SPAWN_RATE_GROWTH);
        }
    }

	void Start () {
		Screen.showCursor = true;
        currentSpawnRate = START_SPAWN_RATE;
	}

	void Update () {
	}

    void OnGUI() {

    }

}

