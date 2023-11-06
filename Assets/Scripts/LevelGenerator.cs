using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

	// using this to begin tracking your floor and walls, along with create holes
	public enum BlockType {
		FLOOR,
		WALL,
		HOLE
	}

	public GameObject floorPrefab;
	public GameObject wallPrefab;
	public GameObject ceilingPrefab;

	public GameObject characterController;

	public GameObject floorParent;
	public GameObject wallsParent;

	// allows us to see the maze generation from the scene view
	public bool generateRoof = true;

	// number of times we want to "dig" in our maze
	public int tilesToRemove = 50;

	public int mazeSize;

	// added to limit the number of holes to 3
	public int numberOfHoles = 3;

	// spawns at the end of the maze generation
	public GameObject pickup;

	// this will determine whether we've placed the character controller
	private bool characterPlaced = false;

	// 2D array representing the map
	//private bool[,] mapData;
	//changed from bool to BlockType to account for the third option of holes.
	private BlockType[,] mapData;

	// we use these to dig through our maze and to spawn the pickup at the end
	private int mazeX = 4, mazeY = 1;

	// Use this for initialization
	void Start () {

		// initialize map 2D array
		mapData = GenerateMazeData();

		// create actual maze blocks from maze boolean data
		for (int z = 0; z < mazeSize; z++) {
			for (int x = 0; x < mazeSize; x++) {
				//had to change to incorporate the BlockType
				if (mapData[z, x] == BlockType.WALL) {
					CreateChildPrefab(wallPrefab, wallsParent, x, 1, z);
					CreateChildPrefab(wallPrefab, wallsParent, x, 2, z);
					CreateChildPrefab(wallPrefab, wallsParent, x, 3, z);
					//had to change to incorporate the BlockType
				} else if (mapData[z, x] == BlockType.FLOOR && !characterPlaced) {
					
					// place the character controller on the first empty wall we generate
					characterController.transform.SetPositionAndRotation(
						new Vector3(x, 1, z), Quaternion.identity
					);

					// flag as placed so we never consider placing again
					characterPlaced = true;
				}

				// create floor and holes
				if (mapData[z, x] != BlockType.HOLE) {
					CreateChildPrefab(floorPrefab, floorParent, x, 0, z);
				} else {
					// creates a hole that's -6 y deep to give a look of an actual hole
					CreateChildPrefab(floorPrefab, floorParent, x, -10, z);
				}

				// creates the ceiling
				if (generateRoof) {
					CreateChildPrefab(ceilingPrefab, wallsParent, x, 4, z);
				}
			}
		}

		// spawn the pickup at the end
		var myPickup = Instantiate(pickup, new Vector3(mazeX, 1, mazeY), Quaternion.identity);
		myPickup.transform.localScale = new Vector3(0.25f, 0.25f, 0.25f);
	}

	// generates the booleans determining the maze, which will be used to construct the cubes
	// actually making up the maze
	//had to change bool into BlockType
	BlockType[,] GenerateMazeData() {
		BlockType[,] data = new BlockType[mazeSize, mazeSize];

		// initialize all walls to true
		for (int y = 0; y < mazeSize; y++) {
			for (int x = 0; x < mazeSize; x++) {
				// had to change from true to wall because we are no longer opporating as a bool model
				data[y, x] = BlockType.WALL;
				//data[y, x] = true;
			}
		}

		// counter to ensure we consume a minimum number of tiles
		int tilesConsumed = 0;

		// iterate our random crawler, clearing out walls and straying from edges
		while (tilesConsumed < tilesToRemove) {
			
			// directions we will be moving along each axis; one must always be 0
			// to avoid diagonal lines
			int xDirection = 0, yDirection = 0;

			if (Random.value < 0.5) {
				xDirection = Random.value < 0.5 ? 1 : -1;
			} else {
				yDirection = Random.value < 0.5 ? 1 : -1;
			}

			// random number of spaces to move in this line
			int numSpacesMove = (int)(Random.Range(1, mazeSize - 1));

			// move the number of spaces we just calculated, clearing tiles along the way
			for (int i = 0; i < numSpacesMove; i++) {
				mazeX = Mathf.Clamp(mazeX + xDirection, 1, mazeSize - 2);
				mazeY = Mathf.Clamp(mazeY + yDirection, 1, mazeSize - 2);

				// needed to incorporate blocktype here as well since no longer bool and needed to be more specific
				if (data[mazeY, mazeX] == BlockType.WALL) {
					data[mazeY, mazeX] = BlockType.FLOOR;
					tilesConsumed++;
				}
			}
		}

		//For generating holes
		//while holes is > 0, which is assigned near the top as 3,
		while (numberOfHoles > 0) {
			//we get a random x and y within the mazeSize
			int ranX = Random.Range(0, mazeSize);
			int ranY = Random.Range(0, mazeSize);
			// and then take the floor of that x and y coordinate and not a wall
			if (data[ranY, ranX] == BlockType.FLOOR) {
				//and change that value from Floor to Hole.
				data[ranY, ranX] = BlockType.HOLE;
				//lastly we remove one hole from our numberOfHoles variable.
				numberOfHoles--;
			}
		}

		return data;
	}

	// allow us to instantiate something and immediately make it the child of this game object's
	// transform, so we can containerize everything. also allows us to avoid writing Quaternion.
	// identity all over the place, since we never spawn anything with rotation
	void CreateChildPrefab(GameObject prefab, GameObject parent, int x, int y, int z) {
		var myPrefab = Instantiate(prefab, new Vector3(x, y, z), Quaternion.identity);
		myPrefab.transform.parent = parent.transform;
	}
}
