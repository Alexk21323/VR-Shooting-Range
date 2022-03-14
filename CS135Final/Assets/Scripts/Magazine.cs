using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magazine : MonoBehaviour
{
    public GameObject bullet1;
    public GameObject bullet2;

    public int numberOfBullet = 8;

    void Update()
    {
        if(numberOfBullet == 1)
        {
            Destroy(bullet2);
        }    
        
        if(numberOfBullet == 0)
        {
            Destroy(bullet1);
        }
    }
}
