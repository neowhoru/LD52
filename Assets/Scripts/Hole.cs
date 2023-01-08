using UnityEngine;

public class Hole : MonoBehaviour
{
    public Collectable Collectable = null;
    public Sprite harvestedSprite;
    public GameObject treePrefab;
    public Transform treeSpawnPosition;
    public TreeState treeInstance = null;
    public enum HOLE_STATE
    {
        EMPTY,
        SEEDED,
        READY_TO_HARVEST,
        HARVESTED
    }

    public HOLE_STATE currentHoleState = HOLE_STATE.EMPTY;

    public void SpawnTreeWithCollectable(Collectable collectable)
    {
        Collectable = collectable;
        Collectable.gameObject.SetActive(false);
        GameObject treeInstanceObject  = Instantiate(treePrefab, treeSpawnPosition.position, Quaternion.identity);
        treeInstanceObject.GetComponent<TreeState>().StartTreeStateTimer(collectable.growTimerInterval);
        treeInstanceObject.transform.parent = treeSpawnPosition;
        
        
        treeInstance = treeInstanceObject.GetComponent<TreeState>();

        treeInstance.SetHole(this);
        currentHoleState = HOLE_STATE.SEEDED;
    }
}
