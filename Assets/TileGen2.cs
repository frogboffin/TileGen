using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TileGen2 : MonoBehaviour {

    public enum Tiles { Water, Sand, Grass };
    Tiles tileType;
    Color[] colorList = new Color[3]
    {
         new Color(0.95f,0.95f,0.95f,1.0f),
         new Color(1.0f,1.0f,1.0f,1.0f),
         new Color(1.05f,1.05f,1.05f,1.0f),
    };

    public Color currentColor = new Color32(234, 214, 140, 255);
    public Color sandColor = new Color32(234, 214, 140, 255);
    public Color grassColor = new Color32(77, 169, 65, 255);
    Color water;

    //public Slider islandSize;
    public Text colorText;
    public Texture2D tex;
    public GameObject options;
    public int count = 0;
    public int dir = 0;
    bool shiftDown = false;

    //Option sliders/text
    [Header("Option Sliders and Text")]
    
    public Text islandSizeText;

    public Text waterText, fillText, spreadText, smoothText, beachText;

    public Slider waterSlider, fillSlider, sizeSlider, spreadSlider, smoothSlider, beachSlider;

    public Button clearButton, genButton, randomButton;

    [Header("Painting Variables")]
    public int size = 1;
    public int sandAmount = 5;
    public int coreSize = 1;
    public float coreDistance = 1;

    [Header("Island Gen")]
    public float sandSizeRandom = 5;
    public float waterSizeRandom = 5;
    public float grassSizeRandom = 5;
	// Use this for initialization
	void Start () 
    {
        water = tex.GetPixel(0,0);
        //AttemptToPlace(new Vector2(Random.Range(0, tex.width), Random.Range(0, tex.height)));
	}
	
	// Update is called once per frame
	void Update () 
    {
        waterText.text = waterSlider.value.ToString();
        fillText.text = fillSlider.value.ToString();
        spreadText.text = spreadSlider.value.ToString();
        smoothText.text = smoothSlider.value.ToString();
        beachText.text = beachSlider.value.ToString();
        islandSizeText.text = sizeSlider.value.ToString();

        size = (int)sizeSlider.value;
        sandAmount = (int)beachSlider.value;
        coreSize = (int)smoothSlider.value;
        coreDistance = spreadSlider.value;
        sandSizeRandom = fillSlider.value;
        waterSizeRandom = waterSlider.value;
        grassSizeRandom = 5;

        int mult = 1;

        if (shiftDown)
            mult = 5;
        else
            mult = 1;

        size += (int)(Input.GetAxis("Mouse ScrollWheel") * Random.Range(90, 110) * mult);

        //if (size < 0)
        //    size = 9001;
        //if (size > 9999)
        //    size = 1;

        //coreSize = Mathf.Clamp(coreSize, 1, 4);
        //coreDistance = Mathf.Clamp(coreDistance, 0, 4);
        //sandAmount = Mathf.Clamp(sandAmount, 0, 15);

        //islandSizeText.text = size.ToString();
        size = (int)sizeSlider.value;

        float x = tex.width / (Screen.width / Input.mousePosition.x);
        float y = tex.height / (Screen.height / Input.mousePosition.y);

        if (!shiftDown)
        {
            if (Input.GetMouseButtonDown(0) && !options.activeSelf)
            {
                count = 0;
                AttemptToPlace(new Vector2(x, y));
                tex.Apply();
            }
        }
        else if (Input.GetMouseButton(0) && !options.activeSelf)
        {
            count = Random.Range(0, size);
            AttemptToPlace(new Vector2(x, y));
            tex.Apply();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            shiftDown = true;
        }
        else
            shiftDown = false;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AutoPlaceIsland();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            tileType = Tiles.Grass;
            currentColor = grassColor;
            colorText.text = "Grass";
            colorText.color = grassColor;
            ChangeGrass();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            ClearMap();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            options.SetActive(!options.activeSelf);
        }

        switch(Input.inputString)
        {
            case "w":
                tileType = Tiles.Water;
                break;
            case "g":
                tileType = Tiles.Grass;
                break;
            case "s":
                tileType = Tiles.Sand;
                break;
            default:
                break;
        }

        switch (tileType)
        {
            case Tiles.Water:
                currentColor = water;
                colorText.text = "Water";
                colorText.color = water;
                break;
            case Tiles.Grass:
                currentColor = grassColor;
                colorText.text = "Grass";
                colorText.color = grassColor;
                break;
            case Tiles.Sand:
                currentColor = sandColor;                
                colorText.text = "Sand";
                colorText.color = sandColor;
                break;
        }
    }

    void ClearMap()
    {
        for (int x = 0; x < tex.width; x++)
        {
            for (int y = 0; y < tex.height; y++)
            {
                tex.SetPixel(x, y, water);
            }
        }
        tex.Apply();
    }

    void OnApplicationQuit()
    {
        ClearMap();
    }

    void AttemptToPlace(Vector2 p)
    {
        int oldDir = dir;
        dir = Random.Range(0, 4);

        while (oldDir == dir)
        {
            dir = Random.Range(0, 4);
        }

        switch (dir)
        {
            case 0: 
                p.x += Random.Range(1, coreDistance); //right
                break;
            case 1:
                p.y += Random.Range(1, coreDistance); //down
                break;
            case 2:
                p.x -= Random.Range(1, coreDistance); //left
                break;
            case 3:
                p.y -= Random.Range(1, coreDistance); //up
                break;
        }

        try
        {
            DrawColor(p);
        }
        catch
        {
            Debug.Log("ERROR");
            // do nothing
        }

        count++;

        if (count < size)
        {
            int r = Random.Range(1, 4);
            for (int i = 0; i < r; i++ )
                AttemptToPlace(p);
        }

    }

    bool IsGrass(Vector2 p)
    {
        //int sandAmount = Random.Range(8, 10);
        try
        {
            if (tex.GetPixel((int)p.x + sandAmount, (int)p.y) != water && tex.GetPixel((int)p.x - sandAmount, (int)p.y) != water && tex.GetPixel((int)p.x, (int)p.y - sandAmount) != water && tex.GetPixel((int)p.x, (int)p.y + sandAmount) != water)
                return true;
            else
                return false;
        }
        catch
        {
            return false;
        }
    }

    void ChangeGrass()
    {
        for (int x = 0; x < tex.width; x++)
        {
            for (int y = 0; y < tex.height; y++)
            {
                if (tex.GetPixel(x, y) != water)
                {
                    if (IsGrass(new Vector2(x, y)))
                    {
                        int i = Random.Range(0,colorList.Length);

                        tex.SetPixel(x, y, currentColor * colorList[i]);
                    }
                }
            }
        }
        tex.Apply();
    }

    void DrawColor(Vector2 p)
    {
        //int coreSize = Random.Range(1, 1);

        for (int x = -coreSize; x <= coreSize; x++)
        {
            for (int y = -coreSize; y <= coreSize; y++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) < coreSize + 1)
                {
                    int i = Random.Range(0, colorList.Length);

                    switch (tileType)
                    {
                        case Tiles.Grass:
                            if (tileType == Tiles.Grass)
                                if (!IsGrass(p))
                                    return;
                            tex.SetPixel((int)p.x + x, (int)p.y + y, currentColor * colorList[i]);
                            break;
                        case Tiles.Sand:
                            tex.SetPixel((int)p.x + x, (int)p.y + y, currentColor * colorList[i]);
                            break;
                        case Tiles.Water:
                            tex.SetPixel((int)p.x + x, (int)p.y + y, currentColor);
                            break;
                    }
                }
            }
        }
    }

    void AutoPlaceIsland()
    {
        options.SetActive(false);

        tileType = Tiles.Sand;
        currentColor = sandColor;
        colorText.text = "Sand";
        colorText.color = sandColor; //Generate Sand
        for (int i2 = 0; i2 < sandSizeRandom; i2++)
        {
            count = Random.Range(0, size);

            AttemptToPlace(new Vector2(tex.width / 2, tex.height / 2));

            tex.Apply();
        }

        tileType = Tiles.Water;
        currentColor = water;
        colorText.text = "Water";
        colorText.color = water; //Generate Lakes/Rivers
        for (int w = 0; w < waterSizeRandom; w++)
        {
            Debug.Log("Water Gen");
            count = Random.Range(0, size);

            AttemptToPlace(new Vector2(tex.width / 2, tex.height / 2));

            tex.Apply();
        }

        tileType = Tiles.Grass;
        currentColor = grassColor;
        colorText.text = "Grass";
        colorText.color = grassColor;
        ChangeGrass();
        tex.Apply();
        //for (int g = 0; g < grassSizeRandom; g++)
        //{
        //    //count = Random.Range(0, size);

        //    //AttemptToPlace(new Vector2(tex.width / 2, tex.height / 2));
        //    tex.Apply();
        //}
    }

    void Randomise()
    {
        sizeSlider.value = Random.Range(0,9002);
        beachSlider.value = Random.Range(0, 16);
        smoothSlider.value = Random.Range(0, 5);
        spreadSlider.value = Random.Range(0, 11);
        fillSlider.value = Random.Range(0, 101);
        waterSlider.value = Random.Range(0, 31);
    }

    void LoadOptions()
    {
        options.SetActive(!options.activeSelf);
    }
}
