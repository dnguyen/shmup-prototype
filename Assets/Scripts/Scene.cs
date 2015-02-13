using UnityEngine;
using System.Collections;

public class Scene : MonoBehaviour {

    private Player player;
    private Map map;
    public GameObject enemyDeath;
    public GameObject healthPowerUp;

    public int currentLevel = 0;
    public int currentSpawnRate;
    public int currentMapSize;
    private int MIN_SPAWN_RATE = 10;
    private int MAX_MAP_SIZE = 75;
    public int enemyCount = 0;

    private bool isTransition = false;

    public int SpawnRate {
        
        get {
            var spawn = currentMapSize / 2 - (int)(currentMapSize * 0.1f);
            if (spawn < MIN_SPAWN_RATE) {
                return MIN_SPAWN_RATE;
            }
            else {
                return spawn;
            }
        }
    }

    public int MapSize {
        get {
            var levelmodifier = (currentLevel+2) * 10;
            if (levelmodifier > MAX_MAP_SIZE)
                return MAX_MAP_SIZE;
            else
                return levelmodifier;
        }
    }

    void Start() {

        PlayerPrefs.SetInt("completedLevels", currentLevel);
        Screen.showCursor = false;
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
        GUI.Box(new Rect(Screen.width - 110, 10, 100, 25), "Level: " + currentLevel);
        if (isTransition) {
            GUI.Box(new Rect(Screen.width/2 - 75, Screen.height/2 - 25, 250, 25), "Completed level. Generating level: " + currentLevel);
        }
    }

    public void KillEnemy(Vector3 enemyDeathPos) {
        enemyCount--;
        map.AddGameObject (Instantiate (enemyDeath, enemyDeathPos, Quaternion.identity) as GameObject);
        StartCoroutine("checkEnemiesDead");
    }

    public void SpawnHealth(Vector3 pos) {
        map.AddGameObject (Instantiate (healthPowerUp, pos, Quaternion.identity) as GameObject);
    }

    // check if all enemies are dead.
    public IEnumerator checkEnemiesDead() {
        Debug.Log(this.enemyCount);
        if (enemyCount <= 0) {
            Debug.Log("All enemies dead. Generate new level");
            yield return new WaitForSeconds(0.5f);
            isTransition = true;
            player.enabled = false;
            player.transform.position = new Vector3(-100, 100, -20);
            currentLevel++;
            currentMapSize = MapSize;
            currentSpawnRate = SpawnRate;
            yield return new WaitForSeconds(2);
            map.ResetMap();

            player.enabled = true;
            isTransition = false;
        }
    }

    public void HandleGameOver() {
        PlayerPrefs.SetInt("completedLevels", currentLevel);
        Application.LoadLevel("GameOver");
    }

}

