using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TargetManager : MonoBehaviour
{
    public GameObject[] TargetsPrefabs;
    public Transform[] SpawnPoints;
    public float WaveTime;
    public TextMeshProUGUI timeText;
    public SimpleShoot simpleShoot;

    //time shit
    private float timeSinceLastSpawn;
    private float spawnTime;
    private bool waveActive;
    private float waveStartTime;
    private int waveCount;
    private void Awake()
    {
        timeSinceLastSpawn = 0;
        spawnTime = 0;
        waveActive = false;
        waveCount = 0;
    }

    public bool isWave()
    {
        return waveActive;
    }

    public void Update()
    {
        if (waveActive)
        {
            spawnTime = 2f;
            if(simpleShoot.getbulletCount() != 0)
                timeSinceLastSpawn += Time.deltaTime;

            timeText.text = "Begin";
            if (waveCount >= 2)
            {
                timeText.text = "";
            }

            if (timeSinceLastSpawn > spawnTime)
            {
                int spawnIndex = Random.Range(0, SpawnPoints.Length);
                Target tar = Instantiate(TargetsPrefabs[0], SpawnPoints[spawnIndex].position, SpawnPoints[spawnIndex].rotation).GetComponent<Target>();
                timeSinceLastSpawn = 0;
                waveCount += 1;
            }

            //check for empty mag 
            if (simpleShoot.getbulletCount() == 0)
            {
                timeText.text = "Reload";
            }
            else
            {
                timeText.text = "";
            }

            if (waveCount >= 32 || simpleShoot.getShotCount() >= 32)
            {
                StartCoroutine(EndWave());
                StartCoroutine(ScoreReset());
                waveCount = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
			    Application.Quit();
            #endif
        }
    }

    public void StartWave()
    {
        StartCoroutine("ActualStartWave");
    }

    private IEnumerator ActualStartWave()
    {
        timeText.text = "3";
        yield return new WaitForSeconds(1);

        timeText.text = "2";
        yield return new WaitForSeconds(1);

        timeText.text = "1";
        yield return new WaitForSeconds(1);

        waveActive = true;

    }

    private IEnumerator EndWave()
    {
        waveActive = false;
        timeText.text = "STOP";
        yield return new WaitForSeconds(3f);
        timeText.text = "Press A to Restart";
    }
    private IEnumerator ScoreReset()
    {
        yield return new WaitForSeconds(3f);
    }
}
