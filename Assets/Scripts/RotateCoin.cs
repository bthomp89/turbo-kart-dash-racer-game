using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCoin : MonoBehaviour
{
    public int rotateSpeed;
    // Start is called before the first frame update

    // Update is called once per frame
    void Start()
    {
        rotateSpeed = 1;
    }
    void Update()
    {
        transform.Rotate(0, rotateSpeed, 0, Space.World);
    }
}