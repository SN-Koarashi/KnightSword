using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pot : MonoBehaviour
{
    public ParticleSystem particle;
    private AudioSource audioSource;
    int count = 0;
    // Start is called before the first frame update
    void Start()
    {
        particle.Stop();
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
            particle.Play();
            audioSource.Play();
        }
        if(count == 2){
            count = 0;
            particle.Stop();
            audioSource.Stop();
        }
    }
}
