using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private Image entity;

    public CamerController cam;

    public string npcName;

    // Start is called before the first frame update
    void Start()
    {
        entity.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!cam.isMoving)
        {
            entity.enabled = true;
        }
        else
        {
            entity.enabled = false;
        }
    }
}
