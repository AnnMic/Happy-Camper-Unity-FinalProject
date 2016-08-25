using UnityEngine;
using System.Collections;

public class TerrainGenerator : MonoBehaviour 
{

	public float deltaIncline = 0; 	//should the slope inclinde or decline, value above 0 will make incline, below decline

	private float minHeight = 5; //Min Height of the hill, used for deltaIncline 
	private float maxHeight = 0; //Max Height of the hill, used for deltaIncline 

	public float minDeltaX = 0; //The smallest width of a hill, bigger value will make th hills more spread out
	public float rangeDeltaX = Screen.width / 6; //The random range for the width

	public float minDeltaY = 0; //The smallest height of a hill, bigger value will make th hills more steap
	public float rangeDeltaY = Screen.width / 8; //The random range for the heigth

	private float amountOfKeyPoints = 5; //Amount of keypoints
	private int numberValuesToKeep = 1; //values to keep from last segment

	private Vector3[] terrainKeyPoints = new Vector3[5];

	private float y, x = 0f;
	private float dx, dy, newY = 0;
	private float sign = 1; //+1 is going up, -1 is going down


	public Vector3[] GenerateKeyPoints ()
	{
		ResetKeyPoints ();

		x = terrainKeyPoints [0].x;

		for (int i = numberValuesToKeep; i < amountOfKeyPoints; i++) {

			//Create a new x-position
			dx = Random.Range (minDeltaX, rangeDeltaX + minDeltaX);
			x += dx;

			//Create a new y-position
			dy = Random.Range (minDeltaY, rangeDeltaY + minDeltaY);
			newY = y + dy * sign;

			//Make sure new y-position is not outside of our bounds, add incline if it should go up/down
			newY = Mathf.Clamp (newY, minHeight, maxHeight) + deltaIncline;
			y = newY;

			if (deltaIncline != 0) {
				if (deltaIncline > 0) { //Going up hill
					maxHeight += deltaIncline;
				} else { // going down
					minHeight += deltaIncline;
				}
			}

			//defines it it should be a hill or a valley
			sign *= -1;

			terrainKeyPoints [i] = new Vector3 (x, y, 0);
		}
		return terrainKeyPoints;
	}

	//Keep the last x keypoints to the new segment
	private void ResetKeyPoints ()
	{

		for (int i = 0; i < numberValuesToKeep; i++) {
			terrainKeyPoints [i] = terrainKeyPoints [terrainKeyPoints.Length - numberValuesToKeep + i];
		}
	}
}
