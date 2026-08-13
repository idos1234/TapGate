using UnityEngine;
using UnityEngine.InputSystem;

public class Window : MonoBehaviour
{
    public float moveUpSpeed = 3f; // Speed at which the window moves up
    private float destroyYPosition;

    private bool open; // Adjust speed in the Inspector
    private float gapPercentage = 0.0f; // Gap percentage of screen width (0.0 = no gap, 0.2 = 20% of screen width)

    public float heightPercentage = 0.05f; // 0.05 = 5% of screen height
    public float slideSpeed = 10f; // Speed of opening/closing animation

    public GameObject rightChild;
    public GameObject leftChild;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        open = Random.value > 0.5f; // Randomly set the initial state of the window (open or closed)
        if (!open)
        {
            gapPercentage = 0.2f; // 0.20 = 20% of screen width
        }
        else
        {
            gapPercentage = 0.0f; // no gap
        }

        open = !open;

        MatchScreenSize();
    }

    // Update is called once per frame
    void Update()
    {
        // Move the window upwards
        transform.position += Vector3.up * moveUpSpeed * Time.deltaTime;

        // Check for click/tap using the New Input System
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            if (!open)
            {
                gapPercentage = 0.2f; // 0.20 = 20% of screen width
            }
            else
            {
                gapPercentage = 0.0f; // no gap
            }

            open = !open;
        }


        MatchPosition();

        // Destroy the window if it moves above the screen
        if (transform.position.y >= destroyYPosition)
        {
            Destroy(gameObject);
        }
    }

    void MatchScreenSize()
    {
        //Calculate screen dimensions in Unity world units
        Camera mainCam = Camera.main;
        float screenHeightUnits = mainCam.orthographicSize * 2f;
        float screenWidthUnits = screenHeightUnits * mainCam.aspect;

        //Determine target width in world units
        float targetWidthUnits = screenWidthUnits;
        float targetHeightUnits = screenHeightUnits * heightPercentage;

        //Get actual size of the sprite asset
        SpriteRenderer child_sr = leftChild.GetComponent<SpriteRenderer>();
        float childOriginalSpriteWidth = child_sr.bounds.size.x;

        //Calculate local scale and apply it
        float widthScale = targetWidthUnits / childOriginalSpriteWidth;
        float heightScale = targetHeightUnits / childOriginalSpriteWidth;
        rightChild.transform.localScale = new Vector3(widthScale, heightScale, 1f);
        leftChild.transform.localScale = new Vector3(widthScale, heightScale, 1f);
    }

    void MatchPosition()
    {
        // Get the main camera
        Camera mainCam = Camera.main;

        // Calculate screen dimensions in Unity world units
        float screenHeightUnits = mainCam.orthographicSize * 2f;
        float screenWidthUnits = screenHeightUnits * mainCam.aspect;
        destroyYPosition = screenHeightUnits / 2f + (screenHeightUnits * 0.1f); // Adjust Y position to be above the screen

        // Calculate the gap
        float gap = screenWidthUnits * gapPercentage;

        // Calculate the target position for the left child (left edge of the screen)
        float targetLeftX = transform.position.x - screenWidthUnits / 2f - gap / 2f;
        Vector3 targetLeftPos = new Vector3(targetLeftX, transform.position.y, transform.position.z);
        
        // Calculate the target position for the right child (right edge of the screen)
        float targetRightX = transform.position.x + screenWidthUnits / 2f + gap / 2f;
        Vector3 targetRightPos = new Vector3(targetRightX, transform.position.y, transform.position.z);
        // Smoothly move current position towards target position
        leftChild.transform.position = Vector3.Lerp(leftChild.transform.position, targetLeftPos, Time.deltaTime * slideSpeed);
        rightChild.transform.position = Vector3.Lerp(rightChild.transform.position, targetRightPos, Time.deltaTime * slideSpeed);
    }
}
