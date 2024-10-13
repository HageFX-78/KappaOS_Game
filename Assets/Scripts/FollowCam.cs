using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    [SerializeField] public Transform cameraTr;
    void FixedUpdate()
    {
        transform.position = new Vector3(cameraTr.position.x, cameraTr.position.y, 0);       
    }
}
