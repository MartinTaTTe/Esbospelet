using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MoneyManager : MonoBehaviour {

    Text moneyTextObj;
    public int moneyAmount;

    void Start () {
        moneyTextObj = GameObject.FindGameObjectWithTag("Money").GetComponent<Text>();
    }

    // Update is called once per frame

    //Kostade ca 31000000mk år 1950 att bygga 1km skena
    void Update () {
        moneyTextObj.text = "Stadsbudgeten: " + moneyAmount + " mk";
	}

    public void NewYearListener()
    {
        GameObject.FindGameObjectWithTag("_Manager").GetComponent<MoneyManager>().moneyAmount = 2000000;
    }
    public void NewMonthListener()
    {
        GameObject.FindGameObjectWithTag("_Manager").GetComponent<MoneyManager>().moneyAmount += 400000;
    }
}
