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

    private AudioSource declarativeHint;
    private AudioSource hitMoleSound;
    private AudioSource showMoleSound;
    private AudioSource headHintSound;
    private AudioSource neckHintSound;
    private AudioSource chestHintSound;
    private AudioSource stomachHintSound;
    private AudioSource hipsHintSound;
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
        // TODO : grab sounds by name 
        moleSounds = GetComponents<AudioSource>();
        hitMoleSound = moleSounds[0];
        showMoleSound = moleSounds[1];
        headHintSound = moleSounds[2];
        chestHintSound = moleSounds[3];
        neckHintSound = moleSounds[4];
        stomachHintSound = moleSounds[5];
        hipsHintSound = moleSounds[6];

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

        // Pan audio 
        BucketSoundHorizontal(showMoleSound);

        //Play Sounds
        showMoleSound.PlayDelayed(.25f);
    }

    public void GiveHint(string hintType)
    {
        AudioSource hintUsed;
        if (hintType == "Declarative")
        {
            hintUsed = GetDeclarativeHint();
        }
        // use this for imperative hints
        else
        {
            return;
        }
        BucketSoundHorizontal(hintUsed);
        hintUsed.Play();
    }



    // On mouse click stop movement and return to start position
    private void OnMouseDown()
    {
        HitMole(); 
    }

    // Instantly retract mole and play hit mole sound
    public void HitMole()
    {
        // TODO: determine exact position where mole is behind grid face
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

    public void BucketSoundHorizontal(AudioSource usedSound) 
    {
        if (transform.parent.localPosition.z < -1.5)
        {
            usedSound.panStereo = -1;
        }
        else if (transform.parent.localPosition.z >= -1.5 && transform.parent.localPosition.z < -.5)
        {
            usedSound.panStereo = -0.5f;
        }
        else if (transform.parent.localPosition.z >= -.5 && transform.parent.localPosition.z < .5)
        {
            usedSound.panStereo = 0;
        }
        else if (transform.parent.localPosition.z >= .5 && transform.parent.localPosition.z < 1.5)
        {
            usedSound.panStereo = 0.5f;
        }
        else if (transform.parent.localPosition.z >= 1.5)
        {
            usedSound.panStereo = 1;
        }
    }

    public AudioSource GetDeclarativeHint() 
    {
        if (transform.parent.localPosition.y < -1.5)
        {
            return hipsHintSound;
        }
        else if (transform.parent.localPosition.y >= -1.5 && transform.parent.localPosition.y < -.5)
        {
            return stomachHintSound;
        }
        else if (transform.parent.localPosition.y >= -.5 && transform.parent.localPosition.y < .5)
        {
            return chestHintSound;
        }
        else if (transform.parent.localPosition.y >= .5 && transform.parent.localPosition.y < 1.5)
        {
            return neckHintSound;
        }
        else
        {
            return headHintSound;
        }
    }

}
