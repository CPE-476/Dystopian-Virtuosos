using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScheduler : MonoBehaviour
{

    [SerializeField] private GameObject map;

    public CamerController cam;

    //public SimplePlayerController player;

    private GameObject clone;
    private GameObject clone2;

    public GameObject plane;

    public float CharacterSpeed;

    float dx = 42f;

    private float map_end;

    void Start()
    {
        map_end = plane.transform.position.x;
        clone = Instantiate(map, new Vector3(map_end, map.transform.position.y, 0f), Quaternion.identity);
        map_end += dx;
        clone2 = Instantiate(map, new Vector3(map_end, map.transform.position.y, 0f), Quaternion.identity);
        map_end += dx;
    }

    void Update()
    {
        if (cam.isMoving){
            if (clone != null){
                clone.transform.Translate(new Vector3(-CharacterSpeed * Time.deltaTime, 0, 0));
            }
            if (clone2 != null){
                clone2.transform.Translate(new Vector3(-CharacterSpeed * Time.deltaTime, 0, 0));
            }
            map_end -= CharacterSpeed * Time.deltaTime;
        }

            if (clone2 == null){
                clone2 = Instantiate(map, new Vector3(map_end, map.transform.position.y, 0f), Quaternion.identity);
/*                clone2.transform.Translate(new Vector3(-CharacterSpeed * Time.deltaTime, 0, 0));*/
            map_end += dx;
        }
            if (clone == null){
                clone = Instantiate(map, new Vector3(map_end, map.transform.position.y, 0f), Quaternion.identity);
 /*                clone.transform.Translate(new Vector3(-CharacterSpeed * Time.deltaTime, 0, 0));*/
            map_end += dx;
        }
        if (clone != null){
            if (clone.transform.position.x + dx < 0)
            {
                Destroy(clone.gameObject, 0f);
            }
        }

        if(clone2 != null)
        {
            if (clone2.transform.position.x + dx < 0)
            {
                Destroy(clone2.gameObject, 0f);
            }
        }
    }

}
