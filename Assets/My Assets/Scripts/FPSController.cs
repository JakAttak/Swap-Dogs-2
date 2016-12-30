using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSController : ObjectController {

	public override void Start() {
		base.Start();

		setFontSize(400);
		colorText(Color.red);
	}

	void LateUpdate() {
		displayText("FPS: " + (int) (1.0f / Time.deltaTime));
		transform.position = GameObject.FindGameObjectWithTag("Player").transform.position + new Vector3(0.0f, 4.0f, 0.0f);
	}
}
