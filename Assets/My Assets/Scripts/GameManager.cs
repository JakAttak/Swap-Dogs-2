using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : HelperFunctions {
	private ObjectController[] objects;

	private GameObject originalPlayer;
	private GameObject currentPlayer;

	public Image swapTimer;

	private float timeAwayFromBody = 0f;
	public float timeAllowedOutOfBody = 10f; // time you can be in another body (seconds)

	void Start() {
		refreshObjects();
	}

	void Update() {
		if (currentPlayer != null && currentPlayer != originalPlayer) {
			timeAwayFromBody += Time.deltaTime;
			float percent = 1 - (timeAwayFromBody / timeAllowedOutOfBody);
			swapTimer.fillAmount = percent;

			if (percent < 0) {
				currentPlayer.GetComponent<PlayerController>().swapAfterFrame(originalPlayer);
			}
		}
	}

	// Sets the current player
	public void setCurrentPlayer(GameObject obj) {
		if (!originalPlayer) {
			originalPlayer = obj;
		}
			
		currentPlayer = obj;

		if (currentPlayer == originalPlayer) {
			timeAwayFromBody = 0f;
			swapTimer.fillAmount = 1f;
		}
	}

	// Refreshes the object list
	private void refreshObjects() {
		objects = getInteractableObjects();
	}

	public void destroyObject(GameObject obj) {
		StartCoroutine(destroyThenRecalculate(obj));
	}

	private IEnumerator destroyThenRecalculate(GameObject obj) {
		Destroy(obj);
		yield return null;
		refreshObjects();
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
