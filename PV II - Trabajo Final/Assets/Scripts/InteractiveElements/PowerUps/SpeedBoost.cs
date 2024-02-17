using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour, InteractiveElement
{
    [SerializeField] private float speedBoost;
    [SerializeField] private float maxSpeedDuration;
    [SerializeField] private float maxSpeedValue;
    private float originalMaxSpeed;
    private bool isBoosted = false;

    public void ApplyEffect(PlayerMovement playerMovement)
    {
        if (!isBoosted)
        {
            playerMovement.SetMaxSpeed(maxSpeedValue);

            float currentSpeed = playerMovement.GetCurrentSpeed();

            currentSpeed += speedBoost;

            playerMovement.SetCurrentSpeed(currentSpeed);

            isBoosted = true;

            StartCoroutine(RestoreMaxSpeed(playerMovement));
        }
    }

    private IEnumerator RestoreMaxSpeed(PlayerMovement playerMovement)
    {
        yield return new WaitForSeconds(maxSpeedDuration);
        playerMovement.SetMaxSpeed(originalMaxSpeed);
        isBoosted = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            originalMaxSpeed = other.GetComponent<PlayerMovement>().GetMaxSpeed();
            ApplyEffect(other.GetComponent<PlayerMovement>());
        }
    }

}
