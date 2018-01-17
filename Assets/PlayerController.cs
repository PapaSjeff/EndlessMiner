using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour 
{

	[SerializeField] private float playerSpeed = 50f;
	[SerializeField] private float jumpPower = 25f;

	private Rigidbody2D myRigidbody;

	private float ySpeed;

	void Awake()
	{
		myRigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			ySpeed += jumpPower;
		}
		else
		{
			ySpeed = 0;
		}

		myRigidbody.velocity = new Vector2(playerSpeed * Time.deltaTime, ySpeed);
	}
}
