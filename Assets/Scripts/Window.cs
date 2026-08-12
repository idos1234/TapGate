using UnityEngine;
using UnityEngine.InputSystem;

public class Window : MonoBehaviour
{
    private float widthPercentage = 0.5f; // 0.15 = 15% of screen width
    private bool open = false; // Adjust speed in the Inspector
    private float gapPercentage = 0.0f; // 0.20 = 20% of screen width

    public float heightPercentage = 0.05f; // 0.05 = 5% of screen height
    public float slideSpeed = 10f; // Speed of opening/closing animation

    public GameObject rightChild;
    public GameObject leftChild;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MatchScreenSize();
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    void MatchScreenSize()
    {
        //Calculate screen dimensions in Unity world units
        Camera mainCam = Camera.main;
        float screenHeightUnits = mainCam.orthographicSize * 2f;
        float screenWidthUnits = screenHeightUnits * mainCam.aspect;

        //Determine target width in world units
        float targetWidthUnits = screenWidthUnits * widthPercentage;
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

        //Get actual size of child
        SpriteRenderer child_sr = leftChild.GetComponent<SpriteRenderer>();
        float childWidth = child_sr.bounds.size.x;

        // Calculate the gap
        float gap = screenWidthUnits * gapPercentage;

        // Calculate the target position for the left child (left edge of the screen)
        float targetLeftX = -screenWidthUnits / 2f + childWidth / 2f - gap / 2f;
        Vector3 targetLeftPos = new Vector3(targetLeftX, 0f, 0f);
        
        // Calculate the target position for the right child (right edge of the screen)
        float targetRightX = screenWidthUnits / 2f - childWidth / 2f + gap / 2f;
        Vector3 targetRightPos = new Vector3(targetRightX, 0f, 0f);

        // Smoothly move current position towards target position
        leftChild.transform.position = Vector3.Lerp(leftChild.transform.position, targetLeftPos, Time.deltaTime * slideSpeed);
        rightChild.transform.position = Vector3.Lerp(rightChild.transform.position, targetRightPos, Time.deltaTime * slideSpeed);
    }
}
