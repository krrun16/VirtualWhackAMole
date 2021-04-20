using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private bool scaleSet = false;
    private bool armlengthSet = false;
    private float newScale;
    float armLength;
    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        if (BodySourceView.getHeadToFootHeight() > 0 & armlengthSet == false)
        {
            armLength = BodySourceView.getHeadToFootHeight() / 2;
            transform.localPosition = new Vector3(0, armLength, 0);
            armlengthSet = true;
        }

        if (BodySourceView.getHipToHeadHeight() > 0 & scaleSet == false) {
            newScale = BodySourceView.getHipToHeadHeight() / 4f;
            transform.localScale = new Vector3(transform.localScale.x, newScale, newScale);
            scaleSet = true;
        }
    }
}
