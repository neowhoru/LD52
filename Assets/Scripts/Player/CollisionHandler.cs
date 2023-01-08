using System;
using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    public Collectable currentCollectable = null;
    public Transform collectablePosition;

    private Hole currentHole = null;
    
    public AudioClip collectableSound;
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        HandleCollision(other.collider);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleCollision(other);
    }

    public void HandleCollision(Collider2D other)
    {
        if (other.tag.Equals("Collectable") && currentCollectable == null && CheckIfHolesAreFull())
        {
            currentCollectable = other.gameObject.GetComponent<Collectable>();
            FindObjectOfType<GameManager>().UpdateScore(currentCollectable.scoreCollect);
            currentCollectable.transform.parent = collectablePosition;
            currentCollectable.transform.localPosition = Vector3.zero;
            _audioSource.clip = collectableSound;
            _audioSource.Play();
        }
        
        if (other.tag.Equals("Basket") && currentCollectable == null && CheckIfHolesAreFull())
        {
            Basket basket = other.gameObject.GetComponent<Basket>();
            basket.SpawnCollectable(transform);
            FindObjectOfType<GameManager>().ShowMessageBox(basket.GetCollectableInfo());
        }

        if (other.tag.Equals("Hole"))
        {
            if (currentCollectable != null)
            {
                if (other.GetComponent<Hole>().treeInstance == null)
                {
                    currentCollectable.transform.parent = null;
                    FindObjectOfType<GameManager>().UpdateScore(currentCollectable.scoreSeed);
                    other.GetComponent<Hole>().SpawnTreeWithCollectable(currentCollectable);
                    currentCollectable = null;        
                }
                
            }
            else
            {
                currentHole = other.gameObject.GetComponent<Hole>();
            }
        }
    }

    private bool CheckIfHolesAreFull()
    {
        Hole[] holes = FindObjectsOfType<Hole>();
        
        foreach (Hole hole in holes)
        {
            if (hole.Collectable == null)
            {
                return true;
            }
        }

        return false;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.Equals("Hole"))
        {
            currentHole = null;
        }
        
        if(other.tag.Equals("Basket"))
        {
            FindObjectOfType<GameManager>().HideMessageBox();
        }
    }

    public void CheckIfTreeCanBeChopped()
    {
        if (currentHole != null)
        {
            if (currentHole.treeInstance.treeCanBeChopped)
            {
                currentHole.treeInstance.UpdateState();
            }
        }
    }
}
