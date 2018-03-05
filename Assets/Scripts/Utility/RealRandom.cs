using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealRandom {
    public int randomNum;

	public RealRandom(int from, int to)
    {
        int coolRandomSeed = System.DateTime.Now.Millisecond * (int)System.DateTimeOffset.UtcNow.Ticks + this.GetHashCode();
        Random.InitState(coolRandomSeed);

        randomNum =  Random.Range(from, to);
    }
}
