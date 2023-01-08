using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    public GameObject collectablePrefab;
    public Sprite collectableSprite;

    public SpriteRenderer basketFront;
    public SpriteRenderer basketBack;

    private void Start()
    {
        basketFront.sprite = collectableSprite;
        basketBack.sprite = collectableSprite;
    }

    public void SpawnCollectable(Transform transform)
    {
        Instantiate(collectablePrefab, transform.position, Quaternion.identity);
    }

    public string GetCollectableInfo()
    {
        string collectableMessage = "";

        Collectable collectable = collectablePrefab.GetComponent<Collectable>();
        switch (collectable.CollectableType)
        {
            case Collectable.COLLECTABLE_TYPE.TOMATOE:
                collectableMessage = "Tomatoes gives you <color=green>$ " + collectable.scoreHarvest + "</color> and needs " +
                                     " <color=red>"  + (collectable.growTimerInterval * 3) + " </color> seconds to grow up";  
                break;
            case Collectable.COLLECTABLE_TYPE.DOPE:
                collectableMessage = "Some dope gives you a big amount of <color=green>$ " + collectable.scoreHarvest + "</color> but needs " +
                                     " <color=red>" + (collectable.growTimerInterval * 3) + " </color> seconds to grow up";
                break;
            
            case Collectable.COLLECTABLE_TYPE.STRAWBERRY:
                collectableMessage = "Strawberry are tasty and gives you a <color=green>$ " + collectable.scoreHarvest + "</color> and needs " +
                                     " <color=red>" + (collectable.growTimerInterval * 3) + " </color> seconds to grow up";
                break;
            case Collectable.COLLECTABLE_TYPE.EGGPLANT:
                collectableMessage = "Eggplanet doest give you only <color=green>$ " + collectable.scoreHarvest + "</color> but needs only " +
                                     " <color=red>" + (collectable.growTimerInterval * 3) + " </color> seconds to grow up";
                break;
        }
        

        return collectableMessage;
    }
}
