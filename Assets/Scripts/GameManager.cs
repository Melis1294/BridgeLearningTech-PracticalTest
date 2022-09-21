using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float bounds;
    public GameObject[] objects;
    public enum ObjectType
    {
        Enemy,
        Collectible
    }

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            return instance;
        }
    }    
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        var square = GameObject.Find("Playground").gameObject;
        // adaptive value if square scale is changed during testing
        bounds = square.GetComponent<Transform>().localScale.x * 10 / 2;
    }

    private void Start()
    {
        SpawnNewObject(ObjectType.Collectible);
        SpawnNewObject(ObjectType.Enemy);
    }

    public void SpawnNewObject(ObjectType obj)
    {
        var x = Random.Range(-bounds, bounds);
        var z = Random.Range(-bounds, bounds);
        Instantiate(objects[(int)obj], new Vector3(x, 0, z), Quaternion.identity);
    }
}
