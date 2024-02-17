using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class LapCounter : NetworkBehaviour
{
    public TextMeshProUGUI lapCountText;
    public int totalLaps = 3;

    public Checkpoint[] checkpoints;
    private int currentCheckpointIndex = 0;

    private Dictionary<ulong, int> lapsPerPlayer = new Dictionary<ulong, int>();

    private void Start()
    {
        UpdateLapText(0, totalLaps);
    }

    public int GetCurrentCheckpointIndex()
    {
        return currentCheckpointIndex;
    }
    public void SetCurrentCheckpointIndex(int checkpointIndex)
    {
        currentCheckpointIndex = checkpointIndex;
    }

    private void InitializeLapCount(ulong playerID)
    {
        lapsPerPlayer[playerID] = 0;
    }

    public void IncrementLapCount(ulong playerID)
    {
        if (lapsPerPlayer.ContainsKey(playerID) && currentCheckpointIndex == checkpoints.Length)
        {
            lapsPerPlayer[playerID]++;
            UpdateLapText(lapsPerPlayer[playerID], totalLaps);

            if (lapsPerPlayer[playerID] >= totalLaps)
            {
                PlayerNetworkBehaviour player = NetworkManager.Singleton.ConnectedClients[playerID-1].PlayerObject.GetComponent<PlayerNetworkBehaviour>();
                if (player != null)
                {
                    player.EndGame();
                }
            }
            else
            {
                currentCheckpointIndex = 0;
            }
        }
    }

    private void UpdateLapText(int currentLaps, int totalLaps)
    {
        lapCountText.text = string.Format("Lap: {0}/{1}", currentLaps, totalLaps);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && IsServer)
        {
            NetworkObject networkObject = other.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                ulong playerID = (uint)networkObject.NetworkObjectId;
                if (!lapsPerPlayer.ContainsKey(playerID))
                {
                    InitializeLapCount(playerID);
                }
                IncrementLapCount(playerID);
            }
        }
    }
}