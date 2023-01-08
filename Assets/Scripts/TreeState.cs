using UnityEngine;

public class TreeState : MonoBehaviour
{
    private int TreeHarvestChopScore = 35;
    
    public Sprite initialState = null;
    public Sprite stateGrow1 = null;
    public Sprite stateGrow2smallTree = null;
    public Sprite stateFullTree = null;
    public Sprite stateTreeStump = null;
    public bool treeCanBeChopped = false;
    public int treeTimerStateSeconds = 3;
    public Hole assignedHoleInstance = null;

    public GameObject fallingTree;

    private Animator _animator;
    private AudioSource _audioSource;

    private SpriteRenderer _spriteRenderer;
    public SpriteRenderer collectableSprite;
    
    public enum TREE_STATE
    {
        INITIAL_STATE,
        GROW_1,
        SMALL_TREE,
        FULL_TREE,
        CHOPPED_TREE
    }

    private TREE_STATE currentState = TREE_STATE.INITIAL_STATE;
    
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sprite = initialState;
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    public void StartTreeStateTimer(int secondsCollectable)
    {
        treeTimerStateSeconds = secondsCollectable;
        InvokeRepeating(nameof(UpdateState),treeTimerStateSeconds,treeTimerStateSeconds);
    }

    public void UpdateState()
    {
        switch (currentState)
        {
            case TREE_STATE.INITIAL_STATE:
                currentState = TREE_STATE.GROW_1;
                _spriteRenderer.sprite = stateGrow1;
                break;
            
            case TREE_STATE.GROW_1:
                currentState = TREE_STATE.SMALL_TREE;
                _spriteRenderer.sprite = stateGrow2smallTree;
                break;
            case TREE_STATE.SMALL_TREE:
                currentState = TREE_STATE.FULL_TREE;
                _spriteRenderer.sprite = stateFullTree;
                treeCanBeChopped = true;
                collectableSprite.sprite = assignedHoleInstance.Collectable.GetComponent<SpriteRenderer>().sprite;
                assignedHoleInstance.currentHoleState = Hole.HOLE_STATE.READY_TO_HARVEST;
                _audioSource.Play();
                CancelInvoke(nameof(UpdateState));
                break;
            case TREE_STATE.FULL_TREE:
                collectableSprite.gameObject.SetActive(false);
                _spriteRenderer.sprite = stateTreeStump;
                currentState = TREE_STATE.CHOPPED_TREE;
                treeCanBeChopped = false;
                PlayChopAnimation();
                break;
                
        }
    }

    public void PlayChopAnimation()
    {
        Debug.Log("PlayChop Animation and activate GameObject");
        
        Color alphaChange = fallingTree.GetComponent<SpriteRenderer>().color;
        alphaChange.a = 255;
        fallingTree.GetComponent<SpriteRenderer>().color = alphaChange;
        
        if (_animator != null)
        {
            // ToDo: Animation
            _animator.Play("TreeFallAnimation");
        }
    }

    public void SetHole(Hole hole)
    {
        assignedHoleInstance = hole;
    }

    public void RemoveTreeAndUpdateScore()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.UpdateScore(assignedHoleInstance.Collectable.scoreHarvest + TreeHarvestChopScore);
        gameManager.IncreaseHarvestCollectableCount(assignedHoleInstance.Collectable.CollectableType);
        assignedHoleInstance.Collectable = null;
        Destroy(gameObject);
    }

}
