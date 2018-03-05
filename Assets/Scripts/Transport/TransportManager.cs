using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportManager : MonoBehaviour {

    public MetroStation selectedMetroStation;
    Transform metroPanel;
    public Lines selectedLine = Lines.None;
    Cursor cursor;

    public enum Lines{
        Metro,
        Bus,
        Train,
        None
    }

    private void Start()
    {
        cursor = GameObject.FindGameObjectWithTag("_Manager").GetComponent<Cursor>();
        metroPanel = GameObject.FindGameObjectWithTag("MetroPanel").transform;
    }

    void Update () {
        UpdatePanelPosition();
	}

    void UpdatePanelPosition()
    {
        if (selectedMetroStation == null)
        {
            metroPanel.SetPositionAndRotation(new Vector3(5000, 5000), Quaternion.identity);
            return;
        }

        int x = selectedMetroStation.x;
        int y = selectedMetroStation.y;
        float tileWidth = 1.73f;
        float tileHeight = 1.49f;

        if (y % 2 == 0)
        {
            metroPanel.SetPositionAndRotation(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToScreenPoint(new Vector3(x * tileWidth, y * tileHeight, 0)), Quaternion.identity);
        }
        else
        {
            metroPanel.SetPositionAndRotation(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().WorldToScreenPoint(new Vector3(x * tileWidth + tileWidth / 2, y * tileHeight, 0)), Quaternion.identity);
        }
    }

    public void StartBuildingMetroLine()
    {
        cursor.currentCursorTool = Cursor.CursorTools.None;
        selectedLine = Lines.Metro;
    }
    public void StartBuildingBusLine()
    {
        cursor.currentCursorTool = Cursor.CursorTools.None;
        selectedLine = Lines.Bus;
    }
    public void StartBuildingTrainLine()
    {
        cursor.currentCursorTool = Cursor.CursorTools.None;
        selectedLine = Lines.Train;
    }
    public void SetSelectedLineToNone()
    {
        cursor.currentCursorTool = Cursor.CursorTools.None;
        selectedLine = Lines.None;
    }
}
