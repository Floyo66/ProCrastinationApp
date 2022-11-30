using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectRewardButton : MonoBehaviour
{

    [SerializeField]
    ProgressBar bar;

    public void TestClick()
    {
        bar.UpdateProgress(0.1f);
    }
}
