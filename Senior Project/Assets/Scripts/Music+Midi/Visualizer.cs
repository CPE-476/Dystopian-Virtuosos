using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Visualizer : MonoBehaviour
{
    public float minHeight;
    public float maxHeight;
    public float updateSenstivity;
    public Color visualizerColor;

    [Space(12)]
    public bool loop = true;
    [Space(12), Range(64, 8192)]
    public int visualizerSamples = 64;

    public VisualizerObject[] visualizerObjects;
    AudioSource m_audioSource;

    // Start is called before the first frame update
    void Start()
    {
        visualizerObjects = GetComponentsInChildren<VisualizerObject>();

        //if (!audioCilp)
        //return;

        m_audioSource = GameObject.Find("Conductor").GetComponent<AudioSource>();
        // m_audioSource.loop = loop;
        // m_audioSource.clip = audioCilp;
        //m_audioSource.Play();

    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        float[] spectrumData = m_audioSource.GetSpectrumData(visualizerSamples, 0, FFTWindow.Rectangular);
        float averageVolume = 0f;
        for (int i = 0; i < spectrumData.Length; i++)
        {
            averageVolume += spectrumData[i];
        }
        averageVolume /= spectrumData.Length;

        for (int i = 0; i < visualizerObjects.Length; i++)
        {
            Vector3 newSize = visualizerObjects[i].transform.localScale;

            newSize.y = Mathf.Clamp(Mathf.Lerp(newSize.y, minHeight + (averageVolume * (maxHeight - minHeight) * 25.0f), updateSenstivity), minHeight, maxHeight);
            visualizerObjects[i].transform.localScale = new Vector3(visualizerObjects[i].transform.localScale.x, newSize.y, visualizerObjects[i].transform.localScale.z);
            visualizerObjects[i].GetComponent<SpriteRenderer>().color = visualizerColor;

            // Apply noise to the sprite height
            float noise = Mathf.PerlinNoise(Time.time, i * 0.1f) * 0.5f - 0.1f;
            newSize.y += noise;
            visualizerObjects[i].transform.localScale = new Vector3(visualizerObjects[i].transform.localScale.x, newSize.y, visualizerObjects[i].transform.localScale.z);
        }
    }
}
