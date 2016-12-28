using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	private Vector3 offset;
	private Vector3 targetOffset = Vector3.zero;
	
	public float speedH = 4.0f;
	public float speedV = 2.0f;
	
	private Vector3 manualRotation = Vector3.zero;
	private bool panning = false;
	private Vector3 panTargetAngle = Vector3.zero;
	private float panTime = 0.5f;
	private float panStart = 0f;

	private float magicStartTilt = -35f;
	
	private Vector2 maxUpDown = new Vector2(-40f, 30f);

	private Transform target;
	
	void Start () {
		manualRotation.x = magicStartTilt;
	}
	
	void LateUpdate() {
		rotateCamera();
		moveCamera();
	}
	
	// If the mouse is being dragged, rotate the camera accordingly
	void rotateCamera() {
		if (panning) {
			manualRotation = Vector3.Lerp(manualRotation, panTargetAngle, (Time.time - panStart) / panTime);
			
			panning = ((Time.time - panStart) < panTime);
		} else {
			if (Input.GetMouseButton(0)) {
				manualRotation.y += speedH * Input.GetAxis ("Mouse X");
				manualRotation.x += speedV * Input.GetAxis ("Mouse Y");
			}
			
			manualRotation.y += speedH * 2 * Input.GetAxis("Camera1");
			manualRotation.x += speedV * (int) Input.GetAxis ("Camera2");
			
			manualRotation.x = Mathf.Min (Mathf.Max(manualRotation.x, maxUpDown.x), maxUpDown.y);
		}
	}
	
	// Helper function to rotate a vector
	Vector3 rotateVector(Vector3 vec) {
		return Quaternion.Euler(new Vector3(manualRotation.x, manualRotation.y, 0f)) * vec;
	}
	
	// Moves the camera to be offset distance from the target, and properly rotated per our variables
	void moveCamera() {
		transform.position = target.position + rotateVector(offset); // Set camera position to be offset away from target and rotated around it
		transform.LookAt(actualTarget()); // Rotate to look at the target
	}
	
	// Returns the actual point the camera is targeting
	Vector3 actualTarget() {
		return target.position + rotateVector (targetOffset);
	}
	
	// Returns the offset between the target and the camera
	public Vector3 lookOffset() {
		if (target) {
			return actualTarget() - transform.position;
		}
		
		return Vector3.zero;
	}
	
	// Points the camera in a direction
	public void point(Vector3 newTarget, bool pan) {
		Vector3 dir = newTarget - actualTarget();
		
		Vector3 oldPos = transform.position;
		Quaternion oldRot = transform.rotation;
		
		transform.position = actualTarget() - dir;
		transform.LookAt(newTarget);

		Vector3 nAngle = new Vector3(magicStartTilt + (target.position.y - newTarget.y) * 2, transform.rotation.eulerAngles.y, 0.0f);

		if (pan) {
			panTargetAngle = nAngle;
			panning = true;
			panStart = Time.time;
			
			transform.position = oldPos;
			transform.rotation = oldRot;
		} else {
			manualRotation = nAngle;
		}
	}
	
	void setOffset(Transform player) {
		offset = transform.position - player.position;
	}
	
	public void setTarget(Transform obj) {
		// set the offset
		offset = obj.gameObject.GetComponent<PlayerController>().cameraDistance;
		if (offset == Vector3.zero) {
			setTargetOffset(new Vector3(offset.x, offset.y - 1, offset.z + 2));
		} else {
			setTargetOffset(Vector3.zero);
		}
		
		// set our target variable to this transform
		target = obj;
	}
	
	public void setConstraints(Vector2 max) {
		maxUpDown = max;
	}
	
	public void setTargetOffset(Vector3 off) {
		targetOffset = off;
	}
	
}
