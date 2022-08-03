using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buttonsound : MonoBehaviour
{

    public AudioSource audioSource;
/*    public AudioClip audioClip;*/

    public void playsound()
    {
/*        audioSource.clip = audioClip;*/
        audioSource.Play();

    }

}

