using System.Collections;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Root : MonoBehaviour
{
    public bool m_wait_space_key_to_start = false;
    public bool m_show_time = true;
    public AudioSource m_audio_source;
    public Animator m_animator;
    public DSRenderer[] m_dsr;

#if UNITY_EDITOR
    [MenuItem("Edit/Reset Playerprefs")]
    public static void DeletePlayerPrefs() { PlayerPrefs.DeleteAll(); }
#endif

    public void Quit()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Play()
    {
        m_wait_space_key_to_start = false;
        m_animator.enabled = true;
        m_audio_source.enabled = true;
        m_animator.Play("", 0, 0.0f);
        m_audio_source.Play();
    }

    void Awake()
    {
        if(m_wait_space_key_to_start)
        {
            m_animator.enabled = false;
            m_audio_source.enabled = false;
        }

#if UNITY_STANDALONE
        m_dsr = GetComponentsInChildren<DSRenderer>();
        foreach(var dsr in m_dsr)
        {
            switch (QualitySettings.GetQualityLevel())
            {
                case 0:
                    dsr.resolution_scale = 0.5f;
                    break;
                case 1:
                    dsr.resolution_scale = 0.75f;
                    break;
                case 2:
                    dsr.resolution_scale = 1.0f;
                    break;
            }
        }
#endif // UNITY_STANDALONE
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape)) {
            Quit();
        }
        if (m_wait_space_key_to_start && Input.GetKeyUp(KeyCode.Space))
        {
            Play();
        }
        else if (Input.GetKey(KeyCode.Tab))
        {
            m_wait_space_key_to_start = true;
            m_animator.enabled = false;
            m_audio_source.enabled = false;
        }
    }

    void OnGUI()
    {
        if (m_wait_space_key_to_start)
        {
            GUI.Label(new Rect(5, 5, 300, 20), "press space key to start");
        }
#if UNITY_EDITOR
        if (m_show_time)
        {
            GUI.Label(new Rect(5, 25, 200, 20), "time: " + Time.time);
        }
#endif // UNITY_EDITOR
    }
}
