using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mole : MonoBehaviour
{
    private float speed; 
    private Vector3 endPosition; 
    private Vector3 inPosition;

    // Start is called before the first frame update
    void Start()
    {
        // Set both endPosition and inPosition to the starting position
        // to prevent movement at start
        inPosition = transform.localPosition;
        endPosition = inPosition;
        speed = 1.5f;
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
    public void ShowMole()
    {
        endPosition.x += 1.5f;
    }

    // On mouse click stop movement and return to start position
    private void OnMouseDown()
    {
        endPosition = inPosition;
        transform.localPosition = endPosition;
    }

}
