using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Cinemachine;

public class PlayerNetworkBehaviour : NetworkBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerRotation playerRotation;

    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private AudioListener audioListener;

   void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerRotation = GetComponent<PlayerRotation>();
    }

    void Update()
    {
        if (IsOwner)
        {

            float inputVertical = Input.GetAxisRaw("Vertical");
            float inputHorizontal = Input.GetAxisRaw("Horizontal");

            playerMovement.Move(inputVertical);
            playerRotation.Rotate(inputHorizontal);

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
    public void EndGame()
    {
        Debug.Log("Fin");
        Time.timeScale = 0;
    }
}
