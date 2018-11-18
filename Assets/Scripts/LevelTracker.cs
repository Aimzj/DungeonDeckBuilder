using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTracker : MonoBehaviour {
    public int currentLevel;

	void Awake () {
        DontDestroyOnLoad(this.gameObject);     
    }

    private void Start()
    {
        print("The real level is: " + currentLevel);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
