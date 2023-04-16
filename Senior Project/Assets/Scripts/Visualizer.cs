using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Visualizer : MonoBehaviour
{
    public float minHeight = 15f;
    public float maxHeight = 425f;
    public float updateSenstivity = 0.2f;
    public Color visualizerColor = Color.white;

    [Space(12)]
    public AudioClip audioCilp;
    public bool loop = true;
    [Space(12), Range(64, 8192)]
    public int visualizerSamples = 64;

    public VisualizerObject[] visualizerObjects;
    AudioSource m_audioSource;

    // Start is called before the first frame update
    void Start()
    {
        visualizerObjects = GetComponentsInChildren<VisualizerObject>();

        if (!audioCilp)
            return;

        m_audioSource = new GameObject("AudioSource").AddComponent<AudioSource>();
        m_audioSource.loop = loop;
        m_audioSource.clip = audioCilp;
        m_audioSource.Play();

        foreach (var visualizerObject in visualizerObjects)
        {
            var rb2d = visualizerObject.GetComponent<Rigidbody2D>();
            rb2d.gravityScale = 1;
        }
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        float[] spectrumData = m_audioSource.GetSpectrumData(visualizerSamples, 0, FFTWindow.Rectangular);
        for (int i = 0; i < visualizerObjects.Length; i++)
        {
            Vector2 newPosition = visualizerObjects[i].transform.position;

            newPosition.y = Mathf.Clamp(Mathf.Lerp(newPosition.y, minHeight + (spectrumData[i] * (maxHeight - minHeight) * 5.0f), updateSenstivity),minHeight, maxHeight);
            visualizerObjects[i].transform.position = newPosition;

            visualizerObjects[i].GetComponent<Image>().color = visualizerColor;
        }
    }
}
