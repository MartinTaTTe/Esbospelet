using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class District
{
    public List<Coordinates> tileLocations = new List<Coordinates>();
    public List<Coordinates> factoryLocations = new List<Coordinates>();

    public int nTiles;
    public int nFactories;
    public int nMetroStations;
    public string districtName;
    public Politician[] localPoliticians;
    public int happiness;
    int population;
    public int money;

    public enum Buildings
    {
        Residents,
        Industries,
        Administrative,
        TrainStation
    }

    static public string[] districtNamesArray = new string[]{
        //alfabetisk ordning, håll de så!
        "Alberga",
        "Albro",
        "Alkulla",
        "Bassebacken",
        "Bastunäset",
        "Bastvik",
        "Bemböle",
        "Bergans",
        "Biskopsby",
        "Björkberga",
        "Björkgård",
        "Björnkärr",
        "Björnängen",
        "Blombacken",
        "Blåbacka",
        "Bredstranden",
        "Bredvik",
        "Brukstranden",
        "Brunskog",
        "Dalsvik",
        "Distby",
        "Domsby",
        "Dyningen",
        "Dåvitsby",
        "Esboviken",
        "Estdalen",
        "Estmalmen",
        "Fantsby",
        "Finno",
        "Friherrs",
        "Frisans",
        "Frisbacka",
        "Fågelberga",
        "Fårsbacka",
        "Gammelgård",
        "Gillern",
        "Glasdalen",
        "Glashyttan",
        "Grani",
        "Grundbacka",
        "Gruvsta",
        "Gräsa",
        "Gröndal",
        "Grönkulla",
        "Gumböle",
        "Gunnars",
        "Gäddvik",
        "Hagalund",
        "Hanikka",
        "Hannus",
        "Hannusträsk",
        "Hasselbacken",
        "Hejnäs",
        "Helgakorset",
        "Hovgård",
        "Högnäs",
        "Irisvik",
        "Ivisnäs",
        "Jupper",
        "Kaitans",
        "Kaitbacken",
        "Kalajärvi",
        "Kallvik",
        "Karabacka",
        "Karlsberg",
        "Kavallbacken",
        "Kera",
        "Kilo",
        "Kitteldalen",
        "Klappedet",
        "Klobbskog",
        "Klovis",
        "Knektbro",
        "Konungsböle",
        "Korpilampi",
        "Kortesbacken",
        "Krokudden",
        "Kuckubacka",
        "Kurjenkaski",
        "Kurkijärvi",
        "Kurängen",
        "Kvisbacka",
        "Kyrkträsk",
        "Kägeludden",
        "Kägelviken",
        "Källstrand",
        "Köklax",
        "Ladusved",
        "Lahnus",
        "Lambertsängen",
        "Lansa",
        "Larsvik",
        "Lillmossen",
        "Luk",
        "Lukubäcken",
        "Lustikulla",
        "Långängen",
        "Mankans",
        "Marbäck",
        "Mattby",
        "Metsämaa",
        "Mickels",
        "Milkärr",
        "Musslax",
        "Myntböle",
        "Mäkkylä",
        "Mösskärr",
        "Nedergård",
        "Nedre Stensvik",
        "Neppers",
        "Nikubacken",
        "Nipert",
        "Nissbacken",
        "Nokkala",
        "Norra Hagalund",
        "Notudden",
        "Notviken",
        "Noux",
        "Nupurböle",
        "Nöykis",
        "odilampi",
        "Olarinmäki",
        "Olars",
        "Olarsbäcken",
        "Ovanmalm",
        "Pellas",
        "Påskägg",
        "Rilax",
        "Rosatorpet",
        "Råtorp",
        "Rödskog",
        "Rönnebacka",
        "Serviudden",
        "Sigurdsberg",
        "Siikajärvi",
        "Sillböle",
        "Smedsby",
        "Snettans",
        "Stenkarlen",
        "Stensvik",
        "Storkärr",
        "Storåker",
        "Sockenbacka",
        "Solhöjden",
        "Solsidan",
        "Sommaröarna",
        "Stadsberget",
        "Stenbruket",
        "Stensåsen",
        "Suna",
        "Sveins",
        "Sälviken",
        "Södrik",
        "Söilibacka",
        "Sökö",
        "Sökö Strand",
        "Sökö Sund",
        "Sökö Udd",
        "Tavasteberga",
        "Teknologbyn",
        "Tillisbacken",
        "Tollbacken",
        "Tomtekulla",
        "Trastmossen",
        "Trastparken",
        "Träskända",
        "Vallberget",
        "Vermobranten",
        "Vermo",
        "Videnäs",
        "Villnäs",
        "Viskärr",
        "Vällskog",
        "Westend",
        "Ymmersta",
        "Åminne",
        "Älgkärr",
        "Ängskulla",
        "Äspnäsudd",
        "Örkiängen",
        "Österstranden"
    };
    static public string[] districtNamePrefix = new string[]
    {
        "Norra ",
        "Södra ",
        "Västra ",
        "Östra ",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
        "",
    };
    static public string[] districtNameMiddle = new string[]
    {
        "Al",
        "Basse",
        "Bastu",
        "Bast",
        "Bergans",
        "Biskops",
        "Björk",
        "Björn",
        "Blom",
        "Blå",
        "Bred",
        "Bruk",
        "Brun",
        "Dals",
        "Dist",
        "Doms",
        "Dåvits",
        "Esbo",
        "Est",
        "Fants",
        "Fris",
        "Fågel",
        "Fårs",
        "Gammel",
        "Glas",
        "Grund",
        "Grön",
        "Gum",
        "Gädd",
        "Hag",
        "Hannus",
        "Hassel",
        "Hej",
        "Helga",
        "Hov",
        "Hög",
        "Iris",
        "Ivis",
        "Kait",
        "Kala",
        "Kall",
        "Kara",
        "Karls",
        "Kavall",
        "Kittel",
        "Klapp",
        "Klobb",
        "Knekt",
        "Konungs",
        "Korpi",
        "Kortes",
        "Krok",
        "Kucku",
        "Kurki",
        "Kur",
        "Kvis",
        "Kyrk",
        "Kägel",
        "Käll",
        "Kök",
        "Lamberts",
        "Lars",
        "Lill",
        "Luku",
        "Lusti",
        "Lång",
        "Mar",
        "Matt",
        "Skogs",
        "Mil",
        "Mynt",
        "Möss",
        "Neder",
        "Stens",
        "Niku",
        "Niss",
        "Nokkala",
        "Haga",
        "Not",
        "Nupur",
        "Olars",
        "Ovan",
        "Rosa",
        "Rå",
        "Röd",
        "Rönne",
        "Sigurds",
        "Siika",
        "Sill",
        "Smeds",
        "Sten",
        "Stor",
        "Socken",
        "Sol",
        "Sommar",
        "Stads",
        "Sten",
        "Säl",
        "Tavaste",
        "Teknolog",
        "Tillis",
        "Toll",
        "Tomte",
        "Trast",
        "Träsk",
        "Vall",
        "Vermo",
        "Vide",
        "Vill",
        "Vis",
        "Väll",
        "Älg",
        "Ängs",
        "Äspnäs",
        "Örki"
    };
    static public string[] districtNameSuffix = new string[]
    {
        "gård",
        "backa",
        "dalen",
        "bro",
        "lampi",
        "järvi",
        "träsk",
        "bäcken",
        "malm",
        "lax",
        "torpet",
        "torp",
        "skog",
        "udden",
        "berg",
        "järvi",
        "böle",
        "by",
        "karlen",
        "vik",
        "åker",
        "höjden",
        "berget",
        "bruket",
        "åsen",
        "viken",
        "berga",
        "byn",
        "mossen",
        "parken",
        "berget",
        "branten",
        "näs",
        "skog",
        "sta",
        "kärr",
        "kulla",
        "ängen",
        "stranden"
    };
    static public bool[] nameIsTaken = new bool[districtNamesArray.Length];

    public District(int districtIndex)
    {
        population = 200;
        happiness = 1;
        nTiles = 0;
        nFactories = 0;
        nMetroStations = 0;
        districtName = RandomDistrictName();

        localPoliticians = new Politician[new RealRandom(2, 5).randomNum];
    }

    static public void SetNameArrayToFalse()
    {
        for (int i = 0; i < districtNamesArray.Length; i++)
        {
            nameIsTaken[i] = false;
        }
    }
    public string RandomDistrictName()
    {
        //TODO load array from XML file
        //bool nameFound = false;
        /*int rnd;
        do
        {
            rnd = Random.Range(0, districtNamesArray.Length);
            if (!nameIsTaken[rnd])
            {
                nameIsTaken[rnd] = true;
                nameFound = true;
            }
        } while (!nameFound);*/

        //return districtNamesArray[rnd];

        return districtNameMiddle[new RealRandom(0, districtNameMiddle.Length).randomNum] + districtNameSuffix[new RealRandom(0, districtNameSuffix.Length).randomNum];
    }
    public void Update()
    {
        population += 10 * happiness;
        if (population > nTiles * 50)
        {
            population = nTiles * 50;
        }
    }
}