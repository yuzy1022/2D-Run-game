using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepEnd : MonoBehaviour
{
    public GameObject[] items = new GameObject[3];
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag=="Player"){
            PlatformSpawner.createPlatform = false;
            }
        
   }
}
