using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public GameObject Broken;
    public MeshRenderer model;
    private Transform point1;
    private Transform point2;
    public int points;
    public bool despawn = true;
    private float pathTime;
    private float currentPathTime;
    private bool moving;
    private void Start()
    {
        
    }

    private void Update()
    {
        Destroy(gameObject, 3f);
    }

    private void OnTriggerStay(Collider other)
    {
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<AudioSource>().Play();
        model.enabled = false;
        Broken.SetActive(true);
        Rigidbody[] pieces = Broken.GetComponentsInChildren<Rigidbody>();
        float force = 1000;
        foreach(Rigidbody rb in pieces)
        {
            rb.AddForce(new Vector3(Random.Range(-force, force), Random.Range(-force, force), Random.Range(-force, force)));
        }
        Destroy(gameObject, 3.5f);
        if (FindObjectOfType<TargetManager>().isWave())
        {
            ScoreKeeper.current.ChangeScore(1);
        }
    }

    internal void SetPath(Transform startPos, Transform endpos)
    {
        point1 = startPos;
        point2 = endpos;
    }

    public void SetPathTime(float t)
    {
        pathTime = t;
    }
}
