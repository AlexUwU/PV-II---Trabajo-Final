using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;

public class PlayerNetworkBehaviour : NetworkBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float aceleration;
    [SerializeField]
    private float deceleration;
    private Vector3 direction;
    private Rigidbody rigidbodyPlayer;
    public bool isGrounded;
    [SerializeField]
    private float speedDown;
    [SerializeField]
    private float rotationSpeed;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private AudioListener audioListener;

   // Start is called before the first frame update
   void Start()
    {
        rigidbodyPlayer = GetComponent<Rigidbody>();
    }


    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {

            transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed * Time.deltaTime, 0);


            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }

    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            audioListener.enabled = true;
            virtualCamera.Priority = 3;

        } else
        {
            virtualCamera.Priority = 0;
        }
    }

    private void FixedUpdate()
    {
        Move();
        if (!isGrounded)
        {
            rigidbodyPlayer.AddForce(Vector3.down * speedDown, ForceMode.Impulse);
        }
    }
    public void Move()
    {
        if (!IsOwner)
        {
            return;
        }
        if (Input.GetAxisRaw("Vertical") < 0 && speed < maxSpeed)
        {
            speed = speed - aceleration * Time.deltaTime;
        }
        else if (Input.GetAxisRaw("Vertical") > 0 && speed > -maxSpeed)
        {
            speed = speed + aceleration * Time.deltaTime;
        }
        else
        {
            if (speed > deceleration * Time.deltaTime)
            {
                speed = speed - deceleration * Time.deltaTime;
            }
            else if (speed < -deceleration * Time.deltaTime)
            {
                speed = speed + deceleration * Time.deltaTime;
            }
            else
            {
                speed = 0;
            }
        }

        if (speed > maxSpeed)
        {
            speed = maxSpeed;
        }
        else if (speed < -maxSpeed)
        {
            speed = -maxSpeed;
        }

        direction = transform.forward * speed;

        rigidbodyPlayer.velocity = direction;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Obstacle1"))
        {
            speed = speed + deceleration * Time.deltaTime * -maxSpeed;
        }

        if (other.gameObject.CompareTag("Obstacle2"))
        {
            speed *= 0.9f;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }
    
    void EndGame()
    {
        Time.timeScale = 0;
    }
}
