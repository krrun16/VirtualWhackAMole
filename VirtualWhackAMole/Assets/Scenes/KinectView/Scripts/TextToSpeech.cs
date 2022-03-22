using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;

public class TextToSpeech : MonoBehaviour
{
    public AudioSource _audio;
    public static AudioSource[] numbers;


    void Start()
    {
        numbers = GetComponents<AudioSource>();
    }


    void OnEnable()
    {
        _audio = gameObject.GetComponent<AudioSource>();
        if (gameObject.scene.name == "GameScene")
        {
            StartCoroutine(DownloadTheAudio());
        }
        else if (gameObject.scene.name == "MenuScene")
        {
            StartCoroutine(DownloadTheInputAudio(ScreenReader.getState()));
        }
    }

    IEnumerator DownloadTheAudio()
    {
        //string result = "Your score is: ";
        string result = "";
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
    // Uses a integer (probably should be a bool) to check if we are reading the box or if we are typing in the box
    IEnumerator DownloadTheInputAudio(int state)
    {
        string inputFieldString;
        char inputFieldChar;
        string url;
        if (state == 1)
        {
            inputFieldString  = stringReciever.getPartNumber();
            print(inputFieldString);

            url = "https://translate.google.com/translate_tts?ie=UTF-8&client=tw-ob&tl=en&q=" + inputFieldString;
            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
            {
                yield return www.SendWebRequest();

               _audio.clip = DownloadHandlerAudioClip.GetContent(www);
               _audio.PlayDelayed(1.25f);
               yield return new WaitForSeconds(2f);


            }
        }
        else
        {
            inputFieldChar = stringReciever.getCharNumber();
            print(inputFieldChar);
            int inputFieldInt = (int)Char.GetNumericValue(inputFieldChar);

            url = "https://translate.google.com/translate_tts?ie=UTF-8&client=tw-ob&tl=en&q=" + inputFieldChar;

            using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
            {
                yield return www.SendWebRequest();

                // _audio.clip = DownloadHandlerAudioClip.GetContent(www);
                // _audio.Play();
                // add one bc the first audio is from textToSpeech and is blank
                numbers[inputFieldInt + 1].Play();
            }
        }
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }
}
