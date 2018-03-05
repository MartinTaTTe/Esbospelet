using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour
{
    public float tileWidth;
    public float tileHeight;
    int counter = 0;
    //2D array to hold all tiles, which makes it easier to reference adjacent tiles etc.
    public Map map;
    public LineFactory lineFactory;
    public Transform[,,] drawnObjects;//3rd index is 0:Background 1:Roads 2:Content(houses) 3:Texts 4:Stations 5:Lines
    private List<Transform> drawnPublicTransportLines = new List<Transform>();
    private int GROWSPEED = 1;
    public int[] generatorSettings = new int[6]
        {
            3, //Shore Radicality
            2, //Big Districts amount
            60,//Width
            40,//Height
            20,//District Size
            40 //How Many Resources
        };

    void Start()
    {
        lineFactory = GameObject.FindGameObjectWithTag("LineFactory").GetComponent<LineFactory>();
        drawnObjects = new Transform[300,300,10];

        tileWidth = 1.73f;
        tileHeight = 1.49f;

        map = new Map(this.generatorSettings, this);

        DrawBackground();
        DrawRoads();
        DrawHouses();
        DrawResources();

        foreach (District district in map.districts)
        {
            Coordinates coordinates = district.tileLocations[0]; //TODO: See why this line sometimes errors when game started
            int x = coordinates.x;
            int y = coordinates.y;

            var go = Instantiate(Resources.Load<Transform>("prefabText"), new Vector3(x * tileWidth, y * tileHeight, 0), Quaternion.identity);
            go.SetPositionAndRotation(new Vector3(x * tileWidth, y * tileHeight, 0),Quaternion.identity);
            go.GetComponent<TextMesh>().text = district.districtName;
            go.GetComponent<MeshRenderer>().sortingOrder = 5;

            drawnObjects[x,y,3] = go;
        }

        GameObject.FindGameObjectWithTag("_Manager").GetComponent<LocalPolitics>().GeneratePolitics();

    }
    private void Update()
    {
        RedrawChangedTiles();


        //Change Map --> MapDemo in this File
        counter++;
        if (counter > 90/GROWSPEED)
        {
            map.OneDistrictStep();
            counter = 0;
        }
        RedrawChangedTiles();
    }

    [Command("set-growspeed", "(growSpeed)")]
    public void SetGrowSpeedCommand(int growSpeed)
    {
        this.GROWSPEED = growSpeed;
    }

    public void RedrawChangedTiles()
    {
        for(int x = 0; x < map.mapWidth; x++)
        {
            for (int y = 0; y < map.mapHeight; y++)
            {
                if (map.tileToRedraw[x, y])
                {
                    map.tileToRedraw[x, y] = false;

                    DrawOneBackground(x, y, true);
                    DrawOneRoad(x, y, true);
                    DrawOneHouse(x, y, true);
                    DrawOneResource(x, y, true);
                    DrawOneMetroStation(x, y, true);
                }
            }
        }
    }
    public void DrawBackground()
    {
        for (int y = 0; y < map.mapHeight; y++)
        {
            for (int x = 0; x < map.mapWidth; x++)
            {
                DrawOneBackground(x, y, false);
            }
        }
    }
    public void DrawRoads()
    {
        for (int y = 0; y < map.mapHeight; y++)
        {
            for (int x = 0; x < map.mapWidth; x++)
            {
                DrawOneRoad(x, y, false);
            }
        }
    }
    public void DrawHouses()
    {
        for (int y = 0; y < map.mapHeight; y++)
        {
            for (int x = 0; x < map.mapWidth; x++)
            {
                DrawOneHouse(x, y, false);
            }
        }
    }
    public void DrawResources()
    {
        for (int y = 0; y < map.mapHeight; y++)
        {
            for (int x = 0; x < map.mapWidth; x++)
            {
                DrawOneResource(x, y, false);
            }
        }
    }
    public void DrawMetroStations()
    {
        for (int y = 0; y < map.mapHeight; y++)
        {
            for (int x = 0; x < map.mapWidth; x++)
            {
                DrawOneMetroStation(x, y, false);
            }
        }
    }
    public void DrawOneBackground(int x, int y, bool redraw)
    {
        Transform traf;
        if (y % 2 == 0)
        {
            traf = Instantiate(map.backgroundTiles[x, y].transform, new Vector3(x * tileWidth, y * tileHeight, 0), map.backgroundTiles[x, y].transform.rotation);
        }
        else
        {
            traf = Instantiate(map.backgroundTiles[x, y].transform, new Vector3(x * tileWidth + tileWidth / 2, y * tileHeight, 0), map.backgroundTiles[x, y].transform.rotation);
        }
        int rotation = 60 * map.backgroundTiles[x, y].rotation;
        traf.transform.eulerAngles = new Vector3(traf.transform.eulerAngles.x,traf.transform.eulerAngles.y,rotation);

        if (redraw)
        {
            if (drawnObjects[x, y, 0] != null && drawnObjects[x, y, 0].gameObject)
            {
                GameObject.Destroy(drawnObjects[x, y, 0].gameObject);
            }
        }

        traf.SetParent(transform);
        drawnObjects[x,y,0] = traf;

        traf.GetComponent<TileMouseOverLocation>().x = x;
        traf.GetComponent<TileMouseOverLocation>().y = y;
    }
    public void DrawOneRoad(int x,int y, bool redraw)
    {
        Transform traf;
        if (map.roadTiles[x, y] != null)
        {
            if (y % 2 == 0)
            {
                traf = Instantiate(map.roadTiles[x, y].transform, new Vector3(x * tileWidth, y * tileHeight, 0), map.roadTiles[x, y].transform.rotation);
            }
            else
            {
                traf = Instantiate(map.roadTiles[x, y].transform, new Vector3(x * tileWidth + tileWidth / 2, y * tileHeight, 0), map.roadTiles[x, y].transform.rotation);
            }
            if (map.roadTiles[x, y].tileType == Map.Type.District)
            {
                int rotation = 60 * map.roadTiles[x, y].rotation;
                traf.Rotate(new Vector3(0, 0, rotation));
            }

            if (redraw)
            {
                if (drawnObjects[x, y, 1] != null && drawnObjects[x, y, 1].gameObject)
                {
                    GameObject.Destroy(drawnObjects[x, y, 1].gameObject);
                }
            }

            traf.SetParent(transform);
            drawnObjects[x,y,1] = traf;
        }
    }
    public void DrawOneHouse(int x, int y, bool redraw)
    {
        Transform traf;
        
        if (map.houseTiles[x, y] != null)
        {
            Debug.Log("ADAWDAW");
            if (y % 2 == 0)
            {
                traf = Instantiate(map.houseTiles[x, y].transform, new Vector3(x * tileWidth, y * tileHeight, 0), map.houseTiles[x, y].transform.rotation);
            }
            else
            {
                traf = Instantiate(map.houseTiles[x, y].transform, new Vector3(x * tileWidth + tileWidth / 2, y * tileHeight, 0), map.houseTiles[x, y].transform.rotation);
            }
            if (map.houseTiles[x, y].tileType == Map.Type.District)
            {
                int rotation = 60 * map.houseTiles[x, y].rotation;

                traf.Rotate(new Vector3(-90, -90, 90));
                traf.Rotate(new Vector3(0,-rotation,0));
            }

            if (redraw)
            {
                if (drawnObjects[x, y, 2] != null && drawnObjects[x, y, 2].gameObject)
                {
                    GameObject.Destroy(drawnObjects[x, y, 2].gameObject);
                }
            }

            traf.SetParent(transform);
            drawnObjects[x,y,2] = traf;
        }
    }
    public void DrawOneFactory(int x, int y, bool redraw)
    {
        Transform traf;
        
        if (map.houseTiles[x, y] != null)
        {
            if (y % 2 == 0)
            {
                traf = Instantiate(map.houseTiles[x, y].transform, new Vector3(x * tileWidth, y * tileHeight, 0), map.houseTiles[x, y].transform.rotation);
            }
            else
            {
                traf = Instantiate(map.houseTiles[x, y].transform, new Vector3(x * tileWidth + tileWidth / 2, y * tileHeight, 0), map.houseTiles[x, y].transform.rotation);
            }
            if (map.houseTiles[x, y].tileType == Map.Type.District)
            {
                int rotation = 60 * map.houseTiles[x, y].rotation;

                traf.Rotate(new Vector3(-90, -90, 90));
                traf.Rotate(new Vector3(0, -rotation, 0));
            }

            if (redraw)
            {
                if (drawnObjects[x, y, 2] != null && drawnObjects[x, y, 2].gameObject)
                {
                    GameObject.Destroy(drawnObjects[x, y, 2].gameObject);
                }
            }

            traf.SetParent(transform);
            drawnObjects[x, y, 2] = traf;
        }
    }
    public void DrawOneResource(int x, int y, bool redraw)
    {
        Transform traf;

        if (map.resourceTiles[x, y] != null)
        {
            if (y % 2 == 0)
            {
                traf = Instantiate(map.resourceTiles[x, y].transform, new Vector3(x * tileWidth, y * tileHeight, 0), map.resourceTiles[x, y].transform.rotation);
            }
            else
            {
                traf = Instantiate(map.resourceTiles[x, y].transform, new Vector3(x * tileWidth + tileWidth / 2, y * tileHeight, 0), map.resourceTiles[x, y].transform.rotation);
            }
            if (map.resourceTiles[x, y].tileType == Map.Type.District)
            {
                int rotation = 60 * map.resourceTiles[x, y].rotation;
                traf.transform.eulerAngles = new Vector3(traf.transform.eulerAngles.x, traf.transform.eulerAngles.y, rotation);
            }

            if (redraw)
            {
                if (drawnObjects[x, y, 2] != null && drawnObjects[x, y, 2].gameObject)
                {
                    GameObject.Destroy(drawnObjects[x, y, 2].gameObject);
                }
            }

            traf.SetParent(transform);
            drawnObjects[x, y, 2] = traf;
        }
    }
    public void DrawOneMetroStation(int x, int y, bool redraw)
    {
        Transform traf;
        MetroStation metroStation = map.GetMetroStationInTileIfExistsInTile(x, y);
        //TODO DRAW CONNECTIONS

        if (metroStation != null)
        {
            if (y % 2 == 0)
            {
                traf = Instantiate(map.metroStation, new Vector3(x * tileWidth, y * tileHeight, 0), Quaternion.identity);
            }
            else
            {
                traf = Instantiate(map.metroStation, new Vector3(x * tileWidth + tileWidth / 2, y * tileHeight, 0), Quaternion.identity);
            }

            if (redraw)
            {
                if (drawnObjects[x, y, 4] != null && drawnObjects[x, y, 4].gameObject)
                {
                    GameObject.Destroy(drawnObjects[x, y, 4].gameObject);
                }
            }

            traf.SetParent(transform);
            drawnObjects[x, y, 4] = traf;
        }
    }
    public void ReDrawMetroConnections()
    {
        var activeLines = lineFactory.GetActive();
        foreach (var line in activeLines)
        {
            line.gameObject.SetActive(false);
        }

        foreach (MetroStation fromStation in map.metroStations)
        {
            foreach (MetroStation toStation in fromStation.GetMetroConnections())
            {
                Vector3 fromVector;
                Vector3 toVector;
                if (fromStation.y % 2 == 0)
                {
                    fromVector = new Vector3(fromStation.x * tileWidth, fromStation.y * tileHeight, 2);
                }
                else
                {
                    fromVector = new Vector3(fromStation.x * tileWidth + tileWidth / 2, fromStation.y * tileHeight, 2);
                }

                if (toStation.y % 2 == 0)
                {
                    toVector = new Vector3(toStation.x * tileWidth, toStation.y * tileHeight, 2);
                }
                else
                {
                    toVector = new Vector3(toStation.x * tileWidth + tileWidth / 2, toStation.y * tileHeight, 2);
                }

                Line drawnLine = lineFactory.GetLine(fromVector, toVector, 0.08f, Color.red);

            }
        }
    }
    public void ResetMap()
    {
        for (int x = 0; x < map.mapWidth; x++)
        {
            for (int y = 0; y < map.mapHeight; y++)
            {
                for (int layer = 0; layer < 4; layer++)
                {
                    if (drawnObjects[x, y, layer] != null && drawnObjects[x, y, layer].gameObject)
                    {
                        GameObject.Destroy(drawnObjects[x, y, layer].gameObject);
                    }
                }
            }
                
        }
        Start();
    }
}