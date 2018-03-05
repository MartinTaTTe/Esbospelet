using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class NewsManager
{

    public List<NewsArticle> newsQueue = new List<NewsArticle>();
    int newsArticlePosition = 0;
    int holdNewsTime = 0;
    GameObject newsObj;
    NewsArticleStatus newsArticleStatus = NewsArticleStatus.No_Article;
    TimeManager timeManager;

    public enum NewsArticleStatus
    {
        No_Article,
        Article_Going_Down,
        Article_Going_Up
    }


    public NewsManager(TimeManager timeManager, GameObject newsObj) {
        this.timeManager = timeManager;
        this.newsObj = newsObj;
    }


    public void updateNews()
    {

        if (holdNewsTime >= 0)
        {
            holdNewsTime--;
            return;
        }

        if (newsArticleStatus == NewsArticleStatus.No_Article)
        {
            if (newsQueue.Count > 0)
            {
                NewsArticle nextArticle = newsQueue[0];
                GameObject.FindGameObjectWithTag("NewsText").GetComponent<Text>().text = nextArticle.text;
                GameObject.FindGameObjectWithTag("NewsTextTitle").GetComponent<Text>().text = nextArticle.time.day + "." + nextArticle.time.monthNumber + "." + nextArticle.time.year;
                newsQueue.Remove(newsQueue[0]);

                GameObject.FindGameObjectWithTag("NewsBox").GetComponent<AudioSource>().Play();

                newsArticleStatus = NewsArticleStatus.Article_Going_Up;
            }
        }
        else if (newsArticleStatus == NewsArticleStatus.Article_Going_Up)
        {
            if (newsArticlePosition > 195)
            {
                newsArticleStatus = NewsArticleStatus.Article_Going_Down;
                holdNewsTime = 120;
            }
            else
            {
                newsArticlePosition += 3;
            }
        }
        else if (newsArticleStatus == NewsArticleStatus.Article_Going_Down)
        {
            if (newsArticlePosition < 0)
            {
                newsArticleStatus = NewsArticleStatus.No_Article;
            }
            else
            {
                newsArticlePosition -= 3;
            }
        }

        newsObj.transform.SetPositionAndRotation(new Vector3(newsObj.transform.position.x, newsArticlePosition - 100), Quaternion.Euler(0f, 0f, 0f));
    }
    public void QueueNews(string text, GameDate time)
    {
        newsQueue.Add(new NewsArticle(text, time));
    }

}

public struct NewsArticle
{
    public NewsArticle(string text, GameDate time)
    {
        this.text = text;
        this.time = time;
    }
    public GameDate time;
    public string text;
}