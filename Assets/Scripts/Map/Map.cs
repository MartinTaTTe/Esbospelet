using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map
{
    public int mapWidth;
    public int mapHeight;
    public MapScript mapScript;
    static int nPlayers;
    static int nFacilities;
    int initialDistrictSize;
    int[] generatorSettings;
    int districtAmount;

    public bool[,] tileToRedraw; //Tiles With true are redrawn


    public Tile[,] backgroundTiles;
    public Tile[,] roadTiles;
    public Tile[,] houseTiles;
    public Tile[,] resourceTiles;

    public District[] districts;
    public Facility[] facilities;
    public List<MetroStation> metroStations = new List<MetroStation>();

    public Transform metroStation;//prefab
    public Transform[] tilePrefab = new Transform[] {
            Resources.Load<Transform>("Prefabs/Background/hex_grass_1")
        };
    public Transform[] waterTilePrefabs = new Transform[] {
            Resources.Load<Transform>("Prefabs/Background/hex_water_0"),
            Resources.Load<Transform>("Prefabs/Background/hex_water_1"),
            Resources.Load<Transform>("Prefabs/Background/hex_water_2"),
        };
    public Transform[] districtTilePrefabs = new Transform[] {
            Resources.Load<Transform>("Prefabs/Infrastructure/Roads/tile_road_1"),
            Resources.Load<Transform>("Prefabs/Infrastructure/Roads/tile_road_2"),
            Resources.Load<Transform>("Prefabs/Infrastructure/Roads/tile_road_3"),
            Resources.Load<Transform>("Prefabs/Infrastructure/Roads/tile_road_4"),
            Resources.Load<Transform>("Prefabs/Infrastructure/Roads/tile_road_5"),
            Resources.Load<Transform>("Prefabs/Infrastructure/Roads/tile_road_6"),
            Resources.Load<Transform>("Prefabs/Infrastructure/Roads/tile_road_7"),
            Resources.Load<Transform>("Prefabs/Infrastructure/Roads/tile_road_8"),
            Resources.Load<Transform>("Prefabs/Infrastructure/Roads/tile_road_9"),
            Resources.Load<Transform>("Prefabs/Infrastructure/Roads/tile_road_10"),
            Resources.Load<Transform>("Prefabs/Infrastructure/Roads/tile_road_11"),
            Resources.Load<Transform>("Prefabs/Infrastructure/Roads/tile_road_12"),
            Resources.Load<Transform>("Prefabs/Infrastructure/Roads/tile_road_13"),
        };
    public Transform[] contentTilePrefabs = new Transform[]
    {
        Resources.Load<Transform>("Prefabs/Content/tile_residents_level_1"),
        Resources.Load<Transform>("Prefabs/Content/tile_residents_level_2"),
        Resources.Load<Transform>("Prefabs/Content/tile_residents_level_3"),
        Resources.Load<Transform>("Prefabs/Content/tile_residents_level_4"),
        Resources.Load<Transform>("Prefabs/Content/tile_residents_level_5"),
        Resources.Load<Transform>("Prefabs/Content/tile_factory")
    };
    public Transform[] facilityTilePrefabs = new Transform[]
    {
        Resources.Load<Transform>("Prefabs/Content/tile_mine"),
        Resources.Load<Transform>("Prefabs/Content/tile_farm"),
        Resources.Load<Transform>("Prefabs/Content/tile_oil"),
        Resources.Load<Transform>("Prefabs/Content/tile_fish")
    };

    //index börjar i linjen 30 grader från översta hörnet åt höger
    public int[][] roadConnections = new int[][]{
        new int[]{0,1,1,0,0,0 }, // hex_district_1
        new int[]{0,1,0,1,0,0 }, // hex_district_2
        new int[]{1,0,0,1,0,0 }, // hex_district_3
        new int[]{0,0,1,1,1,0 }, // hex_district_4
        new int[]{0,1,0,1,1,0 }, // hex_district_5
        new int[]{1,1,0,1,0,0 }, // hex_district_6
        new int[]{1,1,1,0,0,1 }, // hex_district_7
        new int[]{0,1,1,1,0,1 }, // hex_district_8
        new int[]{0,1,1,1,1,1 }, // hex_district_9
        new int[]{1,1,1,1,1,1 }, // hex_district_10
        new int[]{1,0,1,0,1,0 }, // hex_district_11
        new int[]{1,1,0,1,1,0 }, // hex_district_12
        new int[]{0,0,0,1,0,0 }, // hex_district_13
        };
    //index börjar i linjen 30 grader från översta hörnet åt höger
    public int[][] pathConnections = new int[][]{
        new int[]{0,0,0,0,1,0 }, // hex_district_0
        new int[]{0,0,0,0,0,0 }, // hex_district_1
        new int[]{0,0,0,0,1,0 }, // hex_district_2
        new int[]{0,0,0,0,0,0 }, // hex_district_3
        new int[]{0,0,0,0,0,0 }, // hex_district_4
        new int[]{0,0,0,0,0,0 }, // hex_district_5
        new int[]{0,1,0,0,0,1 }, // hex_district_6
        };
    //index börjar från 0 grader uppifrån och fortsätter åt höger i hörn
    public int[][] forestConnections = new int[][]{
        new int[]{0,0,0,1,0,0 }, // hex_district_0
        new int[]{0,1,1,0,0,0 }, // hex_district_1
        new int[]{0,1,1,0,0,0 }, // hex_district_2
        new int[]{0,0,0,0,0,0 }, // hex_district_3
        new int[]{0,0,0,0,0,0 }, // hex_district_4
        new int[]{0,0,1,1,0,1 }, // hex_district_5
        new int[]{1,0,0,0,0,0 }, // hex_district_6
        };

    public enum TileDirection
    {
        Left,
        Right,
        UpLeft,
        UpRight,
        DownLeft,
        DownRight
    }
    public enum Type
    {
        Grass,
        Forest,
        District,
        Resource,
        Water
    }
    public enum InfrastructureType
    {
        None,
        Road,
        Rail
    }
    public enum ContentTileType
    {
        Residence_lvl_1,
        Residence_lvl_2,
        Residence_lvl_3,
        Residence_lvl_4,
        Residence_lvl_5,
        Factory
    }
    public enum GeneratorSettings
    {
        ShoreRadicality,
        BigDistricts,
        MapWidth,
        MapHeight,
        DistrictSize,
        ResourceAmount
    }

    public Map(int[] generatorSettings, MapScript mapScript)
    {
        metroStation = Resources.Load<Transform>("Prefabs/Infrastructure/MetroStation");

        this.mapScript = mapScript;
        this.generatorSettings = generatorSettings;

        mapHeight = generatorSettings[(int)GeneratorSettings.MapHeight];
        mapWidth = generatorSettings[(int)GeneratorSettings.MapWidth];
        nPlayers = generatorSettings[(int)GeneratorSettings.BigDistricts];
        initialDistrictSize = generatorSettings[(int)GeneratorSettings.DistrictSize];
        nFacilities = generatorSettings[(int)GeneratorSettings.ResourceAmount]; ;

        District.SetNameArrayToFalse();
        districts = new District[nPlayers];
        facilities = new Facility[nFacilities];

        backgroundTiles = new Tile[mapWidth, mapHeight];
        roadTiles = new Tile[mapWidth, mapHeight];
        houseTiles = new Tile[mapWidth, mapHeight];
        resourceTiles = new Tile[mapWidth, mapHeight];

        tileToRedraw = new bool[mapWidth, mapHeight];

        GenerateMap();
    }

    public void GenerateMap()
    {
        GenerateGrass();
        GenerateSea();
        GenerateDistricts(nPlayers);
        GenerateHouses();
        GenerateFacilities(nFacilities);
    }
    public void GenerateGrass()
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                backgroundTiles[x, y] = new Tile(x, y, Type.Grass, tilePrefab[(int)Type.Grass], 0, InfrastructureType.None);
            }
        }
    }
    public void GenerateSea()
    {
        int shoreRadicality = generatorSettings[(int)GeneratorSettings.ShoreRadicality];
        int coastBoarderHeight = mapHeight / 4;
        int y = Random.Range(0, coastBoarderHeight);

        for (int x = 0; x < mapWidth; x++)
        {
            for (int i = y; i >= 0; i--)
            {
                backgroundTiles[x, i].tileType = Type.Water;
                backgroundTiles[x, i].transform = GetPrefab(Type.Water);
            }
            bool genAgain;
            do
            {
                genAgain = false;
                int a = Random.Range(-shoreRadicality, shoreRadicality);
                y += a;
                if (y > coastBoarderHeight || y < 0)
                {
                    y -= a;
                    genAgain = true;
                }
            } while (genAgain);
        }
    }
    public void GenerateDistricts(int districtAmount)
    {
        this.districtAmount = districtAmount;

        for (int i = 0; i < districtAmount; i++)
        {
            int rndX;
            int rndY;
            bool canBuild = false;
            do
            {
                rndX = Random.Range(i * (mapWidth / districtAmount), (i + 1) * (mapWidth / districtAmount));
                rndY = Random.Range(0, mapHeight);
                canBuild = backgroundTiles[rndX, rndY].tileType == Type.Grass;

                districts[i] = new District(i);//i is for randomSeed

                if (canBuild)
                {
                    int crashStop = 0;
                    int districtTiles = 0;
                    

                    while (districtTiles < initialDistrictSize)
                    {
                        Coordinates[] tileNeighbors = new Coordinates[6];
                        int rnd;
                        if (districtTiles == 0)
                        {
                            tileNeighbors = ListAdjacentCoordinates(rndX, rndY);
                        }
                        else
                        {
                            rnd = Random.Range(0, districts[i].tileLocations.Count);
                            tileNeighbors = ListAdjacentCoordinates(districts[i].tileLocations[rnd].x, districts[i].tileLocations[rnd].y);
                        }

                        //Neighbor location
                        Coordinates neighborCoords = tileNeighbors[Random.Range(0, 5)];

                        if (neighborCoords.x < mapWidth && neighborCoords.x >= 0 && neighborCoords.y < mapHeight && neighborCoords.y >= 0)
                        {
                            //Random neighborTile That will become a district
                            Tile neighborBackgroundTile = backgroundTiles[neighborCoords.x, neighborCoords.y];
                            Tile neighborRoadTile = roadTiles[neighborCoords.x, neighborCoords.y];

                            if (neighborBackgroundTile.tileType == Type.Grass)
                            {
                                if (districtTiles == 0)
                                {
                                    districts[i].tileLocations.Add(new Coordinates(rndX, rndY));
                                }
                                districtTiles++;
                                SetDistrictTile(neighborCoords.x, neighborCoords.y, districts[i]);
                            }
                        }
                        if(crashStop > 50000)
                        {
                            break;
                        }
                        crashStop++;
                    }
                }
            } while (!canBuild);
            int rnd1 = Random.Range(0, initialDistrictSize);
            int rnd2;
            do
            {
                rnd2 = Random.Range(0, initialDistrictSize);
            } while (rnd1 == rnd2);
            districts[i].factoryLocations.Add(districts[i].tileLocations[rnd1]);
            districts[i].factoryLocations.Add(districts[i].tileLocations[rnd2]);
        }
    }
    public void GenerateHouses()
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                if (roadTiles[x, y] != null) {

                    int houseLevel = Random.Range(0, 4); ; //0-4

                    houseTiles[x, y] = new Tile(x, y, Type.District, contentTilePrefabs[houseLevel], roadTiles[x, y].rotation, InfrastructureType.None);
                    switch (houseLevel)
                    {
                        case 0:
                            houseTiles[x, y].SetResourceType(ContentTileType.Residence_lvl_1);
                            break;
                        case 1:
                            houseTiles[x, y].SetResourceType(ContentTileType.Residence_lvl_2);
                            break;
                        case 2:
                            houseTiles[x, y].SetResourceType(ContentTileType.Residence_lvl_3);
                            break;
                        case 3:
                            houseTiles[x, y].SetResourceType(ContentTileType.Residence_lvl_4);
                            break;
                        case 4:
                            houseTiles[x, y].SetResourceType(ContentTileType.Residence_lvl_5);
                            break;
                    }
                }
            }
        }
    }
    public void GenerateFacilities(int nFacilities)
    {
        int x;
        int y;
        int type;
        bool spotFound;
        Coordinates coords;

        for (int i = 0; i < nFacilities; i++)
        {
            do
            {
                spotFound = false;

                x = Random.Range(0, mapWidth);
                y = Random.Range(0, mapHeight);
                type = Random.Range(0, 2);
                coords = new Coordinates(x, y);

                if (houseTiles[x, y] == null && resourceTiles[x, y] == null)
                {
                    if (backgroundTiles[x, y].tileType != Type.Water)
                    {
                        resourceTiles[x, y] = new Tile(coords.x, coords.y, Type.Resource, facilityTilePrefabs[type]);
                        Debug.Log(type);
                        facilities[i] = new Facility(coords, (Facility.FacilityType)type);
                        if (facilities[i].type == Facility.FacilityType.Other)
                        {
                            facilities[i].otherType = Facility.OtherContent.Oil;
                        }
                        spotFound = true;
                    }
                    else
                    {
                        resourceTiles[x, y] = new Tile(coords.x, coords.y, Type.Resource, facilityTilePrefabs[3]);
                        facilities[i] = new Facility(coords, Facility.FacilityType.Other)
                        {
                            otherType = Facility.OtherContent.Fish
                        };
                    }
                }
            } while (!spotFound);
        }
    }

    public void OneDistrictStep()
    {
        int i = Random.Range(0, districtAmount - 1);
        int rnd = Random.Range(0, districts[i].tileLocations.Count);
        Coordinates[]  tileNeighbors = ListAdjacentCoordinates(districts[i].tileLocations[rnd].x, districts[i].tileLocations[rnd].y);

        //Neighbor location
        Coordinates neighborCoords = tileNeighbors[Random.Range(0, 5)];

        if (neighborCoords.x < mapWidth && neighborCoords.x >= 0 && neighborCoords.y < mapHeight && neighborCoords.y >= 0)
        {
            //Random neighborTile That will become a district
            Tile neighborBackgroundTile = backgroundTiles[neighborCoords.x, neighborCoords.y];
            Tile neighborRoadTile = roadTiles[neighborCoords.x, neighborCoords.y];

            if (neighborBackgroundTile.tileType == Type.Grass)
            {
                //Debug.Log("GENERATING!"+ neighborCoords.x + "  " + neighborCoords.y +"  "+ districts[i]);
                SetDistrictTile(neighborCoords.x, neighborCoords.y, districts[i]);
                tileToRedraw[neighborCoords.x, neighborCoords.y] = true;
            }
        }
    }

    public void SetDistrictTile(int districtX, int districtY, District district)
    {
        for (int tries = 0; tries < districtTilePrefabs.Length * 3; tries++) { // Try 3 times the amount of district tiles

            int randomTilePrefabId = Random.Range(0, districtTilePrefabs.Length);
            int rotation = Random.Range(0, 5);

            for (int daw = 0; daw < 6; daw++) {
                if (TileIsCompatible(districtX, districtY, districtTilePrefabs[randomTilePrefabId], randomTilePrefabId, rotation))
                {
                    Tile currTile = new Tile(districtX, districtY, Type.District, districtTilePrefabs[randomTilePrefabId], rotation, InfrastructureType.Road)
                    {
                        prefabId = randomTilePrefabId
                    };
                    backgroundTiles[districtX, districtY].tileType = Type.District;
                    roadTiles[districtX, districtY] = currTile;

                    houseTiles[districtX, districtY] = new Tile(districtX, districtY, Type.District, contentTilePrefabs[1], roadTiles[districtX, districtY].rotation, InfrastructureType.None);
                    houseTiles[districtX, districtY].SetResourceType(ContentTileType.Residence_lvl_1);
                    district.tileLocations.Add(new Coordinates(districtX, districtY));
                }
                else
                {
                    rotation++;
                }
            }
        }
    }
    public bool TileIsCompatible(int districtX, int districtY, Transform districtTilePrefab, int prefabId, int rotation)
    {
        Coordinates[] adjacentCoordinates = ListAdjacentCoordinates(districtX, districtY);

        //Do again
        for(int a = 0; a < 6; a++) {
            if (adjacentCoordinates[a].x < mapWidth - 1 && adjacentCoordinates[a].x > 0 && adjacentCoordinates[a].y < mapHeight - 1 && adjacentCoordinates[a].y > 0)
            {

                Tile adjRoadTile = roadTiles[adjacentCoordinates[a].x, adjacentCoordinates[a].y];
                if (adjRoadTile != null)
                {
                    int currTileConnectionArrIndex = rotation + a;
                    int adjTileConnectionArrIndex = adjRoadTile.rotation + 3 + a;

                    int currTileConnection = roadConnections[prefabId][currTileConnectionArrIndex % 6];
                    int adjTileConnection = roadConnections[adjRoadTile.prefabId][adjTileConnectionArrIndex % 6];

                    if (currTileConnection != adjTileConnection)
                    {
                        return false;
                    }
                }
            }
        }
        
        return true;
    }
    //Returns a list of coordinates in right order starting from UP Right
    public Coordinates[] ListAdjacentCoordinates(int x, int y)
    {
        Coordinates[] list;

        if (y % 2 == 0)
        {
            list = new Coordinates[]{ //Main
                new Coordinates(x,y+1),
                new Coordinates(x+1,y),
                new Coordinates(x,y-1),
                new Coordinates(x-1,y-1),
                new Coordinates(x-1,y),
                new Coordinates(x-1,y+1),
            };
        }
        else
        {
            list = new Coordinates[]{ //Main
                new Coordinates(x+1,y+1),
                new Coordinates(x+1,y),
                new Coordinates(x+1,y-1),
                new Coordinates(x,y-1),
                new Coordinates(x-1,y),
                new Coordinates(x,y+1),
            };
        }
        
        return list;
    }
    public Transform GetPrefab(Type tileType)
    {
        if (tileType == Type.Water)
        {
            //Randomize Water Tiles
            return waterTilePrefabs[Random.Range(0, waterTilePrefabs.Length)];

        }
        else if (tileType == Type.District)
        {
            return districtTilePrefabs[0]; //Default District Tile
        }
        else
        {
            return tilePrefab[(int)tileType];
        }
    }
    public District GetDistrictByCoordinates(int x, int y)
    {
        foreach(District district in districts)
        {
            foreach (Coordinates coords in district.tileLocations)
            {
                if(coords.x == x && coords.y == y)
                {
                    return district;
                }
            }
        }
        Debug.Log("ERROR: No District With coordinates: x=" + x + " y=" + y);
        return null;
    }
    public MetroStation GetMetroStationInTileIfExistsInTile(int x, int y)
    {
        foreach (MetroStation metroStation in metroStations)
        {
            if(metroStation.x == x && metroStation.y == y)
            {
                return metroStation;
            }
        }
        return null;
    }
}

public struct Coordinates
{
    public Coordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
    public int x;
    public int y;
}
