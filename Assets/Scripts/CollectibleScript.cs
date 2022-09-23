using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
    public int objScore = 10;
    [SerializeField] private string enemyTag = "Enemy";
    private Collider _myCollider;

    private void Start()
    {
        _myCollider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            Physics.IgnoreCollision(collision.collider, _myCollider);  
        }
    }
}
