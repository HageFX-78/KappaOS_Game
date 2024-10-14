using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakePlayer : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float jumpForce = 2f;
    [SerializeField] private LayerMask groundLayer; // Define the layer(s) considered as ground
    [SerializeField] private Transform groundCheck; // An empty GameObject or position where we check if we're touching the ground
    [SerializeField] private float groundCheckRadius = 0.2f; // Radius of ground check

    [SerializeField] private float stressMultiplier = 0.1f;

    private bool isGrounded;

    void Update()
    {
        // Check if the player is grounded before jumping
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }
    }

    void Jump()
    {
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        AudioManager.amInstance.PlaySF("jump");
    }

    void OnCollisionEnter2D(Collision2D collision){

        if (collision.gameObject.CompareTag("JumpTrigger") && isGrounded){
            if(Random.Range(0, 101) >= GameManager.instance.Stress * stressMultiplier + GameManager.instance.AdditionalHackStressPercentage)
                Jump();
        }

        if (collision.gameObject.CompareTag("Obstacle")){
            //Time.timeScale = 0;
            GameManager.instance.ShakeGameCamera(0.2f);
            GameManager.instance.Stress += 5;
            GameManager.instance.FakePlayerHitObstacle();
            AudioManager.amInstance.PlaySF("hitob");
        }
    }

    void OnDrawGizmosSelected()
    {
        // Visualize the ground check in the editor
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
