using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;

    private Rigidbody rigidbodyPlayer;
    [SerializeField] private Vector3 direction;
    [SerializeField] private float currentSpeed;
    private bool movementEnabled = true;

    private void Start()
    {
        rigidbodyPlayer = GetComponent<Rigidbody>();
    }

    public void Move(float inputVertical)
    {
        if (movementEnabled)
        {
            if (inputVertical != 0)
            {
                currentSpeed += inputVertical * acceleration * Time.deltaTime;
                currentSpeed = Mathf.Clamp(currentSpeed, -maxSpeed, maxSpeed);
            }
            else
            {
                float decelerationValue = deceleration * Time.deltaTime * Mathf.Sign(currentSpeed);
                currentSpeed = Mathf.Abs(decelerationValue) >= Mathf.Abs(currentSpeed) ? 0 : currentSpeed - decelerationValue;
            }
        }

        direction = transform.forward * currentSpeed;
        rigidbodyPlayer.velocity = direction;
    }

    public float GetCurrentSpeed()
    {
        return currentSpeed;
    }
    public void SetCurrentSpeed(float speed)
    {
        currentSpeed = Mathf.Clamp(speed, -maxSpeed, maxSpeed);
    }
    public float GetMaxSpeed()
    {
        return maxSpeed;
    }
    public void SetMaxSpeed(float speed)
    {
        maxSpeed = Mathf.Max(speed, 0);
    }
    public void SetMovementEnabled(bool enabled)
    {
        movementEnabled = enabled;
    }
        public float GetDeceleration()
    {
        return deceleration;
    }
}
