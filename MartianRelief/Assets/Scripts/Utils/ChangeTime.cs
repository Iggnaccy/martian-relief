using UnityEngine;
using System.Collections;

public class ChangeTime : MonoBehaviour
{
    public void Change()
    {
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
    }
}
