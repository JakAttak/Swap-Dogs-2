using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunctions : MonoBehaviour {
	
	// Helper functions for rounding things
	public float roundToXPlaces(float num, int places) {
		int multi = (int) Mathf.Pow(10, places);
		return Mathf.Round(num * multi) / multi;
	}

	public Vector3 roundVector3(Vector3 vec, int places) {
		return new Vector3(roundToXPlaces(vec.x, places), roundToXPlaces(vec.y, places), roundToXPlaces(vec.z, places));
	}

	// like Math.min, but ignores the sign
	public float minIgnoreSign(float one, float two) {
		if (Mathf.Abs(one) < Mathf.Abs(two)) {
			return one;
		}

		return two;
	}
	// like Math.max, but ignores the sign
	public float maxIgnoreSign(float one, float two) {
		if (Mathf.Abs(one) > Mathf.Abs(two)) {
			return one;
		}

		return two;
	}

	// Returns an angle equivalent to 'start' that is closest in value to 'target'
	public float angleSmallestDifference(float start, float target) {
		/*if (Mathf.Abs(target - start) < 180) {
			return start;
		}

		float side1 = ((360 - (target - start)) + target);
		float side2 = side1 % 360;

		if (Mathf.Abs(target - side1) < Mathf.Abs(target - side2)) {
			return side1;
		}

		return side2;*/

		if (start > 180) {
			return start - 360;
		} else if (start < -180) {
			return start + 360;
		}

		return start;
	}
}
