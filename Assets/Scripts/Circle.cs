using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Circle: MonoBehaviour
{
    [Range(0.01f, 1f)]
    public float widthPercentage = 0.15f; // 0.15 = 15% of screen width
    public float moveSpeed = 15f; // Adjust speed in the Inspector
    private float targetX;

    void Start()
    {
        MatchScreenSize();
        MatchPosition();
        targetX = transform.position.x;
    }

    void Update()
    {
        // Check for click/tap using the New Input System
        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            // Get the screen position of the click/tap
            Vector2 screenPosition = Pointer.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPosition);
            targetX = worldPos.x;
        }

        // Smoothly move current X towards target X
        float newX = Mathf.MoveTowards(transform.position.x, targetX, moveSpeed * Time.deltaTime);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("WindowChild"))
        {
            Debug.Log("Collision detected with Window. Loading GameOverScene.");
            SceneManager.LoadScene("GameOverScene");
        }
    }

    void MatchScreenSize()
    {
        //Calculate screen dimensions in Unity world units
        Camera mainCam = Camera.main;
        float screenHeightUnits = mainCam.orthographicSize * 2f;
        float screenWidthUnits = screenHeightUnits * mainCam.aspect;

        //Determine target width in world units
        float targetWidthUnits = screenWidthUnits * widthPercentage;

        //Get actual size of the sprite asset
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        float originalSpriteWidth = sr.bounds.size.x;

        //Calculate local scale and apply it
        float desiredScale = targetWidthUnits / originalSpriteWidth;
        transform.localScale = new Vector3(desiredScale, desiredScale, 1f);
    }

    void MatchPosition()
    {
        // Get the main camera
        Camera mainCam = Camera.main;

        // Calculate screen dimensions in Unity world units
        float screenHeightUnits = mainCam.orthographicSize * 2f;

        // Calculate the target position (center of the screen)
        Vector3 targetPosition = new Vector3(0, screenHeightUnits / 4, 0); // Center of the screen in world units

        // Apply the position to the circle
        transform.position = targetPosition;
    }
}
