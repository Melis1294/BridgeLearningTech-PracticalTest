using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float h_input;
    private float v_input;
    private float bounds;
    [SerializeField] private readonly float speed = 5.0f;

    private void Start()
    {
        bounds = GameManager.Instance.bounds;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x > bounds)
            transform.position = new Vector3(bounds,0, transform.position.z);
        else if (transform.position.x < -bounds)
            transform.position = new Vector3(-bounds, 0, transform.position.z);
        else if (transform.position.z > bounds)
            transform.position = new Vector3(transform.position.x, 0, bounds);
        else if (transform.position.z < -bounds)
            transform.position = new Vector3(transform.position.x, 0, -bounds);

        h_input = Input.GetAxis("Horizontal");
        v_input = Input.GetAxis("Vertical");

        transform.Translate(new Vector3(h_input, 0, v_input) * speed * Time.deltaTime);
    }
}
