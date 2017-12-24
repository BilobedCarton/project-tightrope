using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls mouse based controls for the user.
public class MouseController : MonoBehaviour
{
	public GameObject selectionBracketPrefab;

	// Selected currently by the user.
	Cell selected;

	// Relevant for camera movement.
	Vector3 lastFramePosition;
	Vector3 currFramePosition;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		// updates involved in camera management.
		currFramePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		currFramePosition.z = 0;
		UpdateCameraMovement ();
		UpdateSelection ();
		lastFramePosition = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		lastFramePosition.z = 0;
	}

	// Updates the camera with drag actions and user zoom movements.
	void UpdateCameraMovement ()
	{
		if (Input.GetMouseButton (1) || Input.GetMouseButton (2)) {
			Vector3 diff = lastFramePosition - currFramePosition;
			Camera.main.transform.Translate (diff);
		}

		Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis ("Mouse ScrollWheel");
		Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, 3f, 50f);
	}

	void UpdateSelection ()
	{
		if (Input.GetMouseButton (0)) {
			selected = WorldController.Instance.GetCellDataAtWorldCoord (currFramePosition);
			if (selected != null) {
				Vector3 cursorPosition = new Vector3 (selected.X, selected.Y, 0);
				selectionBracketPrefab.transform.position = cursorPosition;
				selectionBracketPrefab.SetActive (true);
			} else {
				selectionBracketPrefab.SetActive (false);
			}
		}
	}
}
