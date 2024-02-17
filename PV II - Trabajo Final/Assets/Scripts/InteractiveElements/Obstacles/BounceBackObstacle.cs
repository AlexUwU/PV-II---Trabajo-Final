using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBackObstacle : MonoBehaviour,InteractiveElement
{
    [SerializeField] private float bounceForce;
    [SerializeField] private float bounceDuration;

    public void ApplyEffect(PlayerMovement playerMovement)
    {
        playerMovement.SetMovementEnabled(false);

        float currentSpeed = playerMovement.GetCurrentSpeed();

        float newSpeed = -currentSpeed * bounceForce;

        playerMovement.SetCurrentSpeed(newSpeed);

        StartCoroutine(ResetMovement(playerMovement));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyEffect(other.GetComponent<PlayerMovement>());
        }
    }
    private IEnumerator ResetMovement(PlayerMovement playerMovement)
    {
        yield return new WaitForSeconds(bounceDuration);
        playerMovement.SetMovementEnabled(true);
    }
}
