using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public string startLevel;

	public string localLevel;

	public void NewStart(){
		Application.LoadLevel (startLevel);
	}

	public void LocalStart(){
		Application.LoadLevel (localLevel);
	}

	public void QuitGame(){
		Application.Quit ();
	}
}
