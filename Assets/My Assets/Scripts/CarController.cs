using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : PlayerController {

	private float refocusCamAfter = 1.5f; // How long we wait after the user moves the camera to re-focus it on the road
	private float maxPanTime = 2f; // How long we take to do a full turn of the camera to focus (180 degrees off)

	private bool cameraFollowing = false;

	// override the movement function because Cars need to be controlled differently
	public override void movePlayer() {
		float moveX = Input.GetAxis("Horizontal");
		float moveZ = Input.GetAxis("Vertical");

		if (moveZ < 0) {
			moveX *= -1; // reverse the left-right movement if we're driving backwards 
		}

		// turn car based on left-right input
		float turnSpeed = Mathf.Min(speed / 15, 3);
		GetComponent<Rigidbody>().rotation = Quaternion.Euler(new Vector3(0f, GetComponent<Rigidbody>().rotation.eulerAngles.y + (moveX * turnSpeed), 0f));

		// move forward/back
		Vector3 movement = GetComponent<Rigidbody>().rotation * Quaternion.Euler(modelRotationOffset) * new Vector3(0f, 0f, moveZ * speed);
		GetComponent<Rigidbody>().velocity = movement;

		// to do : camera slowly rotates back to behind the car if not being moved by the user 
		float camDist = Camera.main.GetComponent<CameraController>().currentLookDistance(lookAfterSwap());
		if (camDist > 0.01 && Camera.main.GetComponent<CameraController>().untouchedIn(refocusCamAfter) && !Camera.main.GetComponent<CameraController>().panning) {
			Camera.main.GetComponent<CameraController>().point(lookAfterSwap(), !cameraFollowing, (camDist / 180) * maxPanTime);
			cameraFollowing = true;
		}

		if (cameraFollowing && !Camera.main.GetComponent<CameraController>().untouchedIn(refocusCamAfter)) {
			cameraFollowing = false;
		}
	}

	// Override the animation settings because cars have different animations
	// 		currently different = none
	public override void updateAnimation(float speed, bool ignore) {

	}

	public override Vector3 lookAfterSwap(GameObject previous = null) {
		Vector3 inFrontOfUs = transform.position + transform.rotation * Quaternion.Euler(modelRotationOffset) * Vector3.forward;
		inFrontOfUs.y = transform.position.y + 10;
		return inFrontOfUs;
	}
}
