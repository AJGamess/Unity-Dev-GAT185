using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float groundDrag;
    [SerializeField] float jumpForce;
    [SerializeField] float jumpCooldown;
    [SerializeField] float airMultiplier;
    bool readyToJump;

    [Header("Ground Check")]
    [SerializeField] float playerHeight;
    public LayerMask floor;
    public LayerMask movingFloor;
    public LayerMask DeathFloor;
    public LayerMask DeathObstacle;
    public LayerMask SceneFlip;
    public LayerMask Win;
    bool grounded;
    bool movingPlatform;
    bool deathFloor;
    bool deathObstacle;
    bool canDie = true;
    [HideInInspector] public bool winner;
    [HideInInspector] public bool nextLevel;

    [Header("Music/SFX")]
    [SerializeField] AudioClip music;
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip ouch;
    [SerializeField] AudioClip coin;
    [SerializeField] AudioClip speedSpell;
    private AudioSource audioSource;

    [Header("Other")]
    [SerializeField] Transform orientation;
    [SerializeField] Rigidbody rb;
    public int lives = 3;
    [SerializeField] TMP_Text livesText;
    public int coins = 0;
    [SerializeField] TMP_Text coinsText;
    float horizontalInput;
    float verticalInput;
    public Vector3 spawnPoint;
    Vector3 moveDirection;

    float originalSpeed = 0;
    // To hold the platform's velocity
    private Vector3 platformVelocity;

    void Start()
    {
        spawnPoint = transform.position;
        rb.freezeRotation = true;
        readyToJump = true;

        // Get or Add an AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Play background music
        PlayBackgroundMusic();
    }
    void OnCollisionEnter(Collision collision)
    {
        // Check if the object the player collided with is in the DeathObstacle LayerMask
        if (((1 << collision.gameObject.layer) & DeathObstacle) != 0)
        {
            deathObstacle = true;
            Debug.Log("Death Obstacle");
        }
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Speed"))
        {
            Debug.Log("Speed Boost");
            audioSource.PlayOneShot(speedSpell);
            StartCoroutine(SpeedBoostRoutine()); // Start the temporary speed boost
            Destroy(other.gameObject); // Remove the item after pickup
        }
        if (other.CompareTag("Coin"))
        {
            Debug.Log("Coin Collected");
            audioSource.PlayOneShot(coin);
            coins++;
            coinsText.text = "Coins: " + coins.ToString();
            Destroy(other.gameObject); // Remove the item after pickup
        }
    }

    private IEnumerator SpeedBoostRoutine()
    {
        originalSpeed = speed; // Store the original speed
        speed *= 2; // Double the speed

        yield return new WaitForSeconds(5f); // Speed boost lasts 5 seconds

        speed = originalSpeed; // Reset speed back to normal
    }

    void Update()
    {
        livesText.text = "Lives: " + lives.ToString();

        // Ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, floor);

        // Moving platform check
        movingPlatform = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, movingFloor);

        // Death check
        deathFloor = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, DeathFloor);

        // Win check
        winner = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, Win);

        // Scene Flip check
        nextLevel = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, SceneFlip);

        // Check if stepping on SceneFlip Layer
        if (nextLevel)
        {
            LoadNextLevel();
        }

        MyInput();
        SpeedControl();

        // Handle drag
        rb.linearDamping = (grounded && !movingPlatform) ? groundDrag : 0;

        // Handle death
        if (canDie && (deathFloor || deathObstacle))
        {
            if (originalSpeed != 0)
            {
                speed = originalSpeed; // Reset speed back to normal
            }
            StartCoroutine(HandleDeath());
        }
    }

    void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels! Game Completed.");
        }
    }

    IEnumerator HandleDeath()
    {
        canDie = false; // Prevent multiple deaths
        deathObstacle = false; // Reset death obstacle

        --lives;
        PlayOuchSound();

        transform.position = spawnPoint; // Respawn the player

        yield return new WaitForSeconds(2f); // Delay before allowing death again

        canDie = true;  // Allow dying again
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > speed)
        {
            Vector3 limitedVel = flatVel.normalized * speed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Space) && readyToJump && (grounded || movingPlatform))
        {
            readyToJump = false;
            transform.parent = null;
            Jump();
            PlayJumpSound();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        // If the player is on a moving platform, get the platform's velocity
        if (transform.parent != null && ((1 << transform.parent.gameObject.layer) & movingFloor) != 0)
        {
            platformVelocity = transform.parent.GetComponent<Rigidbody>().linearVelocity;
        }
        else
        {
            platformVelocity = Vector3.zero;
        }

        // Player input movement
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Add platform's velocity to player movement if on a moving platform
        rb.AddForce(moveDirection.normalized * speed * 10f * (grounded ? 1f : airMultiplier) + platformVelocity, ForceMode.Force);
    }

    private void Jump()
    {
        rb.linearVelocity = Vector3.zero;
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    // Play Background Music
    private void PlayBackgroundMusic()
    {
        if (music != null)
        {
            audioSource.clip = music;
            audioSource.loop = true;
            audioSource.volume = 0.5f; // Adjust volume if needed
            audioSource.Play();
        }
    }

    // Play Jump Sound
    private void PlayJumpSound()
    {
        if (jump != null)
        {
            audioSource.PlayOneShot(jump);
        }
    }

    // Play Ouch Sound (Death)
    private void PlayOuchSound()
    {
        if (ouch != null)
        {
            audioSource.PlayOneShot(ouch);
        }
    }
}
