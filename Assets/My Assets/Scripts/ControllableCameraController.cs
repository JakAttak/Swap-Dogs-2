using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControllableCameraController : PlayerController {	
	new void Start() {
		base.Start ();

		actionable = true;
		speed = 0f; // Cameras can't be moved
		cameraDistance = Vector3.zero; // the camera goes right on this object so that it sees from it's viewpoint
		cameraMaxTilt = new Vector2(-75f, 45f); // The camera can be rotated more than a normal player
		interactionRange = Mathf.Max(interactionRange, 30f); // We can interact with these from very far away
		playerNearbyMessage = "This is a camera\nUse it to expand your viewpoints\n(c) Take over";
	}
	
	// Update
	new void Update () {
		if (gameObject.tag == "Player") {
			scanAndInteract(); // We can still interact with other objects
			transform.rotation = Quaternion.Euler(new Vector3(0, Camera.main.transform.rotation.eulerAngles.y + 90, 0)); // poin t in the direction we're looking
		} else { 
			GetComponent<ObjectController>().Update();
		}
	}

	// Override PlayerController FixedUpdate, because we don't want our cameras to be doing that
	new void FixedUpdate() {

	}
	
	new void handleLocking() {
		// Cameras can't be moved
		GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, 0f);
		GetComponent<Rigidbody>().freezeRotation = true;
		GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
	}
	
	// Special functions for switching to and becoming the new player because the camera selectively hides it's model
	new public void swapWithObject(GameObject obj) {
		foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
			r.enabled = true;
		}
		
		base.swapWithObject(obj);
	}
	
	new public void becomePlayer() {
		base.becomePlayer();

		foreach (Renderer r in GetComponentsInChildren<Renderer>()) {
			r.enabled = false;
		}
	}
}

