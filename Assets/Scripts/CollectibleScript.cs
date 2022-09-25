using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
    public int objScore = 10;
    public GameManager.ObjectType objType;
    [SerializeField] private string enemyTag = "Enemy";
    [SerializeField] private string playerTag = "Player";
    private Collider _myCollider;
    private GameManager _manager;

    private void Start()
    {
        _myCollider = GetComponent<Collider>();
        _manager = GameManager.Instance;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            Physics.IgnoreCollision(collision.collider, _myCollider);  
        }
        else if (collision.gameObject.CompareTag(playerTag))
        {
            _manager.UpdateScore(objScore, objType);
            Destroy(gameObject);
        }
    }
}
