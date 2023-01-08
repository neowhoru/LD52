using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public int scoreCollect = 20;
    public int scoreSeed = 45;
    public int scoreHarvest = 140;

    public int growTimerInterval = 3;

    public enum COLLECTABLE_TYPE
    {
        TOMATOE,
        DOPE,
        STRAWBERRY,
        EGGPLANT
    }

    public COLLECTABLE_TYPE CollectableType = COLLECTABLE_TYPE.TOMATOE;
}
