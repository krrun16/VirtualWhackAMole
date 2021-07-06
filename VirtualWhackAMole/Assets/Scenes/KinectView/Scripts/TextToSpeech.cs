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
        if (gameObject.scene.name == "GameScene")
        {
            StartCoroutine(DownloadTheAudio());
        }
        else if (gameObject.scene.name == "MenuScene")
        {
            StartCoroutine(DownloadTheInputAudio());
        }
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
    IEnumerator DownloadTheInputAudio()
    {
        
        char inputFieldTyped = stringReciever.getCharNumber();
        Debug.Log(inputFieldTyped);
        string url = "https://translate.google.com/translate_tts?ie=UTF-8&client=tw-ob&tl=en&q=" + inputFieldTyped;

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            _audio.clip = DownloadHandlerAudioClip.GetContent(www);
            _audio.Play();
        }
        yield return new WaitForSeconds(.45f);
        gameObject.SetActive(false);
    }
}
