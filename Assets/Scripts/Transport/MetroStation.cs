using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetroStation
{
    public int x;
    public int y;
    public District isInDistrict;
    private List<MetroStation> metroConnections = new List<MetroStation>();

    public MetroStation(int x, int y, District district)
    {
        this.x = x;
        this.y = y;
        this.isInDistrict = district;
    }
    public void ConnectToMetroStation(MetroStation metroStation)
    {

        if(!metroConnections.Contains(metroStation) && !metroStation.metroConnections.Contains(this))
        {
            Debug.Log("METRO STATIONS: " + metroStation.GetHashCode() + " AND " + this.GetHashCode() + " ARE NOW CONNECTED!");
            //ADD CONNECTIONS TO BOTH STATIONS
            this.metroConnections.Add(metroStation);
            metroStation.metroConnections.Add(this);
        }
        else
        {
            GameObject.FindGameObjectWithTag("_Manager").GetComponent<Cursor>().TextAtCoordinates("Already Connected!", metroStation.x, metroStation.y);
            Debug.Log("METRO STATIONS: " + metroStation.GetHashCode() + " AND " + this.GetHashCode() + " ARE ALREADY CONNECTED!");
        }

        GameObject.FindGameObjectWithTag("MapTiles").GetComponent<MapScript>().ReDrawMetroConnections();
    }
    public void DisconnectFromMetroStation(MetroStation metroStation)
    {
        //REMOVE THE OTHER STATION FROM THIS STATION
        if (this.metroConnections.Contains(metroStation))
        {
            this.metroConnections.Remove(metroStation);
        }

        //REMOVE THIS STATION FROM THE OTHER STATION
        if (metroStation.metroConnections.Contains(metroStation))
        {
            metroStation.metroConnections.Remove(this);
        }

        GameObject.FindGameObjectWithTag("MapTiles").GetComponent<MapScript>().ReDrawMetroConnections();
    }
    public List<MetroStation> GetMetroConnections()
    {
        return metroConnections;
    }
}
