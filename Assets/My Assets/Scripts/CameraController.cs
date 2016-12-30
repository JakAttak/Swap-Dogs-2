using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : HelperFunctions {
	private Vector3 offset;
	private Vector3 targetOffset = Vector3.zero;
	
	public float speedH = 4.0f;
	public float speedV = 2.0f;

	private Vector3 manualRotation = Vector3.zero;
	public bool panning = false;
	private Vector3 panTargetAngle = Vector3.zero;
	private Vector3 panStartAngle = Vector3.zero;
	private float panTime = 0.5f;
	private float panStart = 0f;

	public float lastUserMovement = 0.0f;

	private float magicStartTilt = -35f;
	
	private Vector2 maxUpDown = new Vector2(-40f, 30f);

	private Vector2 fovLimits = new Vector2(15f, 90f);

	private Transform target;
	
	void Start() {
		manualRotation.x = magicStartTilt;
	}
	
	void LateUpdate() {
		rotateCamera();
		moveCamera();
		zoomCamera();
	}

	// Has it been 'secs' seconds since the user last moved the camera?
	public bool untouchedIn(float secs) {
		return (Time.time - lastUserMovement) >= secs;
	}
	
	// If the mouse is being dragged, rotate the camera accordingly
	void rotateCamera() {
		if (Input.GetMouseButton(0) || Input.GetAxis("Camera1") != 0 || Input.GetAxis("Camera2") != 0) {
			lastUserMovement = Time.time;
			stopPanning(); // If the user is controlling the camera, we don't wanna interfere
		}

		if (panning) {
			manualRotation = Vector3.Lerp(panStartAngle, panTargetAngle, (Time.time - panStart) / panTime);

			panning = ((Time.time - panStart) < panTime);
		} else {
			if (Input.GetMouseButton(0)) {
				manualRotation.y += speedH * Input.GetAxis("Mouse X");
				manualRotation.x -= speedV * Input.GetAxis("Mouse Y");
			}
			
			manualRotation.y += speedH * 2 * Input.GetAxis("Camera1");
			manualRotation.x += speedV * Input.GetAxis("Camera2");
			
			manualRotation.x = Mathf.Clamp(manualRotation.x, maxUpDown.x, maxUpDown.y);
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

	// Zooms the camera in and out based on the scroll wheel
	void zoomCamera() {
		float fov = Camera.main.fieldOfView;
		fov += Input.GetAxis("Mouse ScrollWheel")  * 20;
		fov = Mathf.Clamp(fov, fovLimits.x, fovLimits.y);
		Camera.main.fieldOfView = fov;
	}
	
	// Returns the actual point the camera is targeting
	Vector3 actualTarget() {
		return target.position + rotateVector(targetOffset);
	}
	
	// Returns the offset between the target and the camera
	public Vector3 lookOffset() {
		if (target) {
			return actualTarget() - transform.position;
		}
		
		return Vector3.zero;
	}
	
	// Points the camera in a direction
	public void point(Vector3 newTarget, bool pan = true, float ptime = 0.25f) {
		Vector3 nAngle = angleNeededToPointAt(newTarget);

		if (pan) {
			panStartAngle = manualRotation;
			panTargetAngle = new Vector3(manualRotation.x + Mathf.DeltaAngle(manualRotation.x, nAngle.x), manualRotation.y + Mathf.DeltaAngle(manualRotation.y, nAngle.y), manualRotation.z + Mathf.DeltaAngle(manualRotation.z, nAngle.z));
			panStart = Time.time;
			panTime = ptime;
			panning = true;
		} else {
			manualRotation = nAngle;
		}
	}

	// Returns what the manualRotation would need to be for the camera to be pointing at 'point'
	public Vector3 angleNeededToPointAt(Vector3 point) {
		Vector3 dir = point - actualTarget();

		Vector3 oldPos = transform.position;
		Quaternion oldRot = transform.rotation;

		transform.position = actualTarget() - dir;
		transform.LookAt (point);

		Vector3 nAngle = new Vector3 (magicStartTilt + (target.position.y - point.y) * 2, transform.rotation.eulerAngles.y, 0.0f);

		transform.position = oldPos;
		transform.rotation = oldRot;

		return nAngle;
	}

	// Returns the distance between the angle the camera is at and the angle it would need to be at to point at 'point'
	// 		value is rounded to 2 decimal places
	public float currentLookDistance(Vector3 point) {
		Vector3 dest = angleNeededToPointAt(point);
		return Mathf.Sqrt(Mathf.Pow(Mathf.DeltaAngle(manualRotation.x, dest.x), 2) + Mathf.Pow(Mathf.DeltaAngle(manualRotation.y, dest.y), 2) + Mathf.Pow(Mathf.DeltaAngle(manualRotation.z, dest.z), 2));
	}

	// Stops the camera from panning
	void stopPanning() {
		panning = false;
	}
	
	void setOffset(Transform player) {
		offset = transform.position - player.position;
	}
	
	public void setTarget(Transform obj) {
		// set the offset
		offset = obj.gameObject.GetComponent<PlayerController>().cameraDistance;
		if (offset == Vector3.zero) {
			setTargetOffset(new Vector3(0, -1, 2));
		} else {
			setTargetOffset(Vector3.zero);
		}
		
		// set our target variable to this transform
		target = obj;
	}

	public Transform getTarget() {
		return target;
	}

	public void setConstraints(Vector2 max) {
		maxUpDown = max;
	}
	
	public void setTargetOffset(Vector3 off) {
		targetOffset = off;
	}
	
}
