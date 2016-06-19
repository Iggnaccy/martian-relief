using UnityEngine;
using System.Collections;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class PauseManager : MonoBehaviour
{
    RectTransform myRect;
    bool isFullscreen;


    void Start()
    {
        myRect = GetComponent<RectTransform>();
        isFullscreen = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Pause();
        }
    }

    public void Pause()
    {
        isFullscreen = isFullscreen == false ? true : false;
        if (isFullscreen)
        {
			myRect.sizeDelta = new Vector2(Static.CANVAS_WIDTH, Static.CANVAS_HEIGHT);
        }
        else
            myRect.sizeDelta = new Vector2(Static.MINIMAP_WIDTH, Static.MINIMAP_HEIGHT);
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }
}