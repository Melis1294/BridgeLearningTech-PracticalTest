using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleScript : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag != playerTag) return;

        GameManager.Instance.SpawnNewObject(GameManager.ObjectType.Collectible);
        Destroy(gameObject);
    }
}
