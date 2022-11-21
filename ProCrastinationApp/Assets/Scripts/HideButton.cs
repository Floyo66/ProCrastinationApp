using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideButton : MonoBehaviour
{
    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        hideButton(obj);
    }

    public void hideButton(GameObject obj){
        obj.SetActive(false);
    }

    


}
