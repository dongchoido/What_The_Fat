using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinning : MonoBehaviour
{
    private bool x;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("rotate",1f,1f);
    }
    private void rotate()
    {
        transform.Rotate(new Vector3(0,0,15));
    }
}
