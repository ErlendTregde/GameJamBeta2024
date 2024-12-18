using System.Collections;
using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    public GameObject[] notePrefabs; // Array of note prefabs
    public Transform spawnPoint; // The location where notes will spawn
    public Transform target; // The target the notes will move towards
    public AudioSource song; // Reference to the song
    public float songDuration = 105f; // Set this to match the actual musical duration (excluding silence)

    private bool hasGameStarted = false; // To track if the game has started
    private RhythmManager rhythmManager;

    private float[][] versePatterns = {
        new float[] { 0.612f, 0.306f, 0.153f }, // Pattern 1: quarter, eighth, sixteenth
        new float[] { 0.612f, 0.612f, 0.153f }, // Pattern 2: quarter, quarter, sixteenth
        new float[] { 0.612f, 0.306f, 0.306f }  // Pattern 3: quarter, eighth, eighth
    };

    private float[][] chorusPatterns = {
        new float[] { 0.306f, 0.153f, 0.153f }, // Chorus pattern: eighth, sixteenth, sixteenth
        new float[] { 0.153f, 0.306f, 0.153f }, // Chorus pattern: sixteenth, eighth, sixteenth
        new float[] { 0.306f, 0.306f, 0.153f }  // Chorus pattern: eighth, eighth, sixteenth
    };

    private float[][] bridgePatterns = {
        new float[] { 0.612f, 1.224f }, // Bridge pattern: quarter, half notes
        new float[] { 1.224f, 0.612f }, // Bridge pattern: half, quarter notes
    };

    private float[] currentPattern;
    private int patternIndex = 0;

    private void Start()
    {
        rhythmManager = FindObjectOfType<RhythmManager>();
        song = GetComponent<AudioSource>();

        if (song == null)
        {
            Debug.LogError("AudioSource is missing! Please attach an AudioSource component.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !hasGameStarted)
        {
            hasGameStarted = true;
            song.Play(); // Play the song
            StartCoroutine(SpawnNotes()); // Start spawning notes
        }
    }

    private IEnumerator SpawnNotes()
    {
        while (hasGameStarted)
        {
            float songTime = song.time; // Get the current time of the song

            // Stop spawning notes if the song reaches its duration
            if (!song.isPlaying || songTime >= songDuration)
            {
                Debug.Log("Song has ended. Stopping note spawn.");
                yield break; // Stop the coroutine if the song has ended or reached the duration
            }

            // Select the pattern based on the song time and section
            if (songTime < 5f) // Intro (0:00 - 0:05)
            {
                yield return new WaitForSeconds(5f - songTime); // Wait until the verse starts
                continue;
            }
            else if (songTime < 35f) // Verse 1 (0:05 - 0:35)
            {
                currentPattern = versePatterns[Random.Range(0, versePatterns.Length)];
            }
            else if (songTime < 40f) // Chorus (0:35 - 0:40)
            {
                currentPattern = chorusPatterns[Random.Range(0, chorusPatterns.Length)];
            }
            else if (songTime < 70f) // Verse 2 (0:40 - 1:10)
            {
                currentPattern = versePatterns[Random.Range(0, versePatterns.Length)];
            }
            else if (songTime < 75f) // Chorus (1:10 - 1:15)
            {
                currentPattern = chorusPatterns[Random.Range(0, chorusPatterns.Length)];
            }
            else if (songTime < 85f) // Bridge (1:15 - 1:25)
            {
                currentPattern = bridgePatterns[Random.Range(0, bridgePatterns.Length)];
            }
            else // Final Verse (1:25 - end)
            {
                currentPattern = versePatterns[Random.Range(0, versePatterns.Length)];
            }

            // Spawn the next note in the pattern
            yield return new WaitForSeconds(currentPattern[patternIndex]);

            int randomIndex = Random.Range(0, notePrefabs.Length);
            GameObject selectedPrefab = notePrefabs[randomIndex];

            if (selectedPrefab != null)
            {
                GameObject note = Instantiate(selectedPrefab, spawnPoint.position, Quaternion.identity);
                NoteMovement noteMovement = note.GetComponent<NoteMovement>();

                if (noteMovement != null && target != null)
                {
                    noteMovement.target = target;
                    noteMovement.rhythmManager = rhythmManager; // Assign the RhythmManager to each note
                }
                else
                {
                    Debug.LogWarning("NoteMovement script or target is not assigned correctly!");
                    Destroy(note); // Destroy the note if something went wrong
                }
            }

            // Move to the next note in the pattern
            patternIndex = (patternIndex + 1) % currentPattern.Length;
        }
    }
}
