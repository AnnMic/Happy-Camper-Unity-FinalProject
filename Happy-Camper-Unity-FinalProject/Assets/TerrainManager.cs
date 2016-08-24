using UnityEngine;
using System.Collections;

public class TerrainManager : MonoBehaviour
{

	private int currentTerrainIndex = -1;
	private int previousTerrainIndex = -1;
	public TerrainMesh[] terrainSegments = new TerrainMesh[2];
	//holds the two terrains

	private Transform cameraTransform;
	private Camera camera;
	private float camWidth = 0;
	private float camHeight = 0;

	// Use this for initialization
	void Start ()
	{
		cameraTransform = Camera.main.transform;
		camera = Camera.main;

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
