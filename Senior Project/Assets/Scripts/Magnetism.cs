using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnetism : MonoBehaviour
{
    public float AttractionSpeed = 5;
    private GameObject Player;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (Player != null)
        {
            float distance = Vector2.Distance(Player.transform.position, transform.position);
            if (distance < 40)
            {
                transform.position = Vector2.MoveTowards(transform.position,
                new Vector2(Player.transform.position.x, Player.transform.position.y),
                AttractionSpeed * Time.deltaTime);
            }
        }

    }
}
