using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _hInput;
    private float _vInput;
    private float _bounds;
    private GameManager _manager;
    private Rigidbody _rigidbody;
    [SerializeField] private float speed = 5.0f;
    [SerializeField] private string enemyTag = "Enemy";
    
    private void Start()
    {
        _manager = GameManager.Instance;
        _bounds = _manager.bounds;
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (_manager.gameOver)
        {
            _rigidbody.isKinematic = true;
            return;
        }

        _rigidbody.isKinematic = false;
        // Count time
        _manager.time += Time.deltaTime;
        
        // Manager player movement bounds
        var position = transform.position;
        if (position.x > _bounds)
            transform.position = new Vector3(_bounds,0, transform.position.z);
        else if (position.x < -_bounds)
            transform.position = new Vector3(-_bounds, 0, transform.position.z);
        else if (position.z > _bounds)
            transform.position = new Vector3(transform.position.x, 0, _bounds);
        else if (position.z < -_bounds)
            transform.position = new Vector3(transform.position.x, 0, -_bounds);

        _hInput = Input.GetAxis("Horizontal");
        _vInput = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(_hInput, 0, _vInput) * (speed * Time.deltaTime));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            _manager.UpdateScore(-20, null);
        }
    }
}
