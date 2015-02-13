using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {
    public Scene game;
    public BackgroundTile backgroundTile;
    public GameObject wall;
    public GameObject enemy;
    public Player player;

    private int tileCountX;
    private int tileCountY;
    private float width;
    private float height;
    private float tileWidth;
    private float tileHeight;
    private BackgroundTile[,] mapGameObjects;
    private int[,] mapCoordinates;
    private int[,] spawnPoints;
    private int cellCount = 0;
    private ArrayList spawnWorldPositions;
    private ArrayList enemies;
    private ArrayList mapObjects;

    // Use this for initialization
    void Start() {
        game = FindObjectOfType<Scene>();

        width = this.collider2D.bounds.size.x;
        height = this.collider2D.bounds.size.y;
        tileWidth = backgroundTile.renderer.bounds.size.x;
        tileHeight = backgroundTile.renderer.bounds.size.y;
        tileCountX = (int)(width / tileWidth);
        tileCountY = (int)(height / tileHeight);
        enemies = new ArrayList();
        mapObjects = new ArrayList();
        spawnWorldPositions = new ArrayList();

        CreateMap();
        Debug.Log("Enemy count=" + game.enemyCount);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.G)) {
            //Application.LoadLevel("Main");

            Debug.Log("BeforeDestroyed Map");
            Debug.Log("mapObj count=" + mapObjects.Count);
            Debug.Log("spawnPositions=" + spawnWorldPositions.Count);
            DestroyMap();

            foreach (GameObject obj in mapObjects) {
                if (obj != null) {
                    Debug.Log("Destroying " + obj.name);
                    Destroy(obj);
                }
                else
                    Debug.Log("Trying to destroy null");
            }
            mapObjects.Clear();
            spawnWorldPositions.Clear();
            Debug.Log("Destroyed Map");
            Debug.Log("mapObj count=" + mapObjects.Count);
            CreateMap();
            Debug.Log("Map Created");
            Debug.Log("mapObj count=" + mapObjects.Count);
        }
    }

    void CreateMap() {
        cellCount = 0;
        mapGameObjects = new BackgroundTile[tileCountX, tileCountY];
        spawnPoints = new int[tileCountX, tileCountY];
        mapCoordinates = new int[tileCountX, tileCountY];

        for (int i = 0; i < tileCountX; i++) {
            for (int j = 0; j < tileCountY; j++) {
                mapGameObjects[i, j] = null;
                spawnPoints[i, j] = 0;
            }
        }

        while (cellCount < game.MapSize) {
            for (int i = 0; i < tileCountX; i++) {
                for (int j = 0; j < tileCountY; j++) {
                    mapGameObjects[i, j] = null;
                    spawnPoints[i, j] = 0;
                }
            }
            spawnWorldPositions.Clear();
            GenerateMap();
        }
            Debug.Log("Starting to render");
        for (int i = 0; i < tileCountX; i++) {
            for (int j = 0; j < tileCountY; j++) {
                // Draw walls
                if (mapCoordinates[i, j] == 0) {
                    mapObjects.Add(Instantiate(wall, new Vector3(i * tileWidth, j * tileHeight, 20), Quaternion.identity) as GameObject);
                }
                else if (spawnPoints[i, j] == 1) {
                    mapObjects.Add(Instantiate(enemy, new Vector3(i * tileWidth, j * tileHeight, 0), Quaternion.identity) as GameObject);
                }

            }
        }

        Debug.Log("Spawn positions " + spawnWorldPositions.Count);
        foreach (Vector3 spawnPoint in spawnWorldPositions) {
            game.enemyCount++;
        }

        player.transform.position = FindPlayerSpawnPosition();
    }

    void DestroyMap() {


    }

    void GenerateMap() {
        cellCount = 0;
        ArrayList activeList = new ArrayList();
        DoFirstGenerationStep(activeList);
        while (activeList.Count > 0) {
            StartCoroutine("DoNextGenerationStep", activeList);
        }
    }

    void DoFirstGenerationStep(ArrayList activeList) {
        activeList.Add(CreateCell(new Vector2((int)(tileCountX / 2), (int)(tileCountY / 2))));
    }

    IEnumerator DoNextGenerationStep(ArrayList activeList) {
        int currentIndex = activeList.Count - 1;
        BackgroundTile currentTile = (BackgroundTile)activeList[currentIndex];
        GeneratorDirections direction = CellDirection.getRandomDirection;
        Vector2 coordinates = currentTile.mapCoordinate + CellDirection.toVector(direction);
        if (ContainsCoordinates(coordinates) && mapGameObjects[(int)coordinates.x, (int)coordinates.y] == null) {
            activeList.Add(CreateCell(coordinates));
            cellCount++;
        }
        else {
            activeList.RemoveAt(currentIndex);

        }

        yield return new WaitForSeconds(1.0f);
    }

    BackgroundTile CreateCell(Vector2 coordinate) {
        Vector3 gameWorldPlacementPos = new Vector3(coordinate.x * tileWidth, coordinate.y * tileHeight, 20);
        BackgroundTile newTile = Instantiate(backgroundTile, gameWorldPlacementPos, Quaternion.identity) as BackgroundTile;
        mapObjects.Add(newTile.gameObject);
        newTile.mapCoordinate = new Vector2(coordinate.x, coordinate.y);

        mapGameObjects[(int)coordinate.x, (int)coordinate.y] = newTile;
        mapCoordinates[(int)coordinate.x, (int)coordinate.y] = 1;
        int spawnEnemy = Random.Range(0, 100);
        if (spawnEnemy < game.SpawnRate) {
            spawnPoints[(int)coordinate.x, (int)coordinate.y] = 1;
            spawnWorldPositions.Add(new Vector3((int)coordinate.x * tileWidth, (int)coordinate.y * tileHeight, 0));
        }

        return newTile;
    }

    public bool ContainsCoordinates(Vector2 coordinate) {
        return coordinate.x >= 0 && coordinate.x < tileCountX && coordinate.y >= 0 && coordinate.y < tileCountY;
    }

    public Vector3 FindPlayerSpawnPosition() {
        double furthestDistance = double.NegativeInfinity;
        int coordX = 0, coordY = 0;
        for (int i = 0; i < tileCountX; i++) {
            for (int j = 0; j < tileCountY; j++) {

                // Only spawn player where there is no enemy already spawned
                // Only spawn player where there is no wall either.
                if (spawnPoints[i, j] == 0 && mapCoordinates[i, j] != 0) {
                    Vector3 tileWorldPosition = new Vector3(i * tileWidth, j * tileHeight, 0);
                    // Have to look at each enemy that was spawned.
                    foreach (Vector3 pos in spawnWorldPositions) {
                        double distance = (tileWorldPosition - pos).magnitude;
                        if (distance > furthestDistance) {
                            furthestDistance = distance;
                            coordX = i;
                            coordY = j;
                        }
                    }
                }
            }
        }
        Debug.Log("Furthest Distance =" + furthestDistance + " at " + coordX + "," + coordY);
        return new Vector3(coordX * tileWidth, coordY * tileHeight, 0);
    }
}

// Helper classes for map generation

public enum GeneratorDirections {
    North, East, South, West
}

public class CellDirection {
    public const int Count = 4;
    public static Vector2[] directionVectors =  {
		new Vector2(0, 1),
		new Vector2(1, 0),
		new Vector2(0, -1),
		new Vector2(-1, 0)
	};

    public static GeneratorDirections getRandomDirection {
        get {
            int val = Random.Range(0, 160);
            if (val >= 0 && val <= 35) {
                return GeneratorDirections.West;
            }
            else if (val > 35 && val <= 65) {
                return GeneratorDirections.North;
            }
            else if (val > 65 && val <= 100) {
                return GeneratorDirections.South;
            }
            else {
                return GeneratorDirections.East;
            }
        }
    }

    public static Vector2 toVector(GeneratorDirections direction) {
        return directionVectors[(int)direction];
    }

}
