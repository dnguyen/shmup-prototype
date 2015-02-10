using UnityEngine;
using System.Collections;

public class Map : MonoBehaviour {
    public Scene game;
	public BackgroundTile backgroundTile;
	public GameObject wall;
	public Enemy enemy;

	private int tileCountX;
	private int tileCountY;
	private float width;
	private float height;
	private float tileWidth;
	private float tileHeight;
	private BackgroundTile[,] map;
	private int[,] testMap;
	private int[,] spawnPoints;
	private int cellCount = 0;

	// Use this for initialization
	void Start () {
        game = FindObjectOfType<Scene>();
        
		width = this.collider2D.bounds.size.x;
		height = this.collider2D.bounds.size.y;
		tileWidth = backgroundTile.renderer.bounds.size.x;
		tileHeight = backgroundTile.renderer.bounds.size.y;
		tileCountX = (int)(width / tileWidth);
		tileCountY = (int)(height / tileHeight);
		map = new BackgroundTile[tileCountX,tileCountY];
		spawnPoints = new int[tileCountX, tileCountY];
		testMap = new int[tileCountX,tileCountY];
		for (int i = 0; i < tileCountX; i++) {
			for (int j = 0; j < tileCountY; j++) {
				map[i,j] = null;
				spawnPoints[i,j] = 0;
			}
		}

		while (cellCount < 60) {
			DestroyMap ();
			GenerateMap();
			Debug.Log ("mapsize: " + tileCountX + "," + tileCountY);
			Debug.Log ("cellCount=" + cellCount);
		}
		Debug.Log ("Enemy size: " + enemy.renderer.bounds.size.x + "," + enemy.renderer.bounds.size.y);
		string log = "";
		// temp fix for walls not rendering in correct position.
		DestroyMap ();
		for (int i = 0; i < tileCountX; i++) {
			for (int j = 0; j < tileCountY; j++) {
				log += testMap[i,j];
			}
			log += "\n";
		}
		Debug.Log (log);

	
		for (int i = 0; i < tileCountX; i++) {
			for (int j = 0; j < tileCountY; j++) {
				if (testMap[i,j] == 1 && map[i,j] != null) {
					Vector3 gameWorldPlacementPos = new Vector3(map[i,j].mapCoordinate.x * tileWidth, map[i,j].mapCoordinate.y * tileHeight, 20);
					BackgroundTile newTile = Instantiate (backgroundTile, gameWorldPlacementPos, Quaternion.identity) as BackgroundTile;
				} else if (testMap[i,j] == 0) {
					Instantiate (wall, new Vector3(i * tileWidth, j * tileHeight, 20), Quaternion.identity);
				} else if (spawnPoints[i,j] != 0) {
					Instantiate (enemy, new Vector3(i * tileWidth, j * tileHeight, 0), Quaternion.identity);
				}

			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.G)) {
			Application.LoadLevel ("Main");
		}
	}

	void DestroyMap() {
		for (int i = 0; i < tileCountX; i++) {
			for (int j = 0; j < tileCountY; j++) {
				Destroy (map[i,j]);
				map[i,j] = null;
			}
		}
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
		activeList.Add (CreateCell (new Vector2((int)(tileCountX / 2), (int)(tileCountY / 2))));
	}

	IEnumerator DoNextGenerationStep(ArrayList activeList) {
		int currentIndex = activeList.Count - 1;
		BackgroundTile currentTile = (BackgroundTile)activeList[currentIndex];
		GeneratorDirections direction = CellDirection.getRandomDirection;
		Vector2 coordinates = currentTile.mapCoordinate + CellDirection.toVector (direction);
		if (ContainsCoordinates (coordinates) && map[(int)coordinates.x, (int)coordinates.y] == null) {
			activeList.Add (CreateCell(coordinates));
			cellCount++;
		} else {
			activeList.RemoveAt (currentIndex);

		}

		yield return new WaitForSeconds(1.0f);
	}

	BackgroundTile CreateCell(Vector2 coordinate) {

		Debug.Log ("Creating cell at: " + coordinate);
		Vector3 gameWorldPlacementPos = new Vector3(coordinate.x * tileWidth, coordinate.y * tileHeight, 20);
		BackgroundTile newTile = Instantiate (backgroundTile, gameWorldPlacementPos, Quaternion.identity) as BackgroundTile;
		newTile.mapCoordinate = new Vector2(coordinate.x, coordinate.y);

		map[(int)coordinate.x, (int)coordinate.y] = newTile;
		testMap[(int)coordinate.x, (int)coordinate.y] = 1;
		int spawnEnemy = Random.Range (0, 100);
		if (spawnEnemy < game.SpawnRate) {
			spawnPoints[(int)coordinate.x, (int)coordinate.y] = 1;
		}

		return newTile;
	}
	
	public bool ContainsCoordinates(Vector2 coordinate)
	{
		return coordinate.x >= 0 && coordinate.x < tileCountX && coordinate.y >= 0 && coordinate.y < tileCountY;
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
			if (val >= 0 && val <= 35)
			{
				return GeneratorDirections.West;
			}
			else if (val > 35 && val <= 65)
			{
				return GeneratorDirections.North;
			}
			else if (val > 65 && val <= 100)
			{
				return GeneratorDirections.South;
			}
			else
			{
				return GeneratorDirections.East;
			}
		}
	}

	public static Vector2 toVector(GeneratorDirections direction) {
		return directionVectors[(int)direction];
	}

}
