using UnityEngine;
using System.Collections;

public class Root : MonoBehaviour
{

    public void Quit()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
