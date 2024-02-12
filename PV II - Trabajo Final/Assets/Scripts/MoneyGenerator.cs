using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject money;
    [SerializeField]
    private float initTime;
    [SerializeField]
    private float repeatTime;
    [SerializeField]
    private int counterMoney;
    [SerializeField]
    private float distance;
    [SerializeField]
    private Vector3 initialPosition;
    [SerializeField]
    private bool infinityMoney;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.position;
        initialPosition.x += distance;
        InvokeRepeating("CreateMoney", initTime, repeatTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void CreateMoney()
    {
        if (counterMoney > 0)
        {
            Instantiate(money, initialPosition, transform.rotation);
            counterMoney--;
            initialPosition.x += distance;
        }
        else if (infinityMoney)
        {
            Instantiate(money, initialPosition, transform.rotation);
            initialPosition.x += distance;
        }
    }
}
