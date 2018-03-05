using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class Facility
{
    public Coordinates location;
    public FacilityType type;
    public float[] mine;
    public float[] agriculture;
    public OtherContent otherType;
    public float other;
    public bool isTaken;

    public enum FacilityType
    {
        Mine,
        Agriculture,
        Other,
        length // ska vara sist
    }
    public enum MineContent
    {
        Iron,
        Coal,
        Silver,
        Gold,
        length // ska vara sist
    }
    public enum AgricultureContent
    {
        Wheat,
        Oat,
        Barley,
        Rye,
        Milk,
        Beef,
        Ham,
        length // ska vara sist
    }
    public enum OtherContent
    {
        Oil,
        Fish,
        length // ska vara sist
    }

    public Facility(Coordinates location, FacilityType type)
    {
        this.location = location;
        this.type = type;
        if (this.type == FacilityType.Mine)
        {
            mine = new float[(int)MineContent.length];
            GenerateMineContent();
        }
        if (this.type == FacilityType.Agriculture)
        {
            agriculture = new float[(int)AgricultureContent.length];
            GenerateAgricultureContent();
        }
        isTaken = false;
    }

    void GenerateMineContent()
    {
        for(int i = 0; i < (int)MineContent.length; i++)
        {
            mine[i] = Random.Range(0f, 100f);
        }
    }
    void GenerateAgricultureContent()
    {
        for (int i = 0; i < (int)AgricultureContent.length; i++)
        {
            agriculture[i] = Random.Range(0f, 100f);
        }
    }
    void GenerateOtherContent()
    {
        other = Random.Range(0f, 100f);
    }
    public void ChangeMineContent(MineContent content, float amount)
    {
        mine[(int)content] += amount;
    }
    public void ChangeAgricultureContent(AgricultureContent content, float amount)
    {
        agriculture[(int)content] += amount;
    }
    public void ChangeOtherContent(float amount)
    {
        other += amount;
    }
    public void GetsTaken()
    {
        isTaken = true;
    }
}