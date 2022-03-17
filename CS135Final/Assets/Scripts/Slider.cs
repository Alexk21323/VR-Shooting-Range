using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Slider : MonoBehaviour
{
    public float threshold = 0.02f;
    public Transform target;
    public UnityEvent OnReached;
    public UnityEvent activateWave;
    public TargetManager targetManager;
    public ScoreKeeper score;
    public SimpleShoot simpleshoot;
    public bool waveActive;

    private bool wasReached = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance < threshold && !wasReached)
        {
            //Reached the target
            OnReached.Invoke();
            wasReached = true;
            waveActive = targetManager.isWave();
            if (!waveActive && simpleshoot.magazine)
            {
                ScoreKeeper.current.ResetScore();
                activateWave.Invoke();
            }
        }
        else if (distance >= threshold)
        {
            wasReached = false;
        }
    }
}
