using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	private void UpdateMainMenuEscPress ()
	{
		if (Input.GetButtonUp ("Escape") == true) {
			IngameMenuPanelController.Instance.Toggle ();
		}
	}
}
