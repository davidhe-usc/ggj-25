using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] AudioMixerSnapshot menuSnapshot, coldOpenSnapshot, cafeSnapshot, storeSnapshot, streetSnapshot, bedroomSnapshot, swingSnapshot, endingSnapshot;
    [SerializeField] GameObject stingerObject;
    [SerializeField] AudioSource stingerSource;
    [SerializeField] AudioClip selflessStinger, neutralStinger, badStinger;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        stingerSource = stingerObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;

        if (sceneName == "Main Menu")
        {
            menuSnapshot.TransitionTo(2f);
        }
        if (sceneName == "Intro")
        {
            cafeSnapshot.TransitionTo(2f);
        }
        if (sceneName == "Pre2Interstitial")
        {
            storeSnapshot.TransitionTo(2f);
        }
        if (sceneName == "Pre3Interstitial")
        {
            streetSnapshot.TransitionTo(2f);
        }
        if (sceneName == "Pre4Interstitial")
        {
            bedroomSnapshot.TransitionTo(2f);
        }
        if (sceneName == "Pre5Interstitial")
        {
            swingSnapshot.TransitionTo(2f);
        }

        //TODO: Add functionality for ending snapshot, as well as cold open -> cafe if we have it?
    }
}
