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
        waveStartTime = 0;
        waveActive = false;
        waveCount = 0;
    }

    public bool isWave()
    {
        return waveActive;
    }

    private void Update()
    {
        if (waveActive)
        {
            timeSinceLastSpawn += Time.deltaTime;
            
            if(timeSinceLastSpawn > spawnTime)
            {
                //SpawnWave();
                int spawnIndex = Random.Range(0, (int)SpawnPoints.Length / 2) * 2; //only use even indicies
                float pathTime = Random.Range(2f, 4f);
                Target tar = Instantiate(TargetsPrefabs[0], SpawnPoints[spawnIndex].position, SpawnPoints[spawnIndex].rotation).GetComponent<Target>();
                tar.SetPath(SpawnPoints[spawnIndex], SpawnPoints[spawnIndex+1]);
                tar.SetPathTime(pathTime);
                timeSinceLastSpawn = 0;
                spawnTime = 2f;
                waveCount += 1;
                if (waveCount % 8 == 0)
                {
                    spawnTime = 3;
                }
            }

            timeText.text = "Begin";
            int time = (int) Mathf.Floor(WaveTime - (Time.time - waveStartTime));
            if (waveCount == 16)
            {
                StartCoroutine(EndWave());
                waveCount = 0;
            }
        }
    }
    private void SpawnWave()
    {    
        int[] tarList = { 0, 0, 0, 0, 0, 0, 0, 0, };
        for (int i = 0; i < tarList.Length; i++)
        {
            int spawnIndex = Random.Range(0, (int)SpawnPoints.Length / 2) * 2; //only use even indicies
            float spawnTime = Random.Range(0.75f, 1.5f);//declare random interval that the targets spawn apart from each other
            float pathTime = Random.Range(2f, 4f);
            IEnumerator spawn = SpawnTarget(tarList[i], SpawnPoints[spawnIndex], SpawnPoints[spawnIndex+1],pathTime, i); //spawn target a fixed amount of time from now
            StartCoroutine(spawn);
        }
        waveCount += 1;
    }

    private IEnumerator SpawnTarget(int targetIndex, Transform startPos, Transform endpos, float pathTime, int i)
    {
        yield return new WaitForSeconds(2 * i);
        Target tar = Instantiate(TargetsPrefabs[targetIndex], startPos.position, startPos.rotation).GetComponent<Target>();
        tar.SetPath(startPos, endpos);
        tar.SetPathTime(pathTime);
    }


    public void StartWave() {//wraper so it can be called with button press
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

        waveStartTime = Time.time;
        waveActive = true;

    }

    private IEnumerator EndWave()
    {
        waveActive = false;
        timeText.text = "STOP";
        yield return new WaitForSeconds(3f);
    }
}
