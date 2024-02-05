using System;
using System.Collections;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private AudioSource audioSource;

    public static MusicController Instance;
    // This function is called when the script instance is being loaded.
    void Awake()
    {
        Instance = this;
        // Prevents the GameObject from being destroyed when loading a new scene.
        DontDestroyOnLoad(gameObject);

        // Get the AudioSource component attached to the same GameObject.
        audioSource = GetComponent<AudioSource>();
        if(audioSource == null)
        {
            Debug.LogError("Songolton: No AudioSource component found on the GameObject.");
        }

        
    }

    private void Start()
    {
        PauseAudio();
    }

    // Function to play the audio.
    public void PlayAudio()
    {
        if(audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }

        counter = 0.3f;
        if(!isPlaying)
            StartCoroutine(WaitToPlay());
    }

    private float counter = 1;
    private bool isPlaying = false;
    IEnumerator WaitToPlay()
    {
        while (counter>=0)
        {
            isPlaying = true;
            yield return new WaitForSeconds(0.1f);
            counter -= 0.1f;
            
        }
        isPlaying = false;
        PauseAudio();
    }
    // Function to pause the audio.
    public void PauseAudio()
    {
        if(audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }
}
