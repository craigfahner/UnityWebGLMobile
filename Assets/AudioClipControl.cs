using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipControl : MonoBehaviour
{
    public AudioSource theAudio;
    public bool paused = false;
    public int skipPosition = 0;
    private int prevSkipPos = 0;
    public int skipPos1 = 0;
    public int skipPos2 = 117;
    public int skipPos3 = 0;
    public bool enableTouchSkipping = false;

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

        if (enableTouchSkipping)
        {
            if (Input.touchCount == 2)
            {
                theAudio.time = skipPos1;
            }

            if (Input.touchCount == 3)
            {
                theAudio.time = skipPos2;
            }
            if (Input.touchCount == 4)
            {
                theAudio.time = skipPos3;
            }
        }



    }
}
