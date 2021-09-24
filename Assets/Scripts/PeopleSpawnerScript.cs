using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleSpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject[] people;
    [SerializeField] private Transform spawner_1, spawner_2;

    [SerializeField] private float _freq = 1;
    [SerializeField] int maxPeople = 3;
    [SerializeField] GameObject bus;
    [SerializeField] Transform busPosition;
    private float timeToSpawn, deltaTime;
    private int counter = 0;

    public static bool available = false, isInit = false;

    void Initializate()
    {
        if (transform.Find("Bus(Clone)")) Destroy(transform.Find("Bus(Clone)").gameObject);

        counter = 0;
        Instantiate(bus, busPosition.position, Quaternion.Euler(0, -90, 0), transform);
    }
    // Start is called before the first frame update
    void Start()
    {
        deltaTime = 1f / _freq;
        timeToSpawn = deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (available && !isInit)
        {
            Initializate();
            isInit = true;
        }

        if (Time.time > timeToSpawn && counter < maxPeople && available)
        {
            int nMan = Random.Range(0, 3);
            Transform spawner = spawner_2;
            int rotation = 90;
            if (Random.Range(0, 2) == 1)
            {
                spawner = spawner_1;
                rotation = -90;
            }
            float deltaSpawn = Random.Range(0, 3f);

            Instantiate(people[nMan], new Vector3(spawner.position.x, spawner.position.y, spawner.position.z + deltaSpawn),
                Quaternion.Euler(0, rotation, 0));
            timeToSpawn = Time.time + deltaTime + Random.Range(-5f, 5f) / 10f;
            counter++;
        }
    }
}
