using UnityEngine;
using System.Collections.Generic;

public class TerrainMesh : MonoBehaviour
{

	// Reference to the mesh we will generate
	private Mesh mesh = null;

	// The terrain points along the top of the mesh
	private Vector3[] points = null;

	// Mutable lists for all the vertices and triangles of the mesh
	private List<Vector3> vertices = new List<Vector3> ();
	private List<int> triangles = new List<int> ();
	private List<Vector2> textureCoords = new List<Vector2> ();
	private List<Vector2> borderPoints = new List<Vector2> ();

	private float textureSize = 1024;

	public Vector3 endPoint; 


	void Start ()
	{

	}

	public void GenerateTerrain ()
	{
		// Get a reference to the mesh component and clear it
		MeshFilter filter = GetComponent<MeshFilter> ();
		mesh = filter.mesh;

		mesh.Clear ();

		// Generate 4 random points for the top 
		points = new Vector3[5];
		for (int i = 0; i < points.Length; i++) {            
			points [i] = new Vector3 (10f * (float)i, Random.Range (5f, 10f), 0f);
		}

		CreateCurve ();

		//Set the points for the edge collider
		EdgeCollider2D edgeCollider = GetComponent<EdgeCollider2D> ();
		edgeCollider.Reset ();
		edgeCollider.points = borderPoints.ToArray ();

		endPoint = borderPoints[borderPoints.Count - 1];

		// Assign the vertices and triangles to the mesh
		mesh.vertices = vertices.ToArray ();
		mesh.uv = textureCoords.ToArray ();
		mesh.triangles = triangles.ToArray ();

		mesh.RecalculateBounds ();
		mesh.RecalculateNormals ();
	
	}

	void CreateCurve ()
	{
		Vector3 keyPoint0 = Vector3.zero;
		Vector3 keyPoint1 = Vector3.zero;
		Vector3 newPoint = Vector3.zero;

		//The width of a segment
		float terrainSegmentWidth = 2f;

		for (int i = 1; i < points.Length; i++) {
			keyPoint0 = points [i - 1];
			keyPoint1 = points [i];

			//How many segments will we divide the distance between the points into
			int totalSegments = Mathf.CeilToInt ((keyPoint1.x - keyPoint0.x) / terrainSegmentWidth);
			//Calculate a more accurate width (could be 1.8 instead of 2)
			float segmentWidth = (keyPoint1.x - keyPoint0.x) / totalSegments;

			float deltaAngle = Mathf.PI / totalSegments;
			float ymid = (keyPoint0.y + keyPoint1.y) / 2;
			float amplitude = (keyPoint0.y - keyPoint1.y) / 2;

			for (int segment = 0; segment <= totalSegments; segment++) {
				newPoint.x = keyPoint0.x + segment * segmentWidth;
				newPoint.y = ymid + amplitude * Mathf.Cos (deltaAngle * segment);

				CreateMeshForPoint (newPoint);
			}
		}
	}

	void CreateMeshForPoint (Vector3 point)
	{
		borderPoints.Add (new Vector2 (point.x, point.y));

		// Create a corresponding point along the bottom
		vertices.Add (new Vector3 (point.x, 0f, 0f));
		textureCoords.Add (new Vector2 (point.x / textureSize, 0));
		// Then add our top point
		vertices.Add (point);
		textureCoords.Add (new Vector2 (point.x / textureSize, 1));

		if (vertices.Count >= 4) {
			// We have completed a new quad, create 2 triangles
			int start = vertices.Count - 4;
			triangles.Add (start + 0);
			triangles.Add (start + 1);
			triangles.Add (start + 2);
			triangles.Add (start + 1);
			triangles.Add (start + 3);
			triangles.Add (start + 2);    
		}
	}
}