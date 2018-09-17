
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoPopUp : MonoBehaviour {

    public GameObject PlayerPortrait;
    bool ppActive = false;

    private void OnMouseUp()
    {
       
            PlayerPortrait.transform.localPosition = new Vector3(-12f, -23f, 0);
        
      
       
    }
}
