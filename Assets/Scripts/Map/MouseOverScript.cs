using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseOverScript : MonoBehaviour {

    public Cursor cursor;
    public int cursorX;
    public int cursorY;

    bool rightMouseIsDown = false;
    bool leftMouseIsDown = false;
    bool mouseIsOver = false;


    private void Start()
    {
        cursor = GameObject.FindGameObjectWithTag("_Manager").GetComponent<Cursor>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && mouseIsOver)
        {
            cursor.MouseLeftClicked(cursorX, cursorY);
        }

        if (Input.GetMouseButtonDown(1) && mouseIsOver)
        {
            cursor.MouseRightClicked(cursorX, cursorY);
        }
    }

    private void OnMouseEnter()
    {
        mouseIsOver = true;

        cursorX = transform.gameObject.GetComponent<TileMouseOverLocation>().x;
        cursorY = transform.gameObject.GetComponent<TileMouseOverLocation>().y;

        cursor.SetCursorPos(cursorX, cursorY);
    }
    private void OnMouseExit()
    {
        mouseIsOver = false;
    }
}
