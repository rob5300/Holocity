using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionCreator : MonoBehaviour {

	void Start () {
        Game.SetSession(new Session());
        Debug.Log("New session created and set: " + Game.CurrentSession.Name);
        Game.CurrentSession.City.CreateGrid();
	}
	
}
