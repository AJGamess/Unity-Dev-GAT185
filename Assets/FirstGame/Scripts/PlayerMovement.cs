using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public LayerMask Death;
    public LayerMask Win;
    bool grounded;
    bool death;
    [HideInInspector] public bool winner;

    [Header("Music/SFX")]
    [SerializeField] AudioClip music;
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip ouch;
    private AudioSource audioSource;

    [Header("Other")]
    [SerializeField] Transform orientation;
    [SerializeField] Rigidbody rb;
    public int lives = 3;
    [SerializeField] TMP_Text livesText;
    float horizontalInput;
    float verticalInput;
    public Vector3 spawnPoint;
    Vector3 moveDirection;

    void Start()
    {
        spawnPoint = transform.position;

        rb.GetComponent<Rigidbody>();
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

    void Update()
    {
        livesText.text = "Lives: " + lives.ToString();

        // Ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, floor);

        // Death check
        death = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, Death);

        // Win check
        winner = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, Win);

        MyInput();
        SpeedControl();

        // Handle drag
        rb.linearDamping = grounded ? groundDrag : 0;

        // Handle death
        if (death)
        {
            --lives;
            transform.position = spawnPoint;
            PlayOuchSound();
        }
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

        if (Input.GetKey(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            PlayJumpSound();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.AddForce(moveDirection.normalized * speed * 10f * (grounded ? 1f : airMultiplier), ForceMode.Force);
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
