using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicChange : MonoBehaviour {


    [SerializeField]
    Material cardMat;
    [SerializeField]
    Color cardColour;
    [SerializeField]
    Texture cardArt;



    private void OnMouseDown()
    {
        // cardMat.SetColor("_color", cardColour);
        cardMat.SetTexture("Texture2D_AD8DEE7D", cardArt);
        print("click");
    }

    private void Start()
    {
        cardMat.SetTexture("Texture2D_AD8DEE7D", cardArt);
    }
}
