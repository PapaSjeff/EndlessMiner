using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Transform player;
	public PlayerScript playerScript;
	private int direction;
	public float cameraHeight;
	public float xSightAmount;
	private float xSight;

	void Start () {
		direction = playerScript.direction;
		xSight = xSightAmount;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (direction != playerScript.direction)
		{
			xSight = Mathf.Lerp(xSightAmount * direction, xSightAmount * playerScript.direction, .5f);
			direction = playerScript.direction;
		}
		
		transform.position = new Vector3 (player.position.x + (xSight * direction), player.position.y + cameraHeight, -10);
	}
}
