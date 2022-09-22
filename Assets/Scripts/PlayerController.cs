using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _hInput;
    private float _vInput;
    private float _bounds;
    private GameManager _manager;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private string collectibleTag = "Collectible";

    private void Start()
    {
        _manager = GameManager.Instance;
        _bounds = _manager.bounds;
    }
    
    private void Update()
    {
        if (_manager.gameOver) return;
        
        // Manager player movement bounds
        if (transform.position.x > _bounds)
            transform.position = new Vector3(_bounds,0, transform.position.z);
        else if (transform.position.x < -_bounds)
            transform.position = new Vector3(-_bounds, 0, transform.position.z);
        else if (transform.position.z > _bounds)
            transform.position = new Vector3(transform.position.x, 0, _bounds);
        else if (transform.position.z < -_bounds)
            transform.position = new Vector3(transform.position.x, 0, -_bounds);

        _hInput = Input.GetAxis("Horizontal");
        _vInput = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(_hInput, 0, _vInput) * (speed * Time.deltaTime));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            _manager.UpdateScore(-20);
            _manager.SpawnNewObject(GameManager.ObjectType.Enemy);
        }
        else if (collision.gameObject.CompareTag(collectibleTag))
        {
            _manager.UpdateScore(10);
            _manager.SpawnNewObject(GameManager.ObjectType.Collectible);
            Destroy(collision.gameObject);
        }
    }
}
