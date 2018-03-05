using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiManager : MonoBehaviour {

    MapScript ms;
    public bool windowOpen = false;

    int[] generatorSettings = new int[]
        {
            3, //Shore Radicality
            5, //Big Districts amount
            60,//Width
            40,//Height
            4, //District Size
            15 //Resource Amount
        };

    [Command("regen-map", "Make a new map.")]
    public void RegenMap()
    {
        GameObject mapScript = GameObject.FindGameObjectWithTag("MapTiles");
        ms = mapScript.GetComponent<MapScript>();

        ms.generatorSettings = this.generatorSettings;
        ms.ResetMap();
    }

    public void WindowOpenToTrue()
    {
        if (windowOpen)
            windowOpen = false;
        else
            windowOpen = true;
    }

    //Searches for button declarations from this function, dont call it, its called automatically
    private Rect windowRect = new Rect(200, 200, 300, 500);
    public float shorelineRad = 3.0F;
    public float bigDistricts = 2.0F;
    public float districtSize = 50.0F;

    [Command("set-mapgen-settings", "(shoreRadicality, districtAmount, mapWidth, mapHeight, districtSize, resourceAmount)")]
    public void SetGeneratorSettings(int shoreRadicality,int districtAmount,int mapWidth,int mapHeight,int districtSize,int resourceAmount)
    {
        this.generatorSettings = new int[]
        {
            shoreRadicality, //Shore Radicality
            districtAmount, //Big Districts amount
            mapWidth,//Width
            mapHeight,//Height
            districtSize, //District Size
            resourceAmount //Resource Amount
        };
    }

    [Command("make-debug-election", "Create a debug election")]
    public void MakeElectionDebug()
    {
        TimeManager timeManager = GameObject.FindGameObjectWithTag("_Manager").GetComponent<TimeManager>();
        timeManager.gameEventManager.MakeNewEvent(new GameEvent(timeManager.AddDaysToDate(timeManager.GetGameDate(), 0), GameEventTypes.communal_election_start, null, null));
    }

    [Command("show-controls", "Show the controls in console.")]
    public string ShowControls()
    {
        return "keys: WASD to move; 1,2,3 for tools; Q,E for rotate; R to change rail";
    }
    
}
