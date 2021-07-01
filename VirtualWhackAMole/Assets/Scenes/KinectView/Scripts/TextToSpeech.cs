using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class TextToSpeech : MonoBehaviour
{
    public AudioSource _audio;
    

    void OnEnable()
    {
        _audio = gameObject.GetComponent<AudioSource>();
        StartCoroutine(DownloadTheAudio());
    }

    IEnumerator DownloadTheAudio()
    {
        string result = "Your score is: ";
        int score = GameController.getScore();
        // avoid speech synthesis interperting zero as the letter 'o'
        if (score == 0)
        {
            string avoid = "zero";
            result = result + avoid;
        }
        else
        {
            result = result + score;
        }

        string url = "https://translate.google.com/translate_tts?ie=UTF-8&client=tw-ob&tl=en&q=" + result;

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            _audio.clip = DownloadHandlerAudioClip.GetContent(www);
            _audio.Play();
        }   
    }
    
}
