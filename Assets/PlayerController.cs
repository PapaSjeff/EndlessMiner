using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PhysicsObject {

	public float jumpPower = 7f;
	public float maxSpeed = 7f;
	public float wallSlideGravity = .8f;
	private bool isOnWall = false;
	
	// Use this for initialization
	

	private SpriteRenderer mySpriteRenderer;
	private Animator myAnimator;
	private BoxCollider2D myTriggerBox;
	void Awake () 
	{
		mySpriteRenderer = GetComponent<SpriteRenderer>();
		myAnimator = GetComponent<Animator>();
		myTriggerBox = GetComponent<BoxCollider2D>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Solid")
		{
			Debug.Log(other.tag);
			isOnWall = true;
			velocity.y = 0;
			velocity.x = 0;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Solid")
		{
			isOnWall = false;
		}
	}
	
	protected override void ComputeVelocity()
	{
		

		Vector2 move = Vector2.zero;

		//move.x = Input.GetAxis("Horizontal");
		move.x = 1f;


		if (Input.GetButtonDown("Jump") && isGrounded)
		{
			velocity.y = jumpPower;
		}
		else if (Input.GetButtonUp("Jump"))
		{
			if (velocity.y > 0f)
			{
				velocity.y = velocity.y * .5f;
			}
		}

		//Wall Slide step
		if (isOnWall)
		{
			velocity.y = 0f;
			velocity.x = 0f;
			gravityModifier = wallSlideGravity;
		}
		else
		{
			gravityModifier = 1f;
		}

		if (velocity.x > 0.01f && velocity.x != 0f)
		{
			mySpriteRenderer.flipX = false;
		}
		else if (velocity.x < 0.01f && velocity.x != 0f)
		{
			mySpriteRenderer.flipX = true;
		}
		// bool flipSprite = (mySpriteRenderer.flipX ? (move.x > 0.01f) : (move.x < 0.01f));
		// if (flipSprite)
		// {
		// 	mySpriteRenderer.flipX = !mySpriteRenderer.flipX;
		// }

		myAnimator.SetBool("grounded", isGrounded);
		myAnimator.SetFloat("velocityX", Mathf.Abs (velocity.x) / maxSpeed);

		targetVelocity = move * maxSpeed;

		isOnWall = false;
	}
}
