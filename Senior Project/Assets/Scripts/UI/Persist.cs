using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Persist : MonoBehaviour
{
    public Slider master;
    public Slider music;
    public Slider sfx;

    public float master_volume;
    public float music_volume;
    public float sfx_volume;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        master_volume = master.value;
        music_volume = music.value;
        sfx_volume = sfx.value;
    }
}
