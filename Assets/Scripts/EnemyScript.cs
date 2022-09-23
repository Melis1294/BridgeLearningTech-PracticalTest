using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    private GameObject _target;
    private GameManager _manager;
    private Rigidbody _rigidbody;
    [SerializeField] private float enemySpeed = 4.0f;
    [SerializeField] private float rotationSpeed = 2.0f;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag(playerTag);
        _manager = GameManager.Instance;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_manager.gameOver)
        {
            if (!_rigidbody.isKinematic) _rigidbody.isKinematic = true;
            return;
        }
        
        var enemyPosition = transform.position;
        var enemyRotation = transform.rotation;
        var position = _target.transform.position;
        var targetSpot = new Vector3(position.x, 0, position.z);
        transform.rotation = Quaternion.Slerp(enemyRotation, Quaternion.LookRotation(targetSpot - enemyPosition), rotationSpeed * Time.deltaTime);

        //move towards the player
        transform.position += transform.forward * (Time.deltaTime * enemySpeed);
    }
}
