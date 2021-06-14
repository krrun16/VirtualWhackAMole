using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//using System.Speech.Synthesis;
// Resources used:
// Mole design and intial code based off of Jamie Gant's Whack A Mole Tutorial videos
// youtube channel : JamieGantTechClass
//public sealed class SpeechSynthesizer : IDisposable
//{ }

public class Mole : MonoBehaviour
{
    
    public int timeMoleShowed;
    public int timeMoleHit;

    private float speed; 
    private Vector3 endPosition; 
    private Vector3 inPosition;

    private AudioSource hitMoleSound;
    private AudioSource showMoleSound;
    private AudioSource headHintSound;
    private AudioSource neckHintSound;
    private AudioSource chestHintSound;
    private AudioSource stomachHintSound;
    private AudioSource hipsHintSound;
    private AudioSource missMoleSound; //CHANGE 12JUN21
    private AudioSource[] moleSounds;
    public bool isHit;
    public bool playingHint;
    public double timeHit;

  



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
        missMoleSound = moleSounds[19];         //CHANGE 12JUN21
        isHit = false;
        playingHint = false;
        timeHit = 0;
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

    public void GiveDeclarativeHint()
    {
        List<AudioSource> declarativeHint = new List<AudioSource>();
        declarativeHint.Add(GetDeclarativeHint());
        StartCoroutine(playAudioSequentially(declarativeHint));
    }

    public void GiveImperativeHint()
    {
        List<AudioSource> imperativeHint = new List<AudioSource>();
        imperativeHint = GetImperativeHint();
        StartCoroutine(playAudioSequentially(imperativeHint));
    }

    // Plays audio sources in order and without overlap
    IEnumerator playAudioSequentially(List<AudioSource> hints)
    {
        yield return null;

        //1.Loop through each AudioSource
        foreach (AudioSource currentAudio in hints)
        {
            // Play Audio
            BucketSoundHorizontal(currentAudio);
            currentAudio.Play();
            // Wait for it to finish playing
            while (currentAudio.isPlaying)
            {
                yield return null;
            }
        }
        playingHint = false;
    }

    // Instantly retract mole and play hit mole sound
    public void HitMole()                                               
    {
        isHit = true;
        // TODO: determine exact position where mole is behind grid face
        if (transform.localPosition.x < -0.4f)
        {
            return;
        }
        //Retract Mole
        endPosition = inPosition;
        transform.localPosition = endPosition;

        DateTime dateTime = DateTime.Now;
        timeHit = dateTime.TimeOfDay.TotalMilliseconds;

        //Play hit sound, stop mole sound
        hitMoleSound.Play();

        StopShowSound();

        // Increment score twice because mole was hit
        GameController.incrementScore();
        GameController.incrementScore();
    }

    public void MissMole()      //CHANGE 12JUN21
    {
        missMoleSound.Play();
       
    }

    public void StopShowSound()
    {
        showMoleSound.Stop();
    }

    // Pans audio from left to right ear depending on 
    // horizontal mole position
    private void BucketSoundHorizontal(AudioSource usedSound) 
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

    // Returns the decaratice hint
    private AudioSource GetDeclarativeHint() 
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

    // Returns a list of audio source instructions needed for imperative hint
    private List<AudioSource> GetImperativeHint()
    {
        int[] squaresAway = SquareDistance();
        int squaresAwayVertical = squaresAway[0];
        int squaresAwayHorizontal = squaresAway[1];
        List<AudioSource> imperativeHint = new List<AudioSource>();

        //Determine the hand to give instructions for
        if (transform.parent.localPosition.z >= -.5 && transform.parent.localPosition.z < .5)
        {
            if (PlayerPrefs.GetString("DominantHand") == "Left")
            {
                imperativeHint.Add(moleSounds[18]);                              
                
            }
            else
            {
               imperativeHint.Add(moleSounds[17]);
                                                           
            }
        }
        else if (transform.parent.localPosition.z < -.5)
        {
            imperativeHint.Add(moleSounds[18]);
        }
        else
        {
            imperativeHint.Add(moleSounds[17]);
        }

        if (squaresAwayVertical == 0 && squaresAwayHorizontal == 0)
        {
            imperativeHint.Add(moleSounds[15]);
            return imperativeHint;
            // return You're in the right spot
        }

        if (Math.Abs(squaresAwayVertical) > 4 || Math.Abs(squaresAwayHorizontal) > 4)
        {
            imperativeHint.Add(moleSounds[16]);
            return imperativeHint;
            //return youre too far away
        }

        //VERTICAL
        if (squaresAwayVertical < 0)
        {
            imperativeHint.Add(moleSounds[12]);
            // Add Audio clip down
        }
        else if(squaresAwayVertical > 0 )
        {
            // Audio clip up
            imperativeHint.Add(moleSounds[11]);
        }

        if (Math.Abs(squaresAwayVertical) == 4)
        {
            // 4 audio clip
            imperativeHint.Add(moleSounds[10]);
        }
        else if (Math.Abs(squaresAwayVertical) == 3)
        {
            // 3 audio clip
            imperativeHint.Add(moleSounds[9]);
        }
        else if (Math.Abs(squaresAwayVertical) == 2)
        {
            // 2 audio clip
            imperativeHint.Add(moleSounds[8]);
        }
        else if (Math.Abs(squaresAwayVertical) == 1)
        {
            // 1 audio clip
            imperativeHint.Add(moleSounds[7]);
        }

        //CHANGE: COMMENT REMOVES LEFT-RIGHT IMPERATIVE DIRECTIONS
       
        return imperativeHint;
    }
    
    private int[] SquareDistance()
    {
        //What hammer to determine distance from
        GameObject hammerUsed;
        if (transform.parent.localPosition.z >= -.5 && transform.parent.localPosition.z < .5)
        {
            if (PlayerPrefs.GetString("DominantHand") == "Left")
            {
                hammerUsed = GameObject.FindGameObjectsWithTag("leftHammer")[0];
            }
            else
            {
                hammerUsed = GameObject.FindGameObjectsWithTag("rightHammer")[0];
            }
        }
        else if (transform.parent.localPosition.z < -.5)
        {
            hammerUsed = GameObject.FindGameObjectsWithTag("leftHammer")[0];
        }
        else
        {
            hammerUsed = GameObject.FindGameObjectsWithTag("rightHammer")[0];
        }

        float squareHeight = GameObject.Find("GridManager").transform.localScale.y;

        // Determine horizontal and vertical distance between hammer and mole as number of squares
        float verticalSquareDistance = (transform.parent.position.y - hammerUsed.transform.position.y) / squareHeight;
        float horizontalSquareDistance = (transform.parent.position.x - hammerUsed.transform.position.x) / squareHeight;
        int[] squareDistance = {(int)Math.Round(verticalSquareDistance), (int)Math.Round(horizontalSquareDistance)};

        return squareDistance;
    }
}
