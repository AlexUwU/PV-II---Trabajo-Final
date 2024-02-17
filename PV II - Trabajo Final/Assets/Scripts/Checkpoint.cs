using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Checkpoint : MonoBehaviour
{
    public int checkpointNumber;

    private LapCounter lapCounter; 

    private void Start()
    {
        lapCounter = FindObjectOfType<LapCounter>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerNetworkBehaviour playerNetworkBehaviour = other.GetComponent<PlayerNetworkBehaviour>();
            if (playerNetworkBehaviour != null)
            {
                int currentCheckpointIndex = lapCounter.GetCurrentCheckpointIndex();

                if (checkpointNumber == currentCheckpointIndex)
                {
                    lapCounter.SetCurrentCheckpointIndex(currentCheckpointIndex+1);
                }
            }
        }
    }
}