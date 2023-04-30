using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LatencyCalibrator : MonoBehaviour
{
    public AudioSource audioSource;

    public double startTime;
    public double bufferSchedulingOffset = 1.0;

    public double tempo = 110.0;
    double beatLength;

    double[] latencies = {0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0};
    int cur_index = 0;
    double latency_offset = 0.0;

    double GetCalibrationTime() {
        return AudioSettings.dspTime - startTime;
    }

    void Start()
    {
        startTime = (double)AudioSettings.dspTime + bufferSchedulingOffset;
        beatLength = 60 / tempo;

        audioSource = GetComponent<AudioSource>();
        audioSource.PlayScheduled(startTime);
    }

    double GetDesiredLatencyOffset() {
        double average = 0.0;
        for(int i = 0; i < 10; ++i) {
            average += latencies[i];
        }
        return -(average / 10.0);
    }

    void Update()
    {
        double beat_with_fraction = GetCalibrationTime() / beatLength;
        int beat = (int)beat_with_fraction % 4;
        double fractional_part = beat_with_fraction - (int)beat_with_fraction;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            double current_beat = GetCalibrationTime() / beatLength;
            double off;
            if(fractional_part >= 0.5) off = fractional_part - 1;
            else off = fractional_part;
            latencies[cur_index] = off;
            ++cur_index;
            if(cur_index == 4) cur_index = 0;

            latency_offset = GetDesiredLatencyOffset();
            Debug.Log(latency_offset);
        }

        double beat_with_fraction_latency = GetCalibrationTime() / beatLength + latency_offset;
        int beat_latency = (int)beat_with_fraction_latency % 4;
        double fractional_part_latency = beat_with_fraction_latency - (int)beat_with_fraction_latency;

        // Display the user's tempo.
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if(beat == i) {
                if(fractional_part_latency < 0.1 || fractional_part_latency > 0.9)
                    child.GetComponent<Image>().color = Color.red;
                else
                    child.GetComponent<Image>().color = Color.blue;
            }
        }
    }
}