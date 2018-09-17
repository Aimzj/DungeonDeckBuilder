using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderSet : MonoBehaviour {
    Material setMat;
    public Texture2D Currenttexture;
    public SpriteRenderer setRend;
	// Use this for initialization
	void Start () {
        setRend = GetComponent<SpriteRenderer>();
        setMat = GetComponent<SpriteRenderer>().material;
        setRend.material.mainTexture = Currenttexture;
        //setRend.material.SetTexture("_Texture", Currenttexture);
        //setMat.SetTexture("_Texture",Currenttexture);
        //setRend.material = setMat;
        //setRend.material.mainTexture = Currenttexture;
        //setRend.material.SetTexture("_Texture", Currenttexture);
    }
	

}
