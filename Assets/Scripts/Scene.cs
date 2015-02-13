using UnityEngine;
using System.Collections;

public class Scene : MonoBehaviour {

    private Player player;
    private Map map;

    public int currentLevel = 0;
    public int currentSpawnRate;
    public int currentMapSize;
    private int START_SPAWN_RATE = 7;
    
    private float SPAWN_RATE_GROWTH = 0.2f;
    public int enemyCount = 0;

    private bool isTransition = false;

    public int SpawnRate {
        
        get {
            
            return Mathf.Min(10, currentMapSize - (int)(currentMapSize * 0.1f));
        }
    }

    public int MapSize {
        get {
            var levelmodifier = (currentLevel+2) * 10;
            return Mathf.Min(30, levelmodifier);
        }
    }

    void Start() {
        Screen.showCursor = true;
        player = FindObjectOfType<Player>();
        map = FindObjectOfType<Map>();
        currentMapSize = MapSize;
        currentSpawnRate = SpawnRate;
    }

    void Update() {
        checkEnemiesDead();
    }

    void OnGUI() {
        
        GUI.Box(new Rect(10, 10, 100, 25), "Ammo: " + player.currentBulletCount + " / " + player.MAX_BULLETS);
        GUI.Box(new Rect(120, 10, 125, 25), "Health: " + player.health + " / " + player.MAX_HEALTH);

        if (isTransition) {
            GUI.Box(new Rect(Screen.width/2, Screen.height/2, 125, 25), "Generating level: " + currentLevel);
        }
    }

    // check if all enemies are dead.
    public void checkEnemiesDead() {
        if (enemyCount <= 0) {
            Debug.Log("All enemies dead. Generate new level");
            isTransition = true;
            player.enabled = false;
            player.transform.position = new Vector3(-100, 100, 0);
            currentLevel++;
            currentMapSize = MapSize;
            currentSpawnRate = SpawnRate;
            map.ResetMap();
            player.enabled = true;
            isTransition = false;
        }
    }

}

