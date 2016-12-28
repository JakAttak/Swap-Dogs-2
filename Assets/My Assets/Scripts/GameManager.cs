using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
	public bool alreadySwappedThisRound = false; // used for making sure you don't swap back immediately

	private ObjectController[] objects;

	void Start () {
		objects = getInteractableObjects();
	}

	void Update () {
		// helper used by PlayerController
		alreadySwappedThisRound = false;
	}

	// Helper function that gets and stores every object that has an objectcontroller script
	private ObjectController[] getInteractableObjects() {
		return (ObjectController[]) FindObjectsOfType(typeof(ObjectController));
	}

	// Helper function that returns the objects array
	public ObjectController[] getObjects() {
		return objects;
	}
}
