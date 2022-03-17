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
    private void Awake()
    {
        timeSinceLastSpawn = 0;
        spawnTime = 0;
        waveStartTime = 0;
        waveActive = false;
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
                SpawnWave();
                timeSinceLastSpawn = 0;
                spawnTime = 5 - Random.Range(1,3) * ((Time.time - waveStartTime) / WaveTime);
            }

            timeText.fontSize += 2f * Time.deltaTime;
            int time = (int) Mathf.Floor(WaveTime - (Time.time - waveStartTime));
            timeText.text = time.ToString();
            if(time <= 0)
            {
                StartCoroutine(EndWave());
            }
        }
    }
    private void SpawnWave()
    {
        //declare lists of targets that can be chosen from
        int[] l1 = { 0, 0, 0, 0, 0 };
        int[] l2 = { 0, 0, 0,};
        int[] l3 = { 0, 0, 0, 0, 0, 0 };
        int[] l4 = { 0, 0, 0, 0, 0 };
        int[] l5 = { 0, 0, 0, 0, 0 };
        int[] l6 = { 0, 0, 0, 0, 0 };
        int[] l7 = { 0, 0, 0, 0 };
        int[] l8 = { 0, 0, 0, 0, 0, 0, 0 };
        int[] l9 = { 0, 0, 0};
        int[] l10 = { 0, 0, 0, 0, 0 };
        int[] l11 = { 0 };
        int[] l12 = { 0, 0, 0, 0, 0 };
        List<int[]> targetSets = new List<int[]>();//declare array of those lists
        targetSets.Add(l1); targetSets.Add(l2); targetSets.Add(l3); targetSets.Add(l4); targetSets.Add(l5); targetSets.Add(l6); targetSets.Add(l7);
        targetSets.Add(l8); targetSets.Add(l9); targetSets.Add(l10); targetSets.Add(l1); targetSets.Add(l1); targetSets.Add(l1); targetSets.Add(l11);
        targetSets.Add(l12);

        int spawnIndex = Random.Range(0, (int)SpawnPoints.Length / 2) * 2; //only use even indicies
        float spawnTime = Random.Range(0.5f, 1.5f);//declare random interval that the targets spawn apart from each other
        float pathTime = Random.Range(2f, 4f);
        int[] tarList = targetSets[Random.Range(0, targetSets.Count)];
        for (int i = 0; i < tarList.Length; i++)
        {
            IEnumerator spawn = SpawnTarget(tarList[i],SpawnPoints[spawnIndex], SpawnPoints[spawnIndex+1],pathTime, i);//spawn target a fixed amount of time from now
            StartCoroutine(spawn);
        }
        
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
        timeText.color = new Color(255, 0, 0);
        timeText.fontSize = 90;
        yield return new WaitForSeconds(3f);
        if (ScoreKeeper.current.CheckHighScore())
        {
            timeText.text = "NEW HIGH SCORE";
            timeText.fontSize = 70;
            timeText.color = new Color(80, 255, 0);
            SceneManager.LoadScene(2);
            yield return new WaitForSeconds(3f);
        }
        else
        {
            timeText.text = "GOOD WORK";
            timeText.fontSize = 90;
            yield return new WaitForSeconds(3f);
            timeText.text = "GOODBYE";
            timeText.fontSize = 90;
            timeText.color = new Color(80, 255, 0);
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(0);
        }
    }
}
