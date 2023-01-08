using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    public float speed = 5f;
    private Vector3 change;
    private Animator anim;
    private AudioSource audioSource;
    public AudioClip chopSound;
    private bool canMove = true;

    private Rigidbody2D myRigidBody;
    private CollisionHandler _collisionHandler;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        _collisionHandler = GetComponent<CollisionHandler>();

    }

    // Update is called once per frame
    void Update()
    {
        change = Vector3.zero;
        change.x = Input.GetAxis("Horizontal");
        change.y = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.Space))
        {
            PlayChopAnimation();
        }
    }

    private void PlayChopAnimation()
    {
        if (audioSource != null)
        {
            audioSource.clip = chopSound;
            audioSource.Play();
        }
        canMove = false;
        anim.Play("PlayerChop");
        _collisionHandler.CheckIfTreeCanBeChopped();
        Invoke(nameof(EnableMovement), 0.5f);
    }

    private void EnableMovement()
    {
        audioSource.clip = null; 
        canMove = true;
    }

    public void DisableMovement()
    {
        canMove = false;
    }

    private void FixedUpdate()
    {
        UpdateAnimationAndMove();
    }

    void UpdateAnimationAndMove()
    {
        if (canMove)
        {
            if (change != Vector3.zero)
            {
                MoveCharacter();
            }
            else
            {
            
                anim.Play("PlayerIdleAnimation");
            }    
        }
        
    }

    void MoveCharacter()
    {
        myRigidBody.MovePosition(transform.position + change * speed * Time.deltaTime);
        anim.Play("PlayerWalk");
    }
}
