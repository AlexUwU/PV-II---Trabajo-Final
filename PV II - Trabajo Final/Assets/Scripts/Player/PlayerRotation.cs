using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;


    public void Rotate(float inputHorizontal)
    {
        transform.Rotate(0, inputHorizontal * rotationSpeed * Time.deltaTime, 0);
    }
}
