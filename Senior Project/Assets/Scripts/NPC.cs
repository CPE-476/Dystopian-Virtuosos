using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{

    [SerializeField] private Image cat;
    public CamerController cam;

    // Start is called before the first frame update
    void Start()
    {
        cat.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!cam.isMoving)
        {
            cat.enabled = true;
        }
        else
        {
            cat.enabled = false;
        }
    }
}
