using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
    public float speed = 0.1f;
    public GameObject options;
	// Use this for initialization
	void Start () 
    {
	
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 0.8f;
        }
        else
            speed = 0.2f;

        Vector3 mousePos = (Input.mousePosition);

        //if (!options.activeSelf)
        //    transform.position = new Vector3(mousePos.x / 2 - (Screen.width / 4), mousePos.y / 2 - (Screen.height / 4), -10);
        //else
        //    transform.position = new Vector3(50, 50, -10);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -150f, 250f), Mathf.Clamp(transform.position.y, -150f, 250f), -10.0f);

        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - (100*speed) * Input.GetAxis("Mouse ScrollWheel"), 1, 200);

        Vector3 offset = Camera.main.ScreenToViewportPoint(new Vector3(Input.mousePosition.x - (Screen.width / 2), Input.mousePosition.y - (Screen.height / 2), 0.0f));

        if (!options.activeSelf && Input.GetMouseButton(1))
            Camera.main.transform.position = transform.position + offset * speed;
        else if (options.activeSelf)
            transform.position = new Vector3(50, 50, -10);
	}
}
