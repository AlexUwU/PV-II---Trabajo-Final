using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDownObstacle : MonoBehaviour,InteractiveElement
{
    [SerializeField] private float slowFactor;
    private float originalSpeed;

    public void ApplyEffect(PlayerMovement playerMovement)
    {
        float maxSpeed = playerMovement.GetMaxSpeed();
        playerMovement.SetMaxSpeed(maxSpeed * slowFactor);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            originalSpeed = other.GetComponent<PlayerMovement>().GetMaxSpeed();
            ApplyEffect(other.GetComponent<PlayerMovement>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<PlayerMovement>().SetMaxSpeed(originalSpeed);
    }
}
