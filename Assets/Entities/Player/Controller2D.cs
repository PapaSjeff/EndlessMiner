using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (BoxCollider2D))]
public class Controller2D : MonoBehaviour {

	public LayerMask collisionMask;
	const float skinWidth = 0.030f;
	public int horizontalRayCount = 4;
	public int verticalRayCount = 4;

	public float maxClimbAngle = 80f;

	float horizontalRaySpacing;
	float verticalRaySpacing;

	BoxCollider2D myCollider;
	RaycastOrigins raycastOrigins;

	public CollisionInfo collisions;
	
	void Awake()
	{
		myCollider = GetComponent<BoxCollider2D>();
		collisions.faceDirection = 1;

		CalculateRaySpacing();
	}

	void Update()
	{
		//for (int i = 0; i < horizontalRayCount; i++)
		//{
		//	Debug.DrawRay(raycastOrigins.bottomLeft + Vector2.right * horizontalRaySpacing * i, Vector2.up * -2, Color.red);
		//}
	}

	public void Move(Vector2 velocity)
	{
		//Collision code
		UpdateRaycastOrigins();
		collisions.Reset();

		if (velocity.x != 0)
		{
			collisions.faceDirection = (int) Mathf.Sign(velocity.x);
			Debug.Log(collisions.faceDirection);
		}

		HorizontalCollision(ref velocity);
		
		if (velocity.y != 0)
		{
			VerticalCollision(ref velocity);
		}

		transform.Translate(velocity);
	}

	void HorizontalCollision(ref Vector2 velocity)
	{
		float directionX = collisions.faceDirection;
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;
		
		if (Mathf.Abs(velocity.x) < skinWidth)
		{
			rayLength = 2*skinWidth;
		}
		
		for (int i = 0; i < horizontalRayCount; i ++) {
			Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength,Color.red);

			if (hit) {
				velocity.x = (hit.distance - skinWidth) * directionX;
				rayLength = hit.distance;

				collisions.left = directionX == -1;
				collisions.right = directionX == 1;
			}
		}
	}

	void VerticalCollision(ref Vector2 velocity)
	{
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;

		for (int i = 0; i < verticalRayCount; i ++) {
			Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength,Color.red);

			if (hit) {
				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;

				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}
		}
	}

	// void ClimbSlope(ref Vector2 velocity, float slopeAngle)
	// {
	// 	float moveDistance = Mathf.Abs(velocity.x);
	// 	float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
		
	// 	//Check if the player is jumping on the slope or not
	// 	if (velocity.y <= climbVelocityY)
	// 	{
	// 		//Not jumping on slope
	// 		velocity.y = climbVelocityY;
	// 		velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
	// 		collisions.below = true;
	// 	}
	// }

	void UpdateRaycastOrigins()
	{
		Bounds bounds = myCollider.bounds;
		bounds.Expand(skinWidth * -2);

		raycastOrigins.bottomLeft 	= new Vector2(bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight 	= new Vector2(bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft 		= new Vector2(bounds.min.x, bounds.max.y);
		raycastOrigins.topRight 	= new Vector2(bounds.max.x, bounds.max.y);
	}

	void CalculateRaySpacing()
	{
		Bounds bounds = myCollider.bounds;
		bounds.Expand(skinWidth * -2);

		horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}
	
	struct RaycastOrigins
	{
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;
	}

	public struct CollisionInfo
	{
		public bool above, below;
		public bool left, right;
		public int faceDirection;
	
		public void Reset()
		{
			above = below = false;
			left = right = false;
		}
	}
}
