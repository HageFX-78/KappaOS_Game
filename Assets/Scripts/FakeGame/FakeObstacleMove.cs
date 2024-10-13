using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeObstacleMove : MonoBehaviour
{

    [Header("Auto Move")]
    [SerializeField] public float moveSpeed = 1f;

    [SerializeField] private float tpDistance = 5f;

    [Header("Auto Jump Config")]
    [SerializeField] private Transform jumpTrigger;
    [SerializeField] private float minTriggerXPos = 2f;
    [SerializeField] private float maxTriggerXPos = 5f;
    [SerializeField] private float minMoveSpeed = 5f;
    [SerializeField] private float maxMoveSpeed = 20f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AutoParallax();
    }

    private void AutoParallax(){
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        if (transform.position.x < -tpDistance){
            transform.position = new Vector3(tpDistance, transform.position.y, transform.position.z);
        }else if (transform.position.x > tpDistance){
            transform.position = new Vector3(-tpDistance, transform.position.y, transform.position.z);
        }
    }

    public void SetNewConfig(){
        moveSpeed = Mathf.Clamp(++moveSpeed, minMoveSpeed, maxMoveSpeed);

            // Calculate percentage of moveSpeed within its range
        float speedPercentage = (moveSpeed - minMoveSpeed) / (maxMoveSpeed - minMoveSpeed);
        //Debug.Log("Speed Percentage: " + speedPercentage);

        // Interpolate the new X position for jumpTrigger based on the speedPercentage
        float newXPos = Mathf.Lerp(minTriggerXPos, maxTriggerXPos, speedPercentage);
        //Debug.Log("New X Pos: " + newXPos);
        jumpTrigger.localPosition = new Vector3(-newXPos, jumpTrigger.position.y, jumpTrigger.position.z);
    }
}
