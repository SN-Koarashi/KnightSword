using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clock : MonoBehaviour
{
    private AudioSource audioSource;
    int count = 1;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        count++;
        if(count == 1){
            audioSource.Play();
        }
        if(count == 2){
            count = 0;
            audioSource.Stop();
        }
    }
}
