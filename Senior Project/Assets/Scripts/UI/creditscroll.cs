using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class creditscroll : MonoBehaviour
{
    public float scroll_speed;

    private float initial_y_point;

    // Start is called before the first frame update
    void Start()
    {
        initial_y_point = transform.position.y;        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0,scroll_speed,0);
    }

    public void Reset()
    {
        transform.position = new Vector3(transform.position.x, initial_y_point, transform.position.y);
    }
}
