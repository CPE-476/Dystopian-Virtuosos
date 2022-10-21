using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{

    public Conductor conductor;
    Vector3 oldScale;
    Vector3 newScale;
    int num;
    bool big;
    // Start is called before the first frame update
    void Start()
    {
        num = 0;
        oldScale = transform.localScale;
        newScale = oldScale + new Vector3(0.05f, 0.05f, 0.05f);
        big = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (num == conductor.beatNumber)
        {
            Debug.Log("inside");
            if (big)
            {
                transform.localScale = newScale;
                big = false;
            }
            else
            {
                transform.localScale = oldScale;
                big = true;
            }
            num++;
            if (num >= 4)
                num = 0;
        }
        
    }
}
