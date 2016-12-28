using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : PlayerController {

	// override the movement function because Cars need to be controlled differently
	public override void movePlayer() {
		float moveX = Input.GetAxis ("Horizontal");
		float moveZ = Input.GetAxis ("Vertical");

		// turn car based on left-right input
		GetComponent<Rigidbody>().rotation = Quaternion.Euler(new Vector3(0f, GetComponent<Rigidbody>().rotation.eulerAngles.y + moveX, 0f));

		// move forward/back
		Vector3 movement = GetComponent<Rigidbody>().rotation * Quaternion.Euler(modelRotationOffset) * new Vector3(0f, 0f, moveZ * speed);
		GetComponent<Rigidbody>().velocity = movement;

		// to do : camera slowly rotates back to behind the car if not being moved
	}

	public override void updateAnimation(float speed, bool ignore) {

	}

	public override Vector3 lookAfterSwap(GameObject previous) {
		return previous.transform.position;
	}
}
