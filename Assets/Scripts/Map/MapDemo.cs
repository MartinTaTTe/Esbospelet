using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDemo
{
    public int mapWidth;
    public int mapHeight;
    static int nPlayers;
    int initialDistrictSize;
    float oldTime = 0;
    int[] generatorSettings;
    int districtTiles = 0;

    public bool[,] tileToRedraw; //Tiles With true are redrawn


    public Tile[,] backgroundTiles;
    public Tile[,] roadTiles;
    public Tile[,] houseTiles;

    public District[] districts;

    public Transform[] tilePrefab = new Transform[] {
            Resources.Load<Transform>("Prefabs/Background/hex_grass_1"),
            Resources.Load<Transform>("Prefabs/Background/hex_grass_1"),
            Resources.Load<Transform>("Prefabs/Background/hex_grass_1"),
            Resources.Load<Transform>("Prefabs/Background/hex_grass_1"),
            Resources.Load<Transform>("Prefabs/Background/hex_grass_1"),
            Resources.Load<Transform>("Prefabs/Background/hex_grass_1"),
            Resources.Load<Transform>("Prefabs/Background/hex_grass_1"),
            Resources.Load<Transform>("Prefabs/Background/hex_grass_1"),
            Resources.Load<Transform>("Prefabs/Background/hex_grass_1"),
            Resources.Load<Transform>("Prefabs/Background/hex_grass_1"),
            Resources.Load<Transform>("Prefabs/Background/hex_grass_1"),
            Resources.Load<Transform>("Tiles/hex_district_1"),
            Resources.Load<Transform>("Prefabs/Background/hex_water_0"),
            Resources.Load<Transform>("Prefabs/Background/hex_water_1"),
            Resources.Load<Transform>("Prefabs/Background/hex_water_2"),
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
        Resources.Load<Transform>("Prefabs/Content/tile_residents_level_0"),
        Resources.Load<Transform>("Prefabs/Content/tile_residents_level_1"),
        Resources.Load<Transform>("Prefabs/Content/tile_residents_level_2"),
        Resources.Load<Transform>("Prefabs/Content/tile_residents_level_3"),
        Resources.Load<Transform>("Prefabs/Content/tile_residents_level_4")
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
        Farm,
        CoalMine,
        IronMine,
        OilWell,
        SteelMill,
        Refinery,
        PaperMill,
        PowerPlant,
        Factory,
        District,
        Water
    }
    public enum ContentTileType
    {
        Residence_lvl_1,
        Residence_lvl_2,
        Residence_lvl_3,
        Residence_lvl_4,
        Residence_lvl_5,

    }
    public enum ConnectionType
    {
        Road,
        Forest,
        Path
    }
    public enum GeneratorSettings
    {
        ShoreRadicality,
        BigDistricts,
        MapWidth,
        MapHeight,
        DistrictSize
    }

    public MapDemo(int[] generatorSettings)
    {
        this.generatorSettings = generatorSettings;

        mapHeight = generatorSettings[(int)GeneratorSettings.MapHeight];
        mapWidth = generatorSettings[(int)GeneratorSettings.MapWidth];
        nPlayers = generatorSettings[(int)GeneratorSettings.BigDistricts];
        initialDistrictSize = generatorSettings[(int)GeneratorSettings.DistrictSize];

        District.SetNameArrayToFalse();
        districts = new District[nPlayers];

        backgroundTiles = new Tile[mapWidth, mapHeight];
        roadTiles = new Tile[mapWidth, mapHeight];
        houseTiles = new Tile[mapWidth, mapHeight];

        tileToRedraw = new bool[mapWidth, mapHeight];

        GenerateMap();
    }

    public void GenerateMap()
    {
        districts[0] = new District(0);
        GenerateGrass();
        GenerateSea();
    }
    public void GenerateGrass()
    {
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                backgroundTiles[x, y] = new Tile(x, y, Map.Type.Grass, tilePrefab[(int)Type.Grass], 0, Map.InfrastructureType.None);
            }
        }
    }
    public void OneDistrictStep()
    {
        int rndX = 20;
        int rndY = 20;

        Coordinates[] tileNeighbors = new Coordinates[6];
        int rnd;
        if (districtTiles == 0)
        {
            tileNeighbors = ListAdjacentCoordinates(rndX, rndY);
        }
        else
        {
            rnd = Random.Range(0, districtTiles - 1);
            tileNeighbors = ListAdjacentCoordinates(districts[0].tileLocations[rnd].x, districts[0].tileLocations[rnd].y);
        }

        //Neighbor location
        Coordinates neighborCoords = tileNeighbors[Random.Range(0, 5)];

        if (neighborCoords.x < mapWidth && neighborCoords.x >= 0 && neighborCoords.y < mapHeight && neighborCoords.y >= 0)
        {
            //Random neighborTile That will become a district
            Tile neighborBackgroundTile = backgroundTiles[neighborCoords.x, neighborCoords.y];
            Tile neighborRoadTile = roadTiles[neighborCoords.x, neighborCoords.y];

            if (neighborBackgroundTile.tileType == Map.Type.Grass)
            {
                if (districtTiles == 0)
                {
                    districts[0].tileLocations.Add(new Coordinates(rndX, rndY));
                }
                districtTiles++;
                SetDistrictTile(neighborCoords.x, neighborCoords.y, districts[0]);

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
                backgroundTiles[x, i].tileType = Map.Type.Water;
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
    public void SetDistrictTile(int districtX, int districtY, District district)
    {
        for (int tries = 0; tries < districtTilePrefabs.Length * 3; tries++)
        { // Try 3 times the amount of district tiles

            int randomTilePrefabId = Random.Range(0, districtTilePrefabs.Length);
            int rotation = Random.Range(0, 5);

            for (int daw = 0; daw < 6; daw++)
            {
                if (TileIsCompatible(districtX, districtY, districtTilePrefabs[randomTilePrefabId], randomTilePrefabId, rotation))
                {
                    Tile currTile = new Tile(districtX, districtY, Map.Type.District, districtTilePrefabs[randomTilePrefabId], rotation, Map.InfrastructureType.Road);
                    currTile.prefabId = randomTilePrefabId;
                    backgroundTiles[districtX, districtY].tileType = Map.Type.District;
                    roadTiles[districtX, districtY] = currTile;
                    district.tileLocations.Add(new Coordinates(districtX, districtY));

                    int houseLevel = Random.Range(0, 4); ; //0-4
                    houseTiles[districtX, districtY] = new Tile(districtX, districtY, Map.Type.District, contentTilePrefabs[houseLevel], roadTiles[districtX, districtY].rotation, Map.InfrastructureType.None);
                    tileToRedraw[districtX, districtY] = true;
                    return;
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
        for (int a = 0; a < 6; a++)
        {
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
}