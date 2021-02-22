using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    // Show and Hide Variables
    public float visibleZHeight = -.305f;
    public float hiddenZHeight = -.012f;
    private Vector3 myNewXYZPosition;
    public float speed = 3f; //speed the mole moves

    void Awake()
    {
        HideMole();

        //set current position
        //transform.localPosition = myNewXYZPosition;
        myNewXYZPosition = transform.localPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move mole to new  XYZ position
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            myNewXYZPosition,
            Time.deltaTime * speed
            );
    }

    public void HideMole()
    {
        transform.localPosition = new Vector3(
            transform.localPosition.x,
            transform.localPosition.y,
            hiddenZHeight);  
    }

    public void ShowMole()
    {
        // set current position to visible Z height
        transform.localPosition = new Vector3(
            transform.localPosition.x,
            transform.localPosition.y,
            visibleZHeight);
    }
}
