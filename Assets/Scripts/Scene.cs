using UnityEngine;
using System.Collections;

public class Scene : MonoBehaviour {

    private Player player;

    public int currentLevel = 0;
    public int currentSpawnRate;
    private int START_SPAWN_RATE = 5;
    private float SPAWN_RATE_GROWTH = 0.2f;
    public int enemyCount = 0;

    public int SpawnRate {
        get {
            return currentSpawnRate + (int)(currentLevel * SPAWN_RATE_GROWTH);
        }
    }

    void Start() {
        Screen.showCursor = true;
        player = FindObjectOfType<Player>();
        currentSpawnRate = START_SPAWN_RATE;
    }

    void Update() {
        checkEnemiesDead();
    }

    void OnGUI() {
        
        GUI.Box(new Rect(10, 10, 100, 25), "Ammo: " + player.currentBulletCount + " / " + player.MAX_BULLETS);
        GUI.Box(new Rect(120, 10, 125, 25), "Health: " + player.health + " / " + player.MAX_HEALTH);
    }

    // check if all enemies are dead.
    public void checkEnemiesDead() {
        if (enemyCount <= 0) {
        }
    }

}

