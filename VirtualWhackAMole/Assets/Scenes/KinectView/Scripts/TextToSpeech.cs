using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextToSpeech : MonoBehaviour
{
    public AudioSource _audio;

    // Start is called before the first frame update
    void OnEnable()
    {
        _audio = gameObject.GetComponent<AudioSource>();
        StartCoroutine(DownloadTheAudio());
    }

    IEnumerator DownloadTheAudio()
    {
        int score = GameController.getScore();
        string result = "Your score is: ";
        result = result + score;
        string url = "https://translate.google.com/translate_tts?ie=UTF-8&client=tw-ob&tl=en&q=" + result;
        WWW www = new WWW(url);
        yield return www;
        _audio.clip = www.GetAudioClip(false, true, AudioType.MPEG);
        _audio.Play();
    }
}
