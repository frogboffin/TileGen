using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Move : MonoBehaviour {

    public Texture2D tex;
    public int height;
    public int width;
    public Vector2 center;

    public int a = 0;
    public Color bG;
    public Color32 player;

	// Use this for initialization
	void Start () {
        bG = tex.GetPixel(0, 0);
        height = tex.height;
        width = tex.width;
        center.x = width / 2;
        center.y = height / 2;
	}
	
	// Update is called once per frame
	void Update () {
        tex.SetPixel(a, 0, player);
        tex.Apply();
	}
}
