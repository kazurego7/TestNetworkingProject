using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Billboard : MonoBehaviour {

	void Update () {
		GetComponent<RectTransform> ().LookAt (Camera.main.transform);
	}
}