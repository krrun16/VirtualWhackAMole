using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    private Vector3 leftHipVector;
    private Vector3 rightHipVector;
    private Vector3 leftFootVector;

    public Plane(Vector3 leftHipVector, Vector3 rightHipVector, Vector3 leftFootVector)
    {
        this.leftHipVector = leftHipVector;
        this.rightHipVector = rightHipVector;
        this.leftFootVector = leftFootVector;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal class Set3Points : Plane
    {
        public Set3Points(Vector3 leftHipVector, Vector3 rightHipVector, Vector3 leftFootVector) : base(leftHipVector, rightHipVector, leftFootVector)
        {
        }
    }
}
