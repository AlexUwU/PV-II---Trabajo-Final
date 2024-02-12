using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBehaviour : MonoBehaviour
{
    [SerializeField]
    private float speedRotate;
    [SerializeField]
    private float distance;
    [SerializeField]
    private float speedMove;
    [SerializeField]
    private Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(1 * speedRotate * Time.deltaTime, 0, 0);
        Move();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }

    }

    public void Move()
    {
        transform.Translate(Vector3.right * speedMove * Time.deltaTime);
        if (transform.position.y > initialPosition.y + distance || transform.position.y < initialPosition.y)
        {
            speedMove *= -1;
        }
    }
}
