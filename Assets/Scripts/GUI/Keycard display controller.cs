using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keycarddisplaycontroller : MonoBehaviour
{
    //public PickupControl control;
    public List<Image> keycard_images;



    public void UpdateKeycardDisplay (PickupControl control)//you need to call this function from somewhere else when a keycard is picked up
    {
        //for (int i = 0; i < keycard_images.Count; ++i)
        //{
        if (control.Gotkeycard1)
        {
            keycard_images[0].gameObject.SetActive(true);
            print("Heard first keycard");
        }
        if (control.Gotkeycard2)
        {
            keycard_images[1].gameObject.SetActive(true);
            print("Heard second keycard");
        }
        if (control.Gotkeycard3)
        {
            keycard_images[2].gameObject.SetActive(true);
            print("Heard third keycard");
        }
        //}


    }

    public void hide()
    {
        for (int i = 0; i < keycard_images.Count; i++)
        {
            keycard_images[i].enabled = false;
        }
    }

    public void show()
    {
        for(int i = 0; i < keycard_images.Count; i++)
        {
            keycard_images [i].enabled = true;
        }
    }
}
