using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    private GameObject _target;
    [SerializeField] private float enemySpeed = 4.0f;
    [SerializeField] private float rotationSpeed = 2.0f;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag(playerTag);
    }

    private void Update()
    {
        var enemyPosition = transform.position;
        var enemyRotation = transform.rotation;
        transform.rotation = Quaternion.Slerp(enemyRotation, Quaternion.LookRotation(_target.transform.position - enemyPosition), rotationSpeed * Time.deltaTime);
 
 
        //move towards the player
        transform.position += transform.forward * (Time.deltaTime * enemySpeed);
    }
}
