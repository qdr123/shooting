using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void OnClickStartBtn(RectTransform rt)
    {
        Debug.Log("Click Button"+rt.localScale.x.ToString());
    }
   
}
