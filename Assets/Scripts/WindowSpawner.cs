using System.Collections;
using TMPro;
using UnityEngine;

public class WindowSpawner : MonoBehaviour
{
    public GameObject windowPrefab; // Drag your Window prefab here in Inspector
    public float spawnInterval = 3f; // Spawn every 3 seconds

    public TextMeshProUGUI levelsTimerText;
    public TextMeshProUGUI levelText;

    public float fadeDuration = 1.0f;
    public float displayDuration = 3.0f;

    private float spawnTimer = 0f;
    private float levelsTimer = 0f;
    private int level = 1;
    private bool levelBreak = false;

    void Update()
    {
        spawnTimer += Time.deltaTime;
        levelsTimer += Time.deltaTime;
        levelsTimerText.text = levelsTimer.ToString();

        if (level == 1 && levelsTimer >= 10)
        {
            levelBreak = true;
            level = 2;
            StartCoroutine(NewLevelAnimation("Speed Up"));
        }
        if (level == 2 && levelsTimer >= 15)
        {
            levelBreak = true;
            level = 3;
            StartCoroutine(NewLevelAnimation("Warning"));
        }
        if (level == 3 && levelsTimer >= 20)
        {
            levelBreak = true;
            level = 4;
            StartCoroutine(NewLevelAnimation("Chaos"));
        }
        if (level == 4 && levelsTimer >= 25)
        {
            levelBreak = true;
            level = 5;
            StartCoroutine(NewLevelAnimation("Danger"));
        }


        // Check if it's time to spawn a new window
        if (spawnTimer >= spawnInterval && !levelBreak)
        {
            SpawnWindow();
            spawnTimer = 0f; // Reset the timer
        }
    }

    IEnumerator NewLevelAnimation(string levelName)
    {
        // Reset levels timer
        levelsTimer = 0f;
        levelText.text = levelName;

        while (GameObject.FindGameObjectWithTag("Window") != null)
        {
            yield return null;
        }

        // Fade In
        yield return Fade(0f, 1f, fadeDuration);

        // Wait while text is fully visible
        yield return new WaitForSeconds(displayDuration);

        // Fade Out
        yield return Fade(1f, 0f, fadeDuration);

        yield return new WaitForSeconds(1f);

        // Code here runs after the delay
        levelsTimer = 0f;
        levelBreak = false;
        spawnTimer = 0f;

    }

    private IEnumerator Fade(float startAlpha, float endAlpha, float duration)
    {
        float elapsedTime = 0f;
        Color color = levelText.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;

            // Calculate the new alpha value
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            levelText.color = color;

            yield return null; // Wait for the next frame before continuing
        }

        // Ensure the alpha is exactly the end value when finished
        color.a = endAlpha;
        levelText.color = color;
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