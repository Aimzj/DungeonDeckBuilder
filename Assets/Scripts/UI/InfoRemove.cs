using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoRemove : MonoBehaviour {

    public GameObject PopUp;

    private void OnMouseUp()
    {
        PopUp.transform.position = new Vector3(0f, 0f, 0f);
    }
}
