using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls other aspects of the UI.
public class UserInterfaceController : MonoBehaviour
{
	public static UserInterfaceController Instance;

	public GameObject SelectionBracketPrefab;

	// Use this for initialization
	void OnEnable ()
	{
		if (Instance != null) {
			Debug.LogError ("UserInterfaceController.OnEnable() -- Instance should be null but isn't.");
		}

		Instance = this;
	}
	
	// Update is called once per frame
	void Update ()
	{
		UpdateMainMenuEscPress ();
	}

	// This locates and activates the selection bracket sprite to be in the correct spot for the selected cell.
	public void SetSelectionBracket ()
	{
		if (WorldController.Instance.SelectedCell != null) {
			this.SelectionBracketPrefab.transform.position = 
				new Vector3 (WorldController.Instance.SelectedCell.X, WorldController.Instance.SelectedCell.Y, 0);
			this.SelectionBracketPrefab.SetActive (true);
		} else {
			this.SelectionBracketPrefab.SetActive (false);
		}
	}

	// Updates the visiblity of the ingame main menu depending on user input ("esc" key)
	private void UpdateMainMenuEscPress ()
	{
		if (Input.GetButtonUp ("Escape") == true) {
			IngameMenuPanelController.Instance.Toggle ();
		}
	}
}
