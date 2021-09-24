using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckSpawner : MonoBehaviour
{
    [SerializeField] GameObject truck;
    [SerializeField] Transform truckPos;
    // Start is called before the first frame update

    public static bool available = false, isInit = false;

    void Initializate()
    {
        if (transform.Find("Truck(Clone)")) Destroy(transform.Find("Truck(Clone)").gameObject);

        Instantiate(truck, truckPos.position, Quaternion.Euler(0, 152.8f, 0), transform);
    }
 
    private void Update()
    {
        if (available && !isInit)
            {
                Initializate();
                isInit = true;
            }
    }
}
