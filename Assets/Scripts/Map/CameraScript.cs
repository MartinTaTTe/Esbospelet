using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public float scrollSpeed = 0.5f;
    public float zoomSpeed = 15f;
    public float zoomLevel = 0.9f;

    Camera camera;
    Camera lineCamera;

    void Start () {
        camera = Camera.main;
        lineCamera = GameObject.FindGameObjectWithTag("LineCamera").GetComponent<Camera>();

        camera.transform.Rotate(new Vector3(-35, 0, 0));
        lineCamera.transform.Rotate(new Vector3(-35, 0, 0));
    }
	
	// Update is called once in a while regardless of frames// (OTTO)
	void FixedUpdate () {

        bool keydown = Input.GetKey("w") || Input.GetKey("up")
                    || Input.GetKey("s") || Input.GetKey("down")
                    || Input.GetKey("a") || Input.GetKey("left")
                    || Input.GetKey("d") || Input.GetKey("right");

        if (keydown)
        {
            camera.transform.Rotate(new Vector3(35, 0, 0));
            lineCamera.transform.Rotate(new Vector3(35, 0, 0));
        }

        if (Input.GetKeyDown("[+]"))
        {
            if (zoomLevel > 0.45)
            {
                zoomLevel -= (float)0.15;
                camera.transform.Translate(0, (float)0.15 * zoomSpeed, (float)0.15 * zoomSpeed, Space.World);
                lineCamera.transform.Translate(0, (float)0.15 * zoomSpeed, (float)0.15 * zoomSpeed, Space.World);
            }
        }
        if (Input.GetKeyDown("[-]"))
        {
            if (zoomLevel < 3)
            {
                zoomLevel += (float)0.15;
                camera.transform.Translate(0, (float)-0.15 * zoomSpeed, (float)-0.15 * zoomSpeed, Space.World);
                lineCamera.transform.Translate(0, (float)-0.15 * zoomSpeed, (float)-0.15 * zoomSpeed, Space.World);
            }
        }

        if (Input.GetKey("w") || Input.GetKey("up"))
        {
            camera.transform.Translate(new Vector3(0, scrollSpeed * zoomLevel, 0));
            lineCamera.transform.Translate(new Vector3(0, scrollSpeed * zoomLevel, 0));
        }
        if (Input.GetKey("s") || Input.GetKey("down"))
        {
            camera.transform.Translate(new Vector3(0, -scrollSpeed * zoomLevel, 0));
            lineCamera.transform.Translate(new Vector3(0, -scrollSpeed * zoomLevel, 0));
        }
        if (Input.GetKey("a") || Input.GetKey("left"))
        {
            camera.transform.Translate(new Vector3(-scrollSpeed * zoomLevel, 0f, 0));
            lineCamera.transform.Translate(new Vector3(-scrollSpeed * zoomLevel, 0f, 0));
        }
        if (Input.GetKey("d") || Input.GetKey("right"))
        {
            camera.transform.Translate(new Vector3(scrollSpeed * zoomLevel, 0f, 0));
            lineCamera.transform.Translate(new Vector3(scrollSpeed * zoomLevel, 0f, 0));
        }

        if (keydown)
        {
            camera.transform.Rotate(new Vector3(-35, 0, 0));
            lineCamera.transform.Rotate(new Vector3(-35, 0, 0));
        }

        if(zoomLevel > 0.45 && zoomLevel < 3)
        {
            zoomLevel -= Input.GetAxis("Mouse ScrollWheel");
            camera.transform.Translate(0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.World);
            lineCamera.transform.Translate(0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.World);
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0 && zoomLevel < 3)
        {
            zoomLevel -= Input.GetAxis("Mouse ScrollWheel");
            camera.transform.Translate(0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.World);
            lineCamera.transform.Translate(0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.World);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0 && zoomLevel > 0.35)
        {
            zoomLevel -= Input.GetAxis("Mouse ScrollWheel");
            camera.transform.Translate(0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.World);
            lineCamera.transform.Translate(0, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Input.GetAxis("Mouse ScrollWheel") * zoomSpeed, Space.World);
        }
    }
}
