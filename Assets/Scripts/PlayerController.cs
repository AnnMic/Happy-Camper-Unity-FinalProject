using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{

	private Rigidbody2D rigidBody;
	private float jumpSpeed = 20f;

	private bool onGround = true;

	private bool crached = false;
	// Use this for initialization
	void Start ()
	{
		rigidBody = GetComponent<Rigidbody2D> ();
	}

	// FixedUpdate should be used instead of Update when dealing with rigidbodies
	void FixedUpdate ()
	{
		if (crached) {
			return;
		}
		rigidBody.velocity = Vector2.ClampMagnitude (rigidBody.velocity, 30);

		rigidBody.rotation = Mathf.Clamp (rigidBody.rotation, -50, 50);

		if (rigidBody.velocity.x < 10) {
			rigidBody.AddForce (new Vector2 (1000, 1000));
		}

		//Checks for input if the player should jump
		if (onGround && (Input.GetButtonDown ("Jump") || Input.touchCount > 0)) {
			rigidBody.velocity += jumpSpeed * Vector2.up;
			onGround = false;
		}
	}

	//Checking if the player is touching the ground
	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Terrain") {
			onGround = true;
		}
		else if (coll.gameObject.tag == "Rock") {
			crached = true;
		}
	}

}