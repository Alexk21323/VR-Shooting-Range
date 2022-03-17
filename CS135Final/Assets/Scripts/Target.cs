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
        moving = true;
        Destroy(gameObject, Random.Range(5f, 8f));
        currentPathTime = 0;
    }

    private void Update()
    {
        if (moving)
        {
            currentPathTime += Time.deltaTime;
            //transform.position = Vector3.Lerp(point1.position, point2.position, currentPathTime / pathTime);
            if (currentPathTime / pathTime >= 1)
            {
                Destroy(gameObject, 1f);
            }
        }
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
        Destroy(gameObject, 3f);
        if (FindObjectOfType<TargetManager>().isWave())
        {
            ScoreKeeper.current.ChangeScore(1);
        }
        moving = false;
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
