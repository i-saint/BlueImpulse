using UnityEngine;
using System.Collections;

public class Root : MonoBehaviour
{
    public bool m_wait_key_to_start = false;
    public AudioSource m_audio_source;
    public Animator m_animator;

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
        m_animator.enabled = true;
        m_audio_source.enabled = true;
        m_animator.Play("", 0, 0.0f);
        m_audio_source.Play();
    }

    void Awake()
    {
        if(m_wait_key_to_start)
        {
            m_animator.enabled = false;
            m_audio_source.enabled = false;
        }
    }

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape)) {
            Quit();
        }
        if (m_wait_key_to_start && Input.GetKeyUp(KeyCode.Space))
        {
            Play();
        }
    }
}
