using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSidedLock : ObjectController {

	public Vector3 lockRotation = Vector3.zero;
	public bool locked = true;

	public override void Start() {
		base.Start();

		actionable = true;
	}

	// Returns what the default message should be if one isnt already set (overrides object setting)
	public override string defaultNearbyMessage() {
		return lockedMessage();
	}

	// returns a message based on if locked or not
	string lockedMessage() {
		if (locked) {
			return "This object is locked!\n(c) to unlock.";
		}

		return "This object is unlocked!\n(c) to lock.";
	}

	// Unlocks the lock
	void unlockMe() {
		locked = false;
		setNearbyMessage(lockedMessage());
		displayText(playerNearbyMessage);

		Camera.main.GetComponent<GameManager>().destroyObject(gameObject); // Right now we don't have any kind of unlock animation/behavior, so we just delete the object/door
	}

	// Locks the lock
	void lockMe() {
		locked = true;
		setNearbyMessage(lockedMessage());
		displayText(playerNearbyMessage);
	}

	// Interact
	public override void handleInteractions(GameObject cause) {
		if (Input.GetButtonUp("Swap")) {
			if (playerOnCorrectSide()) {
				if (locked) {
					unlockMe();
				} else {
					lockMe();
				}
			} else {
				displayText("You are on the wrong\nside to interact with the lock!");
			}
		}
	}

	bool playerOnCorrectSide() {
		Transform player = GameObject.FindWithTag("Player").transform;
		Vector3 dist = player.position - transform.position;
		dist.z = 0f;
		if (Vector3.Angle(Quaternion.Euler(lockRotation) * transform.forward, dist) < 90) {
			return true;
		}
		return false;
	}
}
