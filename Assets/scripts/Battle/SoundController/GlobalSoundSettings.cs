using System;
using System.Collections.Generic;
using UnityEngine;

class GlobalSoundSettings: MonoBehaviour
{
    public void ChangeAudio(System.Single value)
    {
        AudioListener.volume = value;
    }    

}
