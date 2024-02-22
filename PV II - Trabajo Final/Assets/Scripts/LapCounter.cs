using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class LapCounter : NetworkBehaviour
{
    public TextMeshProUGUI lapCountTextPrefab;
    public Transform textSpawnParent;
    public int totalLaps = 3;

    public Checkpoint[] checkpoints;
    private int currentCheckpointIndex = 0;

    private Dictionary<ulong, TextMeshProUGUI> lapTextPerPlayer = new Dictionary<ulong, TextMeshProUGUI>();
    private Dictionary<ulong, int> lapsPerPlayer = new Dictionary<ulong, int>();

    private void Start()
    {
        if (IsServer)
        {
            foreach (var playerID in NetworkManager.Singleton.ConnectedClients.Keys)
            {
                InitializeLapCount(playerID);
            }
        }
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

        TextMeshProUGUI lapText = Instantiate(lapCountTextPrefab, textSpawnParent);
        lapTextPerPlayer[playerID] = lapText;

        lapText.transform.SetParent(textSpawnParent);

        NetworkObject networkObject = lapText.GetComponent<NetworkObject>();
        networkObject.SpawnWithOwnership(playerID);

        UpdateLapText(playerID);
    }

    public void IncrementLapCount(ulong playerID)
    {
        if (lapsPerPlayer.ContainsKey(playerID) && currentCheckpointIndex >= checkpoints.Length)
        {
            lapsPerPlayer[playerID]++;
            UpdateLapText(playerID);

            if (lapsPerPlayer[playerID] >= totalLaps)
            {
                PlayerNetworkBehaviour player = NetworkManager.Singleton.ConnectedClients[playerID].PlayerObject.GetComponent<PlayerNetworkBehaviour>();
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

    private void UpdateLapText(ulong playerID)
    {
        int currentLaps = lapsPerPlayer[playerID];
        TextMeshProUGUI lapText = lapTextPerPlayer[playerID];
        lapText.text = string.Format("Lap: {0}/{1}", currentLaps, totalLaps);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && IsServer)
        {
            NetworkObject networkObject = other.GetComponent<NetworkObject>();
            if (networkObject != null)
            {
                ulong playerID = networkObject.OwnerClientId;
                if (!lapsPerPlayer.ContainsKey(playerID))
                {
                    InitializeLapCount(playerID);
                }
                IncrementLapCount(playerID);
            }
        }
    }
}