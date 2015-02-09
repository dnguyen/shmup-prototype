using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {
	public BackgroundTile backgroundTile;

	private Random random;
	private int tileCountX;
	private int tileCountY;
	private float width;
	private float height;
	private float tileWidth;
	private float tileHeight;
	private BackgroundTile[,] map;

	// Use this for initialization
	void Start () {
		width = this.collider2D.bounds.size.x;
		height = this.collider2D.bounds.size.y;
		tileWidth = backgroundTile.renderer.bounds.size.x;
		tileHeight = backgroundTile.renderer.bounds.size.y;
		tileCountX = (int)(width / tileWidth);
		tileCountY = (int)(height / tileHeight);
		map = new BackgroundTile[tileCountX,tileCountY];
		for (int i = 0; i < tileCountX; i++) {
			for (int j = 0; j < tileCountY; j++) {
				map[i,j] = null;
			}
		}

		GenerateMap();
//		for (float x = 0; x <= width; x+= tileWidth) {
//			for (float y = 0; y <= height; y += tileHeight) {
//				GameObject clone = Instantiate (backgroundTile, new Vector3(x, y, 20), Quaternion.identity) as GameObject;
//			}
//		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	void GenerateMap() {
		ArrayList activeList = new ArrayList();
		DoFirstGenerationStep(activeList);
		while (activeList.Count > 0) {
			DoNextGenerationStep(activeList);
		}
	}

	void DoFirstGenerationStep(ArrayList activeList) {
		activeList.Add (CreateCell (new Vector2((int)(tileCountX / 2), (int)(tileCountY / 2))));
	}

	void DoNextGenerationStep(ArrayList activeList) {
		int currentIndex = activeList.Count - 1;
		BackgroundTile currentTile = (BackgroundTile)activeList[currentIndex];
		GeneratorDirections direction = CellDirection.getRandomDirection;
		Vector2 coordinates = currentTile.mapCoordinate + CellDirection.toVector (direction);
		if (ContainsCoordinates (coordinates) && map[(int)coordinates.x, (int)coordinates.y] == null) {
			activeList.Add (CreateCell(coordinates));

		} else {
			activeList.RemoveAt (currentIndex);
		}
	}

	BackgroundTile CreateCell(Vector2 coordinate) {

		Debug.Log ("Creating cell at: " + coordinate);
		Vector3 gameWorldPlacementPos = new Vector3(coordinate.x * tileWidth, coordinate.y * tileHeight, 20);
		BackgroundTile newTile = Instantiate (backgroundTile, gameWorldPlacementPos, Quaternion.identity) as BackgroundTile;
		newTile.mapCoordinate = new Vector2(coordinate.x, coordinate.y);
		map[(int)coordinate.x, (int)coordinate.y] = newTile;

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
