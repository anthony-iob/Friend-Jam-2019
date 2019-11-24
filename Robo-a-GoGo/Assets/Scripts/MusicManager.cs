using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{

    public AudioSource battleMusic;

    public AudioClip introTrack;
    public AudioClip loopTrack;

    // Start is called before the first frame update
    void Start()
    {
        battleMusic.clip = introTrack;
        battleMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!battleMusic.isPlaying)
        {
            battleMusic.clip = loopTrack;
            battleMusic.Play();
            battleMusic.loop = isActiveAndEnabled;
           
        }
            
    }
}
