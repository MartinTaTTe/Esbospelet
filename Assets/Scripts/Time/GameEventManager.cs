using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameEventManager
{
    public TimeManager timeManager;
    public LocalPolitics politicsManager;

    //The GameEvent queue
    public List<GameEvent> eventQueue = new List<GameEvent>();

    public GameEventManager(TimeManager timeManager)
    {
        this.timeManager = timeManager;
        this.politicsManager = GameObject.FindGameObjectWithTag("_Manager").GetComponent<LocalPolitics>();
    }
    
    /*
    * Checks all events in the eventQueue, if one is expired --> ExecuteEvent();
    */
    public void Tick()
    {
        GameDate currentGameDate = timeManager.GetGameDate();
        foreach (GameEvent gEvent in this.eventQueue.ToArray())
        {
            if(DateIsGreaterOrEqualThanDate(timeManager.GetGameDate(), gEvent.dateTime))
            {
                ExecuteEvent(gEvent);
                this.eventQueue.Remove(gEvent);
            }
        }
    }

    /*
    * Executes the eventflow caused by an expired event in the queue
    */
    public void ExecuteEvent(GameEvent gEvent)
    {
        timeManager.newsManager.QueueNews(GetEventNewsString(gEvent), gEvent.dateTime);

        switch (gEvent.eventType)
        {
            case (GameEventTypes.communal_election_start):
                politicsManager.StartCommunalElection();
                break;
            case (GameEventTypes.communal_election_end):
                politicsManager.EndCommunalElection();
                break;
            case (GameEventTypes.politician_assassinated):
                break;
            case (GameEventTypes.politician_change_party):
                break;
            case (GameEventTypes.politician_elected):
                break;
            case (GameEventTypes.politician_scandal):
                break;
        }
    }

    /*
    * Adds an event to the GameEvent queue
    */
    public void MakeNewEvent(GameEvent gEvent)
    {
        eventQueue.Add(gEvent);
    }

    /*
    * Returns the string that will be seen in the news article based on the GameEvent type and such
    */
    string GetEventNewsString(GameEvent gEvent)
    {
        string[] polParties = new string[50];//MAX 50 politicians in a news article

        int polNumber = 0;
        if (gEvent.involvedPoliticians != null) {
            foreach (Politician politician in gEvent.involvedPoliticians)
            {
                if (politician.politicalParty == LocalPolitics.Parties.DEM)
                {
                    polParties[polNumber] = "(DEM)";
                }
                else
                {
                    polParties[polNumber] = "(KOM)";
                }

                polNumber++;
            }
        }
        switch (gEvent.eventType){
            case (GameEventTypes.politician_scandal):
                return gEvent.involvedPoliticians[0].politicianName + polParties[0] + " Is involved in a scandal and is pressured to resign.";
            case (GameEventTypes.politician_assassinated):
                return gEvent.involvedPoliticians[0].politicianName + polParties[0] + " Has been assasinated by an unknown political entity.";
            case (GameEventTypes.politician_elected):
                return gEvent.involvedPoliticians[0].politicianName + polParties[0] + " Has elected to represent " + gEvent.involvedDistricts[0] + ".";
            case (GameEventTypes.politician_change_party):
                string newParty = "(DEM)";

                if (polParties[0] == "(DEM)")
                {
                    newParty = "(KOM)";
                }
                return gEvent.involvedPoliticians[0].politicianName + polParties[0] + " Has changed his political party status to: " + newParty;
            case (GameEventTypes.communal_election_start):
                return "A communal election is starting.";
            case (GameEventTypes.communal_election_end):
                return "A communal election has concluded.";
        }
        return "ERROR: NO GAME EVENT WITH THAT ENUM EXISTS";
    }

    /*
    * Returns boolean based on if date1 > date2
    */
    public bool DateIsGreaterOrEqualThanDate(GameDate date1, GameDate date2)
    {
        return date1.day + date1.monthNumber * 30 + date1.year * 30 * 12 >= date2.day + date2.monthNumber * 30 + date2.year * 30 * 12;
    }
}

/*
* The GameEvent struct
*/
public struct GameEvent
{
    public GameEvent(GameDate dateTime, GameEventTypes eventType, Politician[] involvedPoliticians, string[] involvedDistricts)
    {
        this.involvedDistricts = involvedDistricts;
        this.dateTime = dateTime;
        this.eventType = eventType;
        this.involvedPoliticians = involvedPoliticians;
    }
    public string[] involvedDistricts;
    public GameDate dateTime;
    public GameEventTypes eventType;
    public Politician[] involvedPoliticians;
}

public enum GameEventTypes
{
    politician_scandal,
    politician_assassinated,
    politician_elected,
    politician_change_party,
    communal_election_start,
    communal_election_end

}