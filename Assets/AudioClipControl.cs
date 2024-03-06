using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipControl : MonoBehaviour
{
    public AudioSource theAudio;
    public bool paused = false;
    public int skipPosition = 0;
    private int prevSkipPos = 0;

    // Start is called before the first frame update
    void Start()
    {
        theAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (paused)
        {
            theAudio.Pause();
        } else
        {
            theAudio.UnPause();
        }

        if(skipPosition != prevSkipPos)
        {
            theAudio.time = skipPosition;
            prevSkipPos = skipPosition;
        }
    }
}
