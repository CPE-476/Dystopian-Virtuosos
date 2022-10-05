using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScheduler : MonoBehaviour
{

    [SerializeField] private GameObject map;

    public CamerController cam;

    public SimplePlayerController player;

    private GameObject clone;

    public GameObject plane;

    float dx = 42f;

    public float map_start;

    void Start()
    {
        map_start = plane.transform.position.x;
        clone = Instantiate(map, new Vector3(map_start, map.transform.position.y, 0f), Quaternion.identity);
    }

    void Update()
    {

        if(cam.transform.position.x > map_start)
        {
            Destroy(clone.gameObject, 15f);
            clone = Instantiate(map, new Vector3(map_start + dx, map.transform.position.y, 0f), Quaternion.identity);
            map_start += dx;
        }
    }

}
