using UnityEngine;

public class WindowSpawner : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject windowPrefab; // Drag your Window prefab here in Inspector
    public float spawnInterval = 3f; // Spawn every 3 seconds

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        // Check if it's time to spawn a new window
        if (timer >= spawnInterval)
        {
            SpawnWindow();
            timer = 0f; // Reset the timer
        }
    }

    void SpawnWindow()
    {
        // Get the main camera
        Camera mainCam = Camera.main;

        // Calculate screen dimensions in Unity world units
        float screenHeightUnits = mainCam.orthographicSize * 2f;
        float screenWidthUnits = screenHeightUnits * mainCam.aspect;

        float targetX = Random.Range(-screenWidthUnits / 3f, screenWidthUnits / 3f);
        float targetY = -screenHeightUnits / 2f - (screenHeightUnits * 0.1f); // Adjust Y position to be at the bottom of the screen


        // Instantiate (spawn) the window prefab in the scene
        Instantiate(windowPrefab, new Vector3(targetX, targetY, 0f), Quaternion.identity);
    }
}