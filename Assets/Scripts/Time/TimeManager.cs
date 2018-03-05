using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TimeManager : MonoBehaviour {

    Text timeText;
    float oldTime = 0F;
    float gameTime = 0F;
    int oldMonth = -1;
    int oldYear = 1959;
    bool timePaused = false;

    int GAMESPEED = 1;

    public GameEventManager gameEventManager;
    public NewsManager newsManager;
    public MoneyManager moneyManager;
 
    void Start () {
        timeText = GameObject.FindGameObjectWithTag("TimeText").GetComponent<Text>();
        newsManager = new NewsManager(this, GameObject.FindGameObjectWithTag("NewsBox"));
        gameEventManager = new GameEventManager(this);
        moneyManager = GameObject.FindGameObjectWithTag("_Manager").GetComponent<MoneyManager>();
    }

    void FixedUpdate () {
        if (!timePaused) {
            float deltaTime = Time.time - oldTime;
            gameTime += deltaTime * GAMESPEED;
            timeText.text = GetDateString(gameTime);
        }
        oldTime = Time.time;

        if(oldYear < GetCurrentYear())
        {
            moneyManager.NewYearListener();
            oldYear = GetCurrentYear();
        }

        if (oldMonth < GetCurrentMonth())
        {
            moneyManager.NewMonthListener();
            gameEventManager.politicsManager.NewMonthListener();
            oldMonth = GetCurrentMonth();
        }
        newsManager.updateNews();
        gameEventManager.Tick();
    }
    public int GetCurrentMonth()
    {
        return ((int)(gameTime / 30)) % 12 + 1;
    }


    public string GetDateString(float gameTime)
    {
        //gameTime = gameTime / 2;
        string dateTime = ((int)(gameTime % 30) + 1).ToString() + " ";

        if((int)((gameTime / 30) % 12) == 0)
        {
            dateTime += "Jan" + " ";
        }
        if ((int)((gameTime / 30) % 12) == 1)
        {
            dateTime += "Feb" + " ";
        }
        if ((int)((gameTime / 30) % 12) == 2)
        {
            dateTime += "Mar" + " ";
        }
        if ((int)((gameTime / 30) % 12) == 3)
        {
            dateTime += "Apr" + " ";
        }
        if ((int)((gameTime / 30) % 12) == 4)
        {
            dateTime += "May" + " ";
        }
        if ((int)((gameTime / 30) % 12) == 5)
        {
            dateTime += "Jun" + " ";
        }
        if ((int)((gameTime / 30) % 12) == 6)
        {
            dateTime += "Jul" + " ";
        }
        if ((int)((gameTime / 30) % 12) == 7)
        {
            dateTime += "Aug" + " ";
        }
        if ((int)((gameTime / 30) % 12) == 8)
        {
            dateTime += "Sep" + " ";
        }
        if ((int)((gameTime / 30) % 12) == 9)
        {
            dateTime += "Oct" + " ";
        }
        if ((int)((gameTime / 30) % 12) == 10)
        {
            dateTime += "Nov" + " ";
        }
        if ((int)((gameTime / 30) % 12) == 11)
        {
            dateTime += "Dec" + " ";
        }

        dateTime += 1960 + (int)(gameTime / 360);

        return dateTime;
    }
    public void pauseRun()
    {
        timePaused = !timePaused;
    }
    public int GetCurrentYear()
    {
        return (int)(gameTime / 360) + 1960;
    }
    public GameDate GetGameDate()
    {
        return new GameDate((int)(gameTime % 30) + 1, (int)((gameTime / 30) % 12) + 1, GetCurrentYear());
    }

    public GameDate AddDaysToDate(GameDate date, int days)
    {
        days += date.day + date.monthNumber * 30 + date.year * 30 * 12;
        return new GameDate((int)(days % 30), (int)((days / 30) % 12), (int)(days / 360));
    }
} 



public struct GameDate
{
    public GameDate(int day, int monthNumber, int year)
    {
        this.day = day;
        this.monthNumber = monthNumber;
        this.year = year;
    }
    public int day;
    public int monthNumber;
    public int year;
}