using UnityEngine;
using UnityEngine.InputSystem;

public class CoinWindow : MonoBehaviour
{
    private float heightPercentage = 0.05f; // 0.05 = 5% of screen height
    private float destroyYPosition;
    private float slideSpeed = 10f; // Speed of opening/closing animation

    private bool open; // Adjust speed in the Inspector
    private float gapPercentage = 0f; // Gap percentage of screen width
    private float gapOpen = 0.37f;

    public GameObject rightChild;
    public GameObject leftChild;
    public GameObject middleChild;
    public GameObject coin;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MatchScreenSize();


        // Get the main camera
        Camera mainCam = Camera.main;

        // Calculate screen dimensions in Unity world units
        float screenHeightUnits = mainCam.orthographicSize * 2f;
        float screenWidthUnits = screenHeightUnits * mainCam.aspect;

        // Calculate the target position for the coin
        float targetCoinX = screenWidthUnits / 8 + coin.transform.localScale.y + gapOpen / 2f;
        targetCoinX = UnityEngine.Random.Range(0, 2) == 0 ? -1 * targetCoinX : targetCoinX;
        Vector3 targetCoinPos = new Vector3(targetCoinX, transform.position.y, transform.position.z);

        coin.transform.position = targetCoinPos;

    }

    // Update is called once per frame
    void Update()
    {
        // Move the window upwards
        //transform.position += Vector3.up * moveUpSpeed * Time.deltaTime;

        // Check for click/tap using the New Input System
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            if (!open)
            {
                gapPercentage = gapOpen;
            }
            else
            {
                gapPercentage = 0.0f; // no gap
            }

            open = !open;
        }

        MatchPosition();
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
        float targetLeftX = -screenWidthUnits / 2f - screenWidthUnits / 8 - gap;
        Vector3 targetLeftPos = new Vector3(targetLeftX, transform.position.y, transform.position.z);

        // Calculate the target position for the right child (right edge of the screen)
        float targetRightX = screenWidthUnits / 2f + screenWidthUnits / 8 + gap;
        Vector3 targetRightPos = new Vector3(targetRightX, transform.position.y, transform.position.z);

        // Smoothly move current position towards target position
        leftChild.transform.position = Vector3.Lerp(leftChild.transform.position, targetLeftPos, Time.deltaTime * slideSpeed);
        rightChild.transform.position = Vector3.Lerp(rightChild.transform.position, targetRightPos, Time.deltaTime * slideSpeed);
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
        middleChild.transform.localScale = new Vector3(widthScale / 4, heightScale, 1f);
        coin.transform.localScale = new Vector3(heightScale, heightScale, 1f);
    }
}
