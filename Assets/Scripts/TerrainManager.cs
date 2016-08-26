using UnityEngine;
using System.Collections;

public class TerrainManager : MonoBehaviour
{

	private int currentTerrainIndex = -1;
	private int previousTerrainIndex = -1;
	public TerrainMesh[] terrainSegments = new TerrainMesh[2];
	//holds the two terrains

	private Transform cameraTransform;
	private float camWidth = 0;
	private float camHeight = 0;

	void Awake(){
		Application.targetFrameRate = 300;
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
	}

	// Use this for initialization
	void Start ()
	{
		Camera camera = Camera.main;
		cameraTransform = camera.transform;

		camHeight = 2f * camera.orthographicSize;
		camWidth = camHeight * camera.aspect;

		currentTerrainIndex = 0;
		previousTerrainIndex = 1;
		TerrainMesh startTerrain = terrainSegments [currentTerrainIndex];
		startTerrain.GenerateTerrain (); //Generates the mesh for the terrain
	}
	
	// Update is called once per frame
	void Update ()
	{
		float cameraPosition = cameraTransform.position.x - camWidth / 2 - 10;

		if (cameraPosition > terrainSegments [previousTerrainIndex].endPoint.x) {
			previousTerrainIndex = currentTerrainIndex;

			currentTerrainIndex++;
			if (currentTerrainIndex > 1) {
				currentTerrainIndex = 0;
			}

			TerrainMesh terrainToRegenerate = terrainSegments [currentTerrainIndex];
			terrainToRegenerate.GenerateTerrain ();
		}
	}
}
