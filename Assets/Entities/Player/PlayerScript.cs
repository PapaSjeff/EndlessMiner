using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]

public class PlayerScript : MonoBehaviour
{
	public Vector2 wallJumpClimb = new Vector2(4, 5);
	public Vector2 wallJumpOff = new Vector2(4, 2);
	public Vector2 wallLeap = new Vector2(8, 8);

	public float moveSpeed = 6f;
	public float velocityXSmoothing;
    float gravity = -20f;

	public float accelerationTimeGrounded = .2f;
	public float accelerationTimeAir = .1f;

	public float jumpPower = 8f;

	public float wallSlideSpeedMax = 3f;
	public float wallStickTime = .25f;
	public float timeToWallUnstick;
    Vector2 velocity;
    Controller2D controller;

	public int direction = 1;

    void Awake()
    {
        controller = GetComponent<Controller2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		if (Input.GetKeyDown(KeyCode.A))
		{
			direction = -1;
		}
		else if (Input.GetKeyDown(KeyCode.D))
		{
			direction = 1;
		}

		 //Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
		 Vector2 input = new Vector2(direction, transform.position.y);
		 int wallDirectionX = (controller.collisions.left)?-1:1;

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAir);

		bool wallSliding = false;
		if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
		{
			wallSliding = true;
			
			if (velocity.y < -wallSlideSpeedMax)
			{
				velocity.y = -wallSlideSpeedMax;
			}

			if (timeToWallUnstick > 0)
			{
				velocityXSmoothing = 0;
				velocity.x = 0;

				if (input.x != wallDirectionX && input.x != 0)
				{
					timeToWallUnstick -= Time.deltaTime;
				}
				else
				{
					timeToWallUnstick = wallStickTime;
				}
			}
			else
			{
				timeToWallUnstick = wallStickTime;
			}
		}

		if (controller.collisions.above || controller.collisions.below)
		{
			velocity.y = 0f;
		}


		if (Input.GetKeyDown(KeyCode.Space))
		{
			if (wallSliding)
			{
				if (wallDirectionX == input.x)
				{
					velocity.x = -wallDirectionX * wallJumpClimb.x;
					velocity.y = wallJumpClimb.y;
				}
				else if (input.x == 0)
				{
					velocity.x = -wallDirectionX * wallJumpOff.x;
					velocity.y = wallJumpOff.y;
				}
				else
				{
					velocity.x = -wallDirectionX * wallLeap.x;
					velocity.y = wallLeap.y;
				}
			}

			if (controller.collisions.below)
			{
				velocity.y = jumpPower;
			}
		}

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
