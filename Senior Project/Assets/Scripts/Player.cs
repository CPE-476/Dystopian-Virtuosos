using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour

{

    private float moveSpeed = 7.0f;
    private bool isInTrigger = false;
    private GameObject projectile = null;
    public ParticleSystem ParticleSys;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float hMove = Input.GetAxis("Horizontal");
        float vMove = Input.GetAxis("Vertical");
        transform.Translate(hMove*Time.deltaTime * moveSpeed, vMove*Time.deltaTime * moveSpeed, 0);

        if (isInTrigger && Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(projectile);
            Instantiate(ParticleSys, gameObject.transform.position, Quaternion.identity);
            projectile = null;
            isInTrigger = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        projectile = collision.gameObject;
        isInTrigger = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        projectile = null;
        isInTrigger = false;
    }
}
