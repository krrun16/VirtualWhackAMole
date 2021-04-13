using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private bool scaleSet = false;
    private float newScale;
    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (BodySourceView.getHipToHeadHeight() > 0 & scaleSet == false) {
            newScale = BodySourceView.getHipToHeadHeight() / 4f;
            transform.localScale = new Vector3(transform.localScale.x, newScale, newScale);
            scaleSet = true;
        }
    }
}
