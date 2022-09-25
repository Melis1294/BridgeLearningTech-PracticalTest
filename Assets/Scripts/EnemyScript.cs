using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    private GameObject _target;
    private GameManager _manager;
    private Rigidbody _rigidbody;
    private float _bounds;
    [SerializeField] private float delta = 10.0f;
    [SerializeField] private float enemySpeed = 4.0f;
    [SerializeField] private float rotationSpeed = 2.0f;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag(playerTag);
        _manager = GameManager.Instance;
        _rigidbody = GetComponent<Rigidbody>();
        _bounds = _manager.bounds;
    }

    private void Update()
    {
        if (_manager.gameOver)
        {
            if (!_rigidbody.isKinematic) _rigidbody.isKinematic = true;
            return;
        }
        
        // Destroy enemies out of bounds
        if (transform.position.x > _bounds + delta || transform.position.x < -_bounds - delta || 
            transform.position.z > _bounds + delta || transform.position.z < -_bounds - delta)
            Destroy(gameObject);

        var enemyPosition = transform.position;
        var enemyRotation = transform.rotation;
        var targetPosition = _target.transform.position;
        var targetSpot = new Vector3(targetPosition.x, 0, targetPosition.z);
        transform.rotation = Quaternion.Slerp(enemyRotation, Quaternion.LookRotation(targetSpot - enemyPosition), rotationSpeed * Time.deltaTime);

        //move towards the player
        transform.position += transform.forward * (Time.deltaTime * enemySpeed);
    }
}
