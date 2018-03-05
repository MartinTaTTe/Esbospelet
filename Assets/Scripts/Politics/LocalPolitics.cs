using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalPolitics : MonoBehaviour
{
    private int communistPoliticalPower = 150;
    Politician mayor;
    District[] districts;
    GameObject politicsCanvas;
    TimeManager timeManager;
    Transform politicalPowerText;
    Transform politicalPowerPMText;
    Politician[] democraticPoliticianPool = new Politician[15];
    Politician[] communisticPoliticianPool = new Politician[15];
    Politician inspectorSelectedPolitician;
    int dialogCooldown;

    bool firstrun = true;
    bool communalElectionIsOngoing = false;

    public enum Parties
    {
        DEM,
        KOM
    }
    Map map;


    private void Update()
    {
        dialogCooldown--;

        this.politicalPowerPMText.GetComponent<Text>().text = "PP/month: " + GetPoliticalPowerPerMonth(Parties.KOM);
        this.politicalPowerText.GetComponent<Text>().text = "Political Power: " + communistPoliticalPower;
    }
    /*
    * Started from MapScript after Map is created and drawn
    * GeneratePolitics(); Procedurally generates, draws and instantiates the class/politicians
    */
    [Command("regen-politics", "Make and draw new politicians.")]
    public void GeneratePolitics()
    {
        if (firstrun)
        {
            politicsCanvas = GameObject.FindGameObjectWithTag("PoliticsCanvas");
            this.timeManager = GameObject.FindGameObjectWithTag("_Manager").GetComponent<TimeManager>();
            firstrun = false;
        }

        this.map = GameObject.FindGameObjectWithTag("MapTiles").GetComponent<MapScript>().map;
        districts = map.districts;
        MakePoliticians();
        DrawPoliticians();

        SetPoliticsVisible(false);
    }

    /*
    * Generates random politicians that are placed in the global district Array
    */
    void MakePoliticians()
    {


        mayor = new Politician(Parties.DEM);

        int playerStartDistrict = Random.Range(0, this.districts.Length);

        int districtIndex = 0;
        foreach (District district in this.districts)
        {
            for (int i = district.localPoliticians.Length - 1; i >= 0; i--)
            {
                district.localPoliticians[i] = new Politician(Parties.DEM);
            }
            if (playerStartDistrict == districtIndex)
            {
                //We want to have one DEM politician in the district except if the starting district has only 2 politicians
                int polIndex = district.localPoliticians.Length - 2;
                if (district.localPoliticians.Length == 2)
                    polIndex++;

                for (int j = polIndex; j >= 0; j--)
                {
                    district.localPoliticians[j] = new Politician(Parties.KOM);
                }
            }
            districtIndex++;
        }

        for (int i = democraticPoliticianPool.Length - 1; i >= 0; i--)
        {
            democraticPoliticianPool[i] = new Politician(Parties.DEM);
        }
        for (int i = communisticPoliticianPool.Length - 1; i >= 0; i--)
        {
            communisticPoliticianPool[i] = new Politician(Parties.KOM);
        }
    }
    /*
    * Instantiates GameObjects of all the politicians made in MakePoliticians();
    */
    void DrawPoliticians()
    {

        //
        // Draw the Main politics district panel
        //

        Transform politicsPanel = politicsCanvas.transform.GetChild(0);
        Transform politicsPoolPanel = politicsCanvas.transform.GetChild(1);

        if (politicsPanel.childCount > 0)
        {
            for(int i = politicsPanel.childCount - 1; i >= 0; i--)
            {
                Destroy(politicsPanel.GetChild(i).gameObject);
            }
                
        }
        if (politicsPoolPanel.childCount > 0)
        {
            for (int i = politicsPoolPanel.childCount - 1; i >= 0; i--)
            {
                Destroy(politicsPoolPanel.GetChild(i).gameObject);
            }

        }

        //
        // DRAW THE POLITICAL POWER
        //
        string color = "Red"; // Player is communist
        this.politicalPowerText = GameObject.Instantiate(Resources.Load<Transform>("Prefabs/Political/" + color + "/" + color + "Mayor"), new Vector3(politicsPanel.position.x - 160, politicsPanel.position.y + 200), Quaternion.identity);
        this.politicalPowerText.SetPositionAndRotation(new Vector3(politicsPanel.position.x - 160, politicsPanel.position.y + 200), Quaternion.identity);
        this.politicalPowerText.SetParent(politicsCanvas.transform.GetChild(0));

        //
        // DRAW THE POLITICAL POWER PER MONTH
        //
        this.politicalPowerPMText = GameObject.Instantiate(Resources.Load<Transform>("Prefabs/Political/" + color + "/" + color + "Mayor"), new Vector3(politicsPanel.position.x - 330, politicsPanel.position.y + 200), Quaternion.identity);
        this.politicalPowerPMText.SetPositionAndRotation(new Vector3(politicsPanel.position.x - 330, politicsPanel.position.y + 200), Quaternion.identity);
        this.politicalPowerPMText.SetParent(politicsCanvas.transform.GetChild(0));
        
        //
        // DRAW THE MAYOR
        //
        if (mayor.politicalParty == Parties.DEM)
        {
            color = "Blue";
        }
        else
        {
            color = "Red";
        }

        Transform a = GameObject.Instantiate(Resources.Load<Transform>("Prefabs/Political/" + color + "/" + color + "Mayor"), new Vector3(politicsPanel.position.x, politicsPanel.position.y + 150), Quaternion.identity);
        a.SetPositionAndRotation(new Vector3(politicsPanel.position.x, politicsPanel.position.y + 150), Quaternion.identity);
        a.GetComponent<Text>().text = mayor.politicianName;
        a.SetParent(politicsPanel);


        //
        // DRAW POLITICIANS AND DISTRICT NAMES
        //
        int districtIndex = 0;
        int row2offset = 0;
        foreach (District district in this.districts)
        {
            if(districtIndex >= 5)
            {
                row2offset = 120;
                districtIndex = 0;
            }
            if (GetPartyMajorityInDistrict(district) == Parties.DEM)
            {
                color = "Blue";
            }
            else
            {
                color = "Red";
            }

            Transform c = GameObject.Instantiate(Resources.Load<Transform>("Prefabs/Political/" + color + "/" + color + "District"), new Vector3(politicsPanel.position.x - 300 + districtIndex * 150, politicsPanel.position.y + 80 - row2offset), Quaternion.identity);
            c.SetPositionAndRotation(new Vector3(politicsPanel.position.x - 300 + districtIndex * 150, politicsPanel.position.y + 80 - row2offset), Quaternion.identity);
            c.GetComponent<Text>().text = district.districtName;
            c.SetParent(politicsPanel);

            int politicianIndex = 0;
            foreach (Politician politician in district.localPoliticians)
            {
                if(politician.politicalParty == Parties.DEM)
                {
                    color = "Blue";
                }
                else
                {
                    color = "Red";
                }
                Transform b = GameObject.Instantiate(Resources.Load<Transform>("Prefabs/Political/" + color + "/" + color + "Politician"), new Vector3(politicsPanel.position.x - 300 + districtIndex * 150, politicsPanel.position.y + 60 - politicianIndex * 20 - row2offset), Quaternion.identity);
                b.SetPositionAndRotation(new Vector3(politicsPanel.position.x - 300 + districtIndex * 150, politicsPanel.position.y + 60 - politicianIndex * 20 - row2offset), Quaternion.identity);
                b.GetComponent<Text>().text = politician.politicianName;
                b.GetComponent<PoliticianTextScript>().politician = politician;
                b.GetComponent<PoliticianTextScript>().localPoliticsManager = this;
                b.SetParent(politicsPanel);
                politicianIndex++;
            }

            districtIndex++;
        }


        //
        // Draw the politician pools
        //

        int polIndex = 0;
        foreach (Politician politician in democraticPoliticianPool)
        {
            if (politician.politicalParty == Parties.DEM)
            {
                color = "Blue";
            }
            else
            {
                color = "Red";
            }
            Transform b = GameObject.Instantiate(Resources.Load<Transform>("Prefabs/Political/" + color + "/" + color + "Politician"), new Vector3(politicsPoolPanel.position.x - 110, politicsPoolPanel.position.y - 150 + polIndex * 25), Quaternion.identity);
            b.SetPositionAndRotation(new Vector3(politicsPoolPanel.position.x - 110, politicsPoolPanel.position.y - 150 + polIndex * 25), Quaternion.identity);
            b.GetComponent<PoliticianTextScript>().politician = politician;
            b.GetComponent<PoliticianTextScript>().localPoliticsManager = this;
            b.GetComponent<Text>().text = politician.politicianName;
            b.SetParent(politicsPoolPanel);
            polIndex++;
        }
        polIndex = 0;
        foreach (Politician politician in communisticPoliticianPool)
        {
            if (politician.politicalParty == Parties.DEM)
            {
                color = "Blue";
            }
            else
            {
                color = "Red";
            }
            Transform b = GameObject.Instantiate(Resources.Load<Transform>("Prefabs/Political/" + color + "/" + color + "Politician"), new Vector3(politicsPoolPanel.position.x + 70, politicsPoolPanel.position.y - 150 + polIndex * 25), Quaternion.identity);
            b.SetPositionAndRotation(new Vector3(politicsPoolPanel.position.x + 70, politicsPoolPanel.position.y - 150 + polIndex * 25), Quaternion.identity);
            b.GetComponent<PoliticianTextScript>().politician = politician;
            b.GetComponent<PoliticianTextScript>().localPoliticsManager = this;
            b.GetComponent<Text>().text = politician.politicianName;
            b.SetParent(politicsPoolPanel);
            polIndex++;
        }
    }

    /*
    * Returns the majority party of a district based on if there are more DEM or KOM politicians
    * If DEM == KOM then returns DEM
    */
    Parties GetPartyMajorityInDistrict(District district)
    {
        int blue = 0;
        int red = 0;

        foreach(Politician pol in district.localPoliticians)
        {
            if(pol.politicalParty == Parties.DEM)
            {
                blue++;
            }
            else
            {
                red++;
            }
        }
        if(blue < red)
        {
            return Parties.KOM;
        }
        else
        {
            return Parties.DEM;
        }
    }

    /*
    * Main Communal Election method that does everything needed for the communal election to start
    * (The politician changes etc. happen in EndCommunalElection(); a month after the start)
    */
    public void StartCommunalElection()
    {
        //ELECTION LASTS A MONTH
        communalElectionIsOngoing = true;
        timeManager.gameEventManager.MakeNewEvent(new GameEvent(timeManager.AddDaysToDate(timeManager.GetGameDate(), 30), GameEventTypes.communal_election_end,null,null));
    }

    /*
    * Calculates what politicians are elected and re-elected, also possibly does other things :D
    */
    public void EndCommunalElection()
    {
        communalElectionIsOngoing = false;

        //TODO IF WE HAVE MAJORITY IN A MAJORITY OF DISTRICTS --> ELECT KOM
        int p = Random.Range(0, this.districts.Length);
        mayor = this.districts[p].localPoliticians[Random.Range(0, this.districts[p].localPoliticians.Length)];

        foreach (District district in this.districts)
        {
            for (int i = district.localPoliticians.Length - 1; i >= 0; i--)
            {
                int random = Random.Range(0, 100);
                int opposingPartyPopularity = 0;

                if (random > 90 - opposingPartyPopularity)//(90% - other party popularity) chance that the politician is re-elected
                {//Replaced
                    int chance = Random.Range(0, 6);

                    Parties party = district.localPoliticians[i].politicalParty;
                    int chanceModifier = 0;

                    if(party == Parties.DEM)
                    {
                        chanceModifier = 4;
                    }
                    else
                    {
                        chanceModifier = 1;
                    }

                    if(chance >= chanceModifier)
                    {//Politician is changed to one that is on the opposing party
                        
                        if (district.localPoliticians[i].politicalParty == Parties.DEM)
                        {//If party of curr. politician is DEM exchange with a KOM in the pool
                            int randomPoolPolitician = Random.Range(0, communisticPoliticianPool.Length);
                            Politician toPoolPolitician = district.localPoliticians[i];
                            district.localPoliticians[i] = communisticPoliticianPool[randomPoolPolitician];
                            communisticPoliticianPool[randomPoolPolitician] = toPoolPolitician;
                        }
                        else
                        {
                            int randomPoolPolitician = Random.Range(0, democraticPoliticianPool.Length);
                            Politician toPoolPolitician = district.localPoliticians[i];
                            district.localPoliticians[i] = democraticPoliticianPool[randomPoolPolitician];
                            democraticPoliticianPool[randomPoolPolitician] = toPoolPolitician;
                        }
                    }
                    else
                    {//Politician is changed to one on the same party
                        if (district.localPoliticians[i].politicalParty == Parties.DEM)
                        {//If party of curr. politician is DEM exchange with a KOM in the pool
                            int randomPoolPolitician = Random.Range(0, democraticPoliticianPool.Length);
                            Politician toPoolPolitician = district.localPoliticians[i];
                            district.localPoliticians[i] = democraticPoliticianPool[randomPoolPolitician];
                            democraticPoliticianPool[randomPoolPolitician] = toPoolPolitician;
                        }
                        else
                        {
                            int randomPoolPolitician = Random.Range(0, communisticPoliticianPool.Length);
                            Politician toPoolPolitician = district.localPoliticians[i];
                            district.localPoliticians[i] = communisticPoliticianPool[randomPoolPolitician];
                            communisticPoliticianPool[randomPoolPolitician] = toPoolPolitician;
                        }
                    }
                }
            }
        }

        DrawPoliticians();
    }

    /*
    * Toggles the politics panel on and off
    */
    public void TogglePoliticsVisible()
    {
        politicsCanvas.SetActive(!politicsCanvas.activeSelf);
    }

    /*
    * Sets the politics panel on or off
    */
    public void SetPoliticsVisible(bool visible)
    {
        politicsCanvas.SetActive(visible);
    }

    public int GetPoliticalPowerPerMonth(Parties party)
    {
        return 20;
    }
    public void NewMonthListener()
    {
        communistPoliticalPower += GetPoliticalPowerPerMonth(Parties.KOM);
    }

    /*
    * Selects the politician in the PoliticianInspectorPanel when clicked(PoliticianTextScript)
    */
    public void SelectPolitician(Politician politician)
    {
        inspectorSelectedPolitician = politician;
        UpdateDialogTexts();
    }

    public void UpdateDialogTexts()
    {
        GameObject politicianInspector = GameObject.FindGameObjectWithTag("PolInspectorPanel");

        GameObject politicianText = politicianInspector.transform.GetChild(0).gameObject;
        Button supportButton = politicianInspector.transform.GetChild(1).gameObject.GetComponent<Button>();
        Button discreditButton = politicianInspector.transform.GetChild(2).gameObject.GetComponent<Button>();
        Button threatenButton = politicianInspector.transform.GetChild(3).gameObject.GetComponent<Button>();
        GameObject popularityText = politicianInspector.transform.GetChild(4).gameObject;
        GameObject controversyText = politicianInspector.transform.GetChild(5).gameObject;

        supportButton.onClick.AddListener(SupportPolitician);
        discreditButton.onClick.AddListener(DiscreditPolitician);
        threatenButton.onClick.AddListener(ThreatenPolitician);

        politicianText.GetComponent<Text>().text = inspectorSelectedPolitician.politicianName;
        popularityText.GetComponent<Text>().text = "Popularity: " + inspectorSelectedPolitician.popularity;
        controversyText.GetComponent<Text>().text = "Controversy: " + inspectorSelectedPolitician.controversy;

        if (inspectorSelectedPolitician.politicalParty == Parties.DEM)
        {
            politicianText.GetComponent<Text>().color = Color.blue;
        }
        else
        {
            politicianText.GetComponent<Text>().color = Color.red;
        }
    }

    public void ThreatenPolitician()
    {
        if (dialogCooldown > 0)
        {
            return;
        }
        else
        {
            dialogCooldown = 24;
        }

        //inspectorSelectedPolitician.
        UpdateDialogTexts();
    }
    public void DiscreditPolitician()
    {
        if(dialogCooldown > 0)
        {
            return;
        }
        else
        {
            dialogCooldown = 24;
        }

        if (communistPoliticalPower >= 60)
        {
            if (inspectorSelectedPolitician.controversy != 0)
            {
                inspectorSelectedPolitician.controversy += (int)(50 / inspectorSelectedPolitician.controversy);
            }
            else
            {
                inspectorSelectedPolitician.controversy += 8;
            }
            
            communistPoliticalPower -= 60;
        }
        UpdateDialogTexts();
    }
    public void SupportPolitician()
    {
        if (dialogCooldown > 0)
        {
            return;
        }
        else
        {
            dialogCooldown = 24;
        }

        if (communistPoliticalPower >= 40)
        {
            if(inspectorSelectedPolitician.popularity != 0)
            {
                inspectorSelectedPolitician.popularity += (int)(50 / inspectorSelectedPolitician.popularity);
            }
            else
            {
                inspectorSelectedPolitician.popularity += 8;
            }
            
            if(inspectorSelectedPolitician.controversy >= 5)
            {
                inspectorSelectedPolitician.controversy -= 5;
            }
            communistPoliticalPower -= 40;
        }
        UpdateDialogTexts();
    }

    [Command("set-political-power", "(politicalPower)")]
    public void DebugSetPoliticalPower(int politicalPower)
    {
        communistPoliticalPower = politicalPower;
    }
}
