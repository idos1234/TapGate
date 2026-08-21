using System.Collections;
using TMPro;
using UnityEngine;

public class WindowSpawner : MonoBehaviour
{
    public GameObject windowPrefab; // Drag your Window prefab here in Inspector
    public GameObject coinWindowPrefab; // Drag your Window prefab here in Inspector

    public TextMeshProUGUI levelText;

    public float fadeDuration = 1.0f;
    public float displayDuration = 3.0f;

    private float spawnTimer = 0f;
    private float levelsTimer = 0f;
    private int level = 1;
    private bool levelBreak = false;

    private float windowSpeed = 4f;
    private float delaySpawn = 2f;
    private float gapPercentage = 0.525f;

    private int windowsSinceLastCoin = 0;

    void Update()
    {

        spawnTimer += Time.deltaTime;
        levelsTimer += Time.deltaTime;

        if (level == 1 && levelsTimer >= 10)
        {
            levelBreak = true;
            level = 2;
            windowSpeed = 4f;
            delaySpawn = 1.5f;
            gapPercentage = 0.45f;
            StartCoroutine(NewLevelAnimation("Speed Up"));
        }
        if (level == 2 && levelsTimer >= 15)
        {
            levelBreak = true;
            level = 3;
            windowSpeed = 5f;
            delaySpawn = 1.2f;
            gapPercentage = 0.375f;
            StartCoroutine(NewLevelAnimation("Warning"));
        }
        if (level == 3 && levelsTimer >= 20)
        {
            levelBreak = true;
            level = 4;
            windowSpeed = 6f;
            delaySpawn = 1f;
            gapPercentage = 0.315f;
            StartCoroutine(NewLevelAnimation("Chaos"));
        }
        if (level == 4 && levelsTimer >= 25)
        {
            levelBreak = true;
            level = 5;
            windowSpeed = 7f;
            delaySpawn = 0.85f;
            gapPercentage = 0.27f;
            StartCoroutine(NewLevelAnimation("Danger"));
        }


        // Check if it's time to spawn a new window
        if (spawnTimer >= delaySpawn && !levelBreak)
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
        float coinChance = 0.05f * windowsSinceLastCoin;
        
        if(Random.value < coinChance || windowsSinceLastCoin >= 10)
        {
            SpawnCoinWindow();
            windowsSinceLastCoin = 0; // Reset
        } else
        {
            SpawnDefaultWindow();
            ++windowsSinceLastCoin;
        }
    }
    
    void SpawnCoinWindow()
    {
        // Get the main camera
        Camera mainCam = Camera.main;

        // Calculate screen dimensions in Unity world units
        float screenHeightUnits = mainCam.orthographicSize * 2f;
        float screenWidthUnits = screenHeightUnits * mainCam.aspect;

        float targetY = -screenHeightUnits / 2f - (screenHeightUnits * 0.1f); // Adjust Y position to be at the bottom of the screen


        // Create the window
        GameObject newWindow = Instantiate(coinWindowPrefab, new Vector3(0f, targetY, 0f), Quaternion.identity);

        // Pass speed and spawn
        CoinWindow coinWindowScript = newWindow.GetComponent<CoinWindow>();
        if (coinWindowScript != null)
        {
            coinWindowScript.Initialize(windowSpeed, gapPercentage);
        }
    }
    void SpawnDefaultWindow()
    {
        // Get the main camera
        Camera mainCam = Camera.main;

        // Calculate screen dimensions in Unity world units
        float screenHeightUnits = mainCam.orthographicSize * 2f;
        float screenWidthUnits = screenHeightUnits * mainCam.aspect;

        float targetX = Random.Range(-screenWidthUnits / 3f, screenWidthUnits / 3f);
        float targetY = -screenHeightUnits / 2f - (screenHeightUnits * 0.1f); // Adjust Y position to be at the bottom of the screen


        // Create the window
        GameObject newWindow = Instantiate(windowPrefab, new Vector3(targetX, targetY, 0f), Quaternion.identity);

        // Pass speed and spawn
        Window windowScript = newWindow.GetComponent<Window>();
        if (windowScript != null)
        {
            windowScript.Initialize(windowSpeed, gapPercentage);
        }
    }
}