using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ProjectileSpawner : MonoBehaviour
{

    public GameObject Projectile;
    public int itemNum = 3;
    private int itemsSpawned = 0;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MyCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator MyCoroutine()
    {
        while (true)
        {
            while (itemsSpawned != itemNum)
            {
                itemsSpawned++;
                yield return new WaitForSeconds(0.5f);
                //code here will execute after 5 seconds
                Instantiate(Projectile, gameObject.transform.position, Quaternion.identity);
            }
            itemsSpawned = 0;
            yield return new WaitForSeconds(5f);
        }
    }
}
