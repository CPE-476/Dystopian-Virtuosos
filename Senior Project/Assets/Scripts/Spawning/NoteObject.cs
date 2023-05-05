using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    private Transform parentTransform;
    private Conductor conductor;
    private NoteTrigger notetrigger;
    private SpawnMaster spawnmaster;
    private CollectableUI collectables;
    float localSpot;
    private float rotSpeed = 100f;

    // Start is called before the first frame update
    void Start()
    {
        conductor = (Conductor)GameObject.Find("/Conductor").GetComponent("Conductor");
        notetrigger = (NoteTrigger)GameObject.Find("/Tracks/NoteTrigger").GetComponent("NoteTrigger");
        spawnmaster = (SpawnMaster)GameObject.Find("/Tracks/SpawnMaster").GetComponent("SpawnMaster");
        collectables = (CollectableUI)GameObject.Find("/Canvas/Gameplay/HealthBar/Collectables").GetComponent("CollectableUI");
        parentTransform = transform.parent;
        parentTransform = transform.parent;
        localSpot = notetrigger.currentSpot;
    }

    // Update is called once per frame
    void Update()
    {
        float interpRatio = ((float)conductor.songPosition - localSpot) / (conductor.spotLength * spawnmaster.noteSpeed);

        Vector3 interpedPostion = Vector3.Lerp(parentTransform.position, new Vector3(notetrigger.transform.position.x, parentTransform.position.y, 0f), interpRatio);
        transform.position = interpedPostion;

        if (gameObject != null && gameObject.CompareTag("Collect"))
        {
            // Make the object spin around the y-axis
            transform.Rotate(Vector3.forward, rotSpeed * Time.deltaTime);
            rotSpeed += 0.03f * Time.deltaTime;
        }
        if (interpRatio > 1.0f)
            GetComponent<SpriteRenderer>().enabled = false;

        if(interpRatio > 1.1f)
        {
            Destroy(gameObject);
        }

        //transform.localScale = new Vector3(1f, 1f, 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        collectables.collectableNum++;
        collectables.updateCollectables();
        //PLAY SFX
        //SPAWN PARTICLES
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
    }
}
