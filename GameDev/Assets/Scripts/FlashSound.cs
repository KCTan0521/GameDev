using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashSound : MonoBehaviour
{
    private void LightningSound()
    {
        string[] names = {
            "Boss - Flash1",
            "Boss - Flash2",
            "Boss - Flash3"
        };

        FindObjectOfType<AudioManager>().RandomPlay(names);
    }

    private void RoarSound()
    {
        FindObjectOfType<AudioManager>().Play("Boss - Roar");
    }

}
