using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Step : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag=="Player"){
            PlatformSpawner.createPlatform = true;
            PlatformSpawner.stepCount += 1;
            }
   }

}
