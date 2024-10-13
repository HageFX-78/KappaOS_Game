using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Parallax : MonoBehaviour
{
    private float length, startposX;
    [SerializeField] public GameObject cam;
    [SerializeField] public float parallaxEffectX;

    [Header("Auto Move")]
    [SerializeField] public float moveSpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        //startposX = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AutoParallax();
    }

    private void MoveParallax()
    {
        float temp = (cam.transform.position.x * (1 - parallaxEffectX));
        float distance = (cam.transform.position.x * parallaxEffectX);


        transform.position = new Vector3(startposX + distance, transform.position.y, transform.position.z);

        if (temp > length + startposX) startposX += length;
        else if (temp < startposX - length) startposX -= length;
    }

    private void AutoParallax(){
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        if (transform.position.x < -length){
            transform.position = new Vector3(length, transform.position.y, transform.position.z);
        }else if (transform.position.x > length){
            transform.position = new Vector3(-length, transform.position.y, transform.position.z);
        }
    }
}