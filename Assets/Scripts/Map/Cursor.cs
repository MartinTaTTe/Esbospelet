using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour {

    Transform cursor;
    int cursorX = 5;
    int cursorY = 5;
    int cursorRot = 0;
    int currentPrefabIndex = 0;
    int lastCursorPrefabIndex = -1;

    CursorTools lastCursorTool;
    public CursorTools currentCursorTool = CursorTools.None;

    bool keyIsUp = true;

    MapScript mapScript;
    SoundManager soundManager;
    MoneyManager moneyManager;
    TransportManager transportManager;
    Transform buyTextPrefab;

    public Transform[] railTilePrefabs;
    public Transform bulldozer;
    Transform metroStation;

    public enum CursorTools
    {
        None,
        Rail,
        Bulldozer,
        MetroStation
    }

    void Start () {
        transportManager = GameObject.FindGameObjectWithTag("_Manager").GetComponent<TransportManager>();
        moneyManager = GameObject.FindGameObjectWithTag("_Manager").GetComponent<MoneyManager>();

        bulldozer = Resources.Load<Transform>("Prefabs/bulldozer");
        metroStation = Resources.Load<Transform>("Prefabs/Infrastructure/MetroStation");
        railTilePrefabs = new Transform[] {
            Resources.Load<Transform>("Prefabs/Infrastructure/Rails/tile_rail_1"),
            Resources.Load<Transform>("Prefabs/Infrastructure/Rails/tile_rail_2"),
            Resources.Load<Transform>("Prefabs/Infrastructure/Rails/tile_rail_3"),
            Resources.Load<Transform>("Prefabs/Infrastructure/Rails/tile_rail_4"),
        };

        buyTextPrefab = Resources.Load<Transform>("prefabBuyText");
        mapScript = GameObject.FindGameObjectWithTag("MapTiles").GetComponent<MapScript>();
        soundManager = GameObject.FindGameObjectWithTag("_Manager").GetComponent<SoundManager>();
	}
    void Update () {
        UpdateRotation();


        if(lastCursorTool != currentCursorTool)
        {
            if(lastCursorTool != CursorTools.None)
            {
                Destroy(cursor.gameObject);
            }

            if (currentCursorTool != CursorTools.None) {
                cursor = DrawCursor(currentPrefabIndex);
            }
            lastCursorTool = currentCursorTool;
        }

        if (currentCursorTool == CursorTools.Rail)
        {
            if (lastCursorPrefabIndex != currentPrefabIndex)
            {

                Destroy(cursor.gameObject);
                cursor = DrawCursor(currentPrefabIndex);

                lastCursorPrefabIndex = currentPrefabIndex;
            }
        }
    }

    public void SetCursorPos(int x, int y)
    {
        if (currentCursorTool == CursorTools.None)
        {
            return;
        }

        cursorX = x;
        cursorY = y;
        int localCursorRot = cursorRot;

        if(currentCursorTool == CursorTools.Bulldozer || currentCursorTool == CursorTools.MetroStation)
        {
            localCursorRot = 0;
        }

        if (y % 2 == 0)
        {
            cursor.SetPositionAndRotation(new Vector3(x * mapScript.tileWidth, y * mapScript.tileHeight, 0), Quaternion.Euler(0f, 0f, localCursorRot * 60));
        }
        else
        {
            cursor.SetPositionAndRotation(new Vector3(x * mapScript.tileWidth + mapScript.tileWidth / 2, y * mapScript.tileHeight, 0), Quaternion.Euler(0f, 0f, localCursorRot * 60));
        }
    }
    public void MouseLeftClicked(int x, int y)
    {
        MetroStation metroStationInCurrTile = mapScript.map.GetMetroStationInTileIfExistsInTile(x, y);

        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }


        if (currentCursorTool == CursorTools.Rail)
        {
            PlaceRailTile(x, y, currentPrefabIndex);
        } else if (currentCursorTool == CursorTools.Bulldozer)
        {
            BulldozeDistrictAndRoad(x, y);
        }else if (currentCursorTool == CursorTools.MetroStation)
        {
            PlaceMetroStation(x, y);
        }else if (transportManager.selectedLine == TransportManager.Lines.Metro && metroStationInCurrTile != null)
        {
            if (transportManager.selectedMetroStation != metroStationInCurrTile)
            {
                metroStationInCurrTile.ConnectToMetroStation(transportManager.selectedMetroStation);
                BuySomething(80000, x, y);
                transportManager.SetSelectedLineToNone();
            }
            
        }


    }
    public void MouseRightClicked(int x, int y)
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        transportManager.SetSelectedLineToNone();

        MetroStation metroStation = mapScript.map.GetMetroStationInTileIfExistsInTile(x, y);
        if (metroStation != null)
        {
            GameObject.FindGameObjectWithTag("_Manager").GetComponent<TransportManager>().selectedMetroStation = metroStation;
        }
        else
        {
            GameObject.FindGameObjectWithTag("_Manager").GetComponent<TransportManager>().selectedMetroStation = null;
        }
    }
    public void PlaceRailTile(int x, int y, int prefabIndex)
    {
        
        if (mapScript.map.houseTiles[x,y] == null || RailTileIsCompatibleWithDistrict(1,1,1,1)) {
            if (BuySomething(31000, x, y)) // If buying works then buy and continue
            {
                Tile currTile = new Tile(x, y, Map.Type.District, railTilePrefabs[prefabIndex], cursorRot, Map.InfrastructureType.Rail);
                currTile.prefabId = prefabIndex;
                mapScript.map.backgroundTiles[x, y].tileType = Map.Type.District;
                mapScript.map.roadTiles[x, y] = currTile;
                mapScript.map.tileToRedraw[x, y] = true;
                return;
            }
        }
        else
        {
            if (BuySomething(31000, x, y)) // If buying works then buy and continue
            {
                Tile currTile = new Tile(x, y, Map.Type.District, railTilePrefabs[prefabIndex], cursorRot, Map.InfrastructureType.Rail);
                currTile.prefabId = prefabIndex;
                mapScript.map.backgroundTiles[x, y].tileType = Map.Type.District;
                mapScript.map.houseTiles[x, y].rotation = cursorRot;
                mapScript.map.roadTiles[x, y] = currTile;
                mapScript.map.tileToRedraw[x, y] = true;
                return;
            }
        }
    }
    public void BulldozeDistrictAndRoad(int x, int y)
    {
        if(mapScript.drawnObjects[x, y, 2] != null && mapScript.drawnObjects[x, y, 1] != null)
        {
            if(BuySomething(250000, x, y))
            {
                District district = mapScript.map.GetDistrictByCoordinates(x, y);
                foreach(Coordinates tileLocation in district.tileLocations)
                {
                    if(tileLocation.x == x && tileLocation.y == y)
                    {
                        district.tileLocations.Remove(tileLocation);
                    }
                }

                GameObject.Destroy(mapScript.drawnObjects[x, y, 2].gameObject); //CONTENT
                GameObject.Destroy(mapScript.drawnObjects[x, y, 1].gameObject); //ROADS
                mapScript.map.houseTiles[x, y] = null;
                return;
            }
        }
        else
        {
            TextAtCoordinates("Nothing to bulldoze", x, y);
        }
        
    }
    public void TextAtCoordinates(string text, int x, int y)
    {
        Transform moneyTag = Instantiate(buyTextPrefab, new Vector3(x * mapScript.tileWidth, y * mapScript.tileHeight, 0), Quaternion.Euler(0f, 0f, 0f));
        moneyTag.GetComponent<TextMesh>().text = text;
    }
    public void SetCursorType(int cursorType)
    {
        currentCursorTool = (CursorTools)cursorType;
    }
    public bool BuySomething(int price)
    {
        if (moneyManager.moneyAmount - price > 0)
        {
            moneyManager.moneyAmount -= price;
            return true;
        }
        return false;
    }
    public bool BuySomething(int price, int x, int y)
    {
        if(moneyManager.moneyAmount - price > 0)
        {
            Transform moneyTag = Instantiate(buyTextPrefab, new Vector3(x * mapScript.tileWidth, y * mapScript.tileHeight, 0), Quaternion.Euler(0f, 0f, 0f));
            moneyTag.SetPositionAndRotation(new Vector3(x * mapScript.tileWidth, y * mapScript.tileHeight, 0), Quaternion.Euler(0f, 0f, 0f));
            moneyTag.GetComponent<TextMesh>().text = "-" + price + "mk";
            moneyManager.moneyAmount -= price;
            return true;
        }

        TextAtCoordinates("Not enough money!", x, y);
        return false;
    }
    private bool RailTileIsCompatibleWithDistrict(int districtRotation, int districtLevel, int railRotation, int railPrefab)
    {
        return false;
    }
    void UpdateRotation()
    {
        if (Input.GetKey("1") && keyIsUp)
        {
            keyIsUp = false;
            currentCursorTool = CursorTools.None;
        }
        if (Input.GetKey("2") && keyIsUp)
        {
            keyIsUp = false;
            currentCursorTool = CursorTools.Rail;
        }
        if (Input.GetKey("3") && keyIsUp)
        {
            keyIsUp = false;
            currentCursorTool = CursorTools.Bulldozer;
        }



        if (Input.GetKey("r") && keyIsUp)
        {
            keyIsUp = false;
            currentPrefabIndex++;
            if (currentPrefabIndex > 3)
            {
                currentPrefabIndex = 0;
            }
        }

        if (Input.GetKey("e") && keyIsUp)
        {
            keyIsUp = false;

            cursorRot--;
            if (cursorRot < 1)
            {
                cursorRot = 6;
            }
            SetCursorPos(cursorX, cursorY);
        }
        else if (Input.GetKey("q") && keyIsUp)
        {
            keyIsUp = false;

            cursorRot++;
            if (cursorRot > 6)
            {
                cursorRot = 1;
            }
            SetCursorPos(cursorX, cursorY);
        }

        if (!Input.GetKey("e") && !Input.GetKey("q") && !Input.GetKey("r") && !Input.GetKey("1") && !Input.GetKey("2") && !Input.GetKey("3"))
        {
            keyIsUp = true;
        }
    }
    void PlaceMetroStation(int x, int y)
    {
        if (mapScript.map.houseTiles[x, y] != null && mapScript.map.GetMetroStationInTileIfExistsInTile(x,y) == null)
        {
            if (BuySomething(60000, x, y)) // If buying works then buy and continue
            {
                Tile currTile = new Tile(x, y, Map.Type.District, metroStation, cursorRot, Map.InfrastructureType.Rail);
                District district = mapScript.map.GetDistrictByCoordinates(x, y);
                mapScript.map.metroStations.Add(new MetroStation(x, y, district));
                mapScript.map.tileToRedraw[x, y] = true;
            }
        }
        else
        {
            TextAtCoordinates("Metro Station Must Be in District",x,y);
        }
    }
    Transform DrawCursor(int prefabIndex)
    {
        int x = cursorX;
        int y = cursorY;

        if(currentCursorTool == CursorTools.Bulldozer)
        {
            if (y % 2 == 0)
            {
                cursor = Instantiate(bulldozer, new Vector3(x * mapScript.tileWidth, y * mapScript.tileHeight, 0), Quaternion.Euler(0f, 0f, cursorRot * 60));
            }
            else
            {
                cursor = Instantiate(bulldozer, new Vector3(x * mapScript.tileWidth + mapScript.tileWidth / 2, y * mapScript.tileHeight, 0), Quaternion.Euler(0f, 0f, cursorRot * 60));
            }
        }else if(currentCursorTool == CursorTools.Rail)
        {
            if (y % 2 == 0)
            {
                cursor = Instantiate(railTilePrefabs[prefabIndex], new Vector3(x * mapScript.tileWidth, y * mapScript.tileHeight, 0), Quaternion.Euler(0f, 0f, cursorRot * 60));
            }
            else
            {
                cursor = Instantiate(railTilePrefabs[prefabIndex], new Vector3(x * mapScript.tileWidth + mapScript.tileWidth / 2, y * mapScript.tileHeight, 0), Quaternion.Euler(0f, 0f, cursorRot * 60));
            }
        }
        else if(currentCursorTool == CursorTools.MetroStation)
        {
            if (y % 2 == 0)
            {
                cursor = Instantiate(metroStation, new Vector3(x * mapScript.tileWidth, y * mapScript.tileHeight, 0), Quaternion.Euler(0f, 0f, cursorRot * 60));
            }
            else
            {
                cursor = Instantiate(metroStation, new Vector3(x * mapScript.tileWidth + mapScript.tileWidth / 2, y * mapScript.tileHeight, 0), Quaternion.Euler(0f, 0f, cursorRot * 60));
            }
        }

        
        cursor.SetParent(transform);

        return cursor;
    }
}
