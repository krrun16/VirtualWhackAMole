using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Resources used:
// Mole design and intial code based off of Jamie Gant's Whack A Mole Tutorial videos
// youtube channel : JamieGantTechClass


public class Mole : MonoBehaviour
{
    private float speed; 
    private Vector3 endPosition; 
    private Vector3 inPosition;
    private AudioSource hitMoleSound;
    private AudioSource showMoleSound;
    private AudioSource[] moleSounds;


    // Start is called before the first frame update
    void Start()
    {
        // Set both endPosition and inPosition to the starting position
        // to prevent movement at start
        inPosition = transform.localPosition;
        endPosition = inPosition;
        speed = 1.5f;

        // Get sounds
        moleSounds = GetComponents<AudioSource>();
        hitMoleSound = moleSounds[0];
        showMoleSound = moleSounds[1];
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, endPosition,
            Time.deltaTime * speed);
    }

    // Retract the mole back towards the start position
    public void HideMole()
    {
        endPosition = inPosition;
    }

    // Start mole movement towards new endPosition of showing
    //BUG: Mole can not show in game but plays sound. Yet to recreeate consistently.
    public void ShowMole()
    {
        //Show mole
        endPosition.x += 1.5f;

        //Play Sound
        showMoleSound.PlayDelayed(.25f);
    }

    // On mouse click stop movement and return to start position
    private void OnMouseDown()
    {
        HitMole(); 
    }

    // Instantly retract mole and play hit mole sound
    public void HitMole()
    {
        if (transform.localPosition.x < -0.4f)
        {
            return;
        }
        //Retract Mole
        endPosition = inPosition;
        transform.localPosition = endPosition;

        //Play hit sound, stop mole sound
        hitMoleSound.Play();
        StopShowSound();

        // Increment score 
        GameController.incrementScore();
    }

    public void StopShowSound()
    {
        showMoleSound.Stop();
    }

}
