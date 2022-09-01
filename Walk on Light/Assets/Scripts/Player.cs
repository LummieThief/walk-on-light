using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
	Controller2D controller;

	float gravity;
	float jumpVelocity;
	Vector3 velocity;
	float VelocityXSmoothing;

	public float moveSpeed = 6f;
	public float accelerationTimeAirborn = 0.2f;
	public float accelerationTimeGrounded = 0.1f;
	public float jumpHeight = 4f;
	public float timeToJumpApex = 0.4f;


	private void Start()
	{
		controller = GetComponent<Controller2D>();
		gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
	}

	private void Update()
	{


		if (controller.collisions.above || controller.collisions.below)
		{
			velocity.y = 0;
		}

		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		if (Input.GetButtonDown("Jump") && controller.collisions.below)
		{
			velocity.y = jumpVelocity;
		}

		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref VelocityXSmoothing, (controller.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborn));
		velocity.y += gravity * Time.deltaTime;
		controller.Move(velocity * Time.deltaTime);
	}
}
