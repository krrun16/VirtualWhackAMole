using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = System.Random;
using System.Threading;


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
    private AudioSource missMoleSound;
    private AudioSource hipNote;
    private AudioSource stomachNote;
    private AudioSource chestNote;
    private AudioSource neckNote;
    private AudioSource headNote;
    private AudioSource[] moleSounds;
    

    public bool isHit;
    public bool isShown;
    public bool playingHint;
    public double timeHit;
    public static string handHit;

    public AudioSource awesome; 
    public AudioSource congrats;
    public AudioSource fantastic;
    public AudioSource great;
    public AudioSource success;

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
        missMoleSound = moleSounds[19];
        hipNote = moleSounds[28];
        stomachNote = moleSounds[29];
        chestNote = moleSounds[30];
        neckNote = moleSounds[31];
        headNote = moleSounds[32];
        isHit = false;
        isShown = false;
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
        isShown = false;
    }

    // Start mole movement towards new endPosition of showing
    public void ShowMole()
    {
        isShown = true;
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
        declarativeHint.Add(GetPianoNotes());
        declarativeHint.Add(GetDeclarativeHint());
        StartCoroutine(playAudioSequentially(declarativeHint));
    }

    public void GiveNoHint()
    {
        List<AudioSource> noHint = new List<AudioSource>();
        noHint.Add(GetPianoNotes());
        StartCoroutine(playAudioSequentially(noHint));
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
    public void HitMole(GameObject gameObject)                                               
    {
        if (endPosition != inPosition)
        {
            isHit = true;
            // TODO: determine exact position where mole is behind grid face
            if (transform.localPosition.x < -0.4f)
            {
                return;
            }
            
            // We only want to vibrate controller if mole that is hit is shown
            if (isShown)
                // Detect which hammer hit by comparing name of gameObject
                if (gameObject.name == "RightHammer")
                {
                    Debug.Log("gameObject right hammer conditional entered");
                    JoyconHammer.rightVibrate();
                    handHit = "right";
                }
                else
                {
                    JoyconHammer.leftVibrate();
                    handHit = "left";
                }
           
            //Retract Mole
            endPosition = inPosition;
            transform.localPosition = endPosition;
            isShown = false;

            DateTime dateTime = DateTime.Now;
            timeHit = dateTime.TimeOfDay.TotalMilliseconds;

            //Play hit sound, stop mole sound
            hitMoleSound.Play();
            StopShowSound();

            // 50% chance that we get a complimentary audio after a hit

            if (UnityEngine.Random.Range(1, 4) > 2)
            {
                switch (UnityEngine.Random.Range(0, 4))
                {
                    case 0:
                        awesome.Play();
                        break;
                    case 1:
                        fantastic.Play();
                        break;
                    case 2:
                        congrats.Play();
                        break;
                    case 3:
                        great.Play();
                        break;
                    case 4:
                        success.Play();
                        break;
                }
            }

            // Increment score twice because mole was hit
            GameController.incrementScore();
            GameController.incrementScore();
        }
    }

    public void MissMole()      
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

    // Returns the declarative hint
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

    //Returns spatialized piano notes for the correct elevation.
    private AudioSource GetPianoNotes()
    {
        if (transform.parent.localPosition.y < -1.5)
        {
            BucketSoundHorizontal(hipNote);
            return hipNote;
        }
        else if (transform.parent.localPosition.y >= -1.5 && transform.parent.localPosition.y < -.5)
        {
            BucketSoundHorizontal(stomachNote);
            return stomachNote;
        }
        else if (transform.parent.localPosition.y >= -.5 && transform.parent.localPosition.y < .5)
        {
            BucketSoundHorizontal(chestNote);
            return chestNote;
        }
        else if (transform.parent.localPosition.y >= .5 && transform.parent.localPosition.y < 1.5)
        {
            BucketSoundHorizontal(neckNote);
            return neckNote;
        }
        else
        {
            BucketSoundHorizontal(headNote);
            return headNote;
        }
    }

    // Returns a list of audio source instructions needed for imperative hint
    private List <AudioSource> GetImperativeHint() 
    {
        int[] squaresAway = SquareDistance();
        int squaresAwayVertical = squaresAway[0];
        int squaresAwayHorizontal = squaresAway[1];
        List<AudioSource> imperativeHint = new List<AudioSource>();
        imperativeHint.Add(GetPianoNotes());

        //Determine the hand to give instructions for
        if (transform.parent.localPosition.z >= -.5 && transform.parent.localPosition.z < .5) 
        {
            if (PlayerPrefs.GetString("DominantHand") == "Left") //if dominant hand is left:
            {
                if (UnityEngine.Random.Range(1, 4) > 2)     //if num in range greater than 2
                {
                    imperativeHint.Add(moleSounds[18]); // play "left hand"  
                }
                else //if num in range less than 2: play "right hand"
                {
                    imperativeHint.Add(moleSounds[17]);
                }
            }
            else
            {
                if (UnityEngine.Random.Range(1, 4) > 2)
                {
                    imperativeHint.Add(moleSounds[17]); // play "right hand"
                }
                else
                {
                    imperativeHint.Add(moleSounds[18]); //play "left hand"
                }
            }
        }
        else if (transform.parent.localPosition.z < -.5)
        {
            imperativeHint.Add(moleSounds[18]); //"left hand"
        }
        else
        {
            imperativeHint.Add(moleSounds[17]); //"right hand"
        }

        if (squaresAwayVertical == 0 && squaresAwayHorizontal == 0)
        {
            imperativeHint.Add(moleSounds[15]);
            return imperativeHint;
            // return You're in the right spot
        }
        
        // Accounts for when player hand is already at the correct elevation.
        if(squaresAwayVertical ==0)         
        {
            imperativeHint.Add(moleSounds[20]);
            return imperativeHint;
        }

        //if hand farther than 3 feet away from mole, lets user know.
        if (Math.Abs(squaresAwayVertical) > 6)  
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
        if (Math.Abs(squaresAwayVertical) == 6)        
        {
            // 3 feet audio clip
            imperativeHint.Add(moleSounds[27]);
        }
        if (Math.Abs(squaresAwayVertical) == 5)        
        {
            // 2.5 feet audio clip
            imperativeHint.Add(moleSounds[26]);
        }
        if (Math.Abs(squaresAwayVertical) == 4)
        {
            // 2 feet audio clip
            imperativeHint.Add(moleSounds[10]);
        }
        else if (Math.Abs(squaresAwayVertical) == 3)
        {
            // 1.5 feet audio clip
            imperativeHint.Add(moleSounds[9]);
        }
        else if (Math.Abs(squaresAwayVertical) == 2)
        {
            // 1 foot audio clip
            imperativeHint.Add(moleSounds[8]);
        }
        else if (Math.Abs(squaresAwayVertical) == 1)
        {
            // 6 inches audio clip
            imperativeHint.Add(moleSounds[7]);
        }

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

    public string getHandHit()
    {
        return handHit;
    }

}
