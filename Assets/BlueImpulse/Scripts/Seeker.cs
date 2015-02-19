using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Seeker : MonoBehaviour
{
#if UNITY_EDITOR
    public GameObject[] m_roots_of_animated_objects;
    public GameObject m_music_player;
    public float m_time;
    bool m_dirty;

    void Start()
    {
        if (m_music_player == null)
        {
            return;
        }
        m_dirty = false;
    }

    void Update()
    {
        if (m_music_player == null || !m_dirty)
        {
            return;
        }

        m_dirty = false;

        AudioSource[] sources = m_music_player.GetComponentsInChildren<AudioSource>();
        foreach (var r in sources)
        {
            r.time = m_time;
        }

        foreach (var r in m_roots_of_animated_objects)
        {
            if (r == null) continue;
            Animator[] animators = r.GetComponentsInChildren<Animator>();
            foreach (var a in animators)
            {
                var si = a.GetCurrentAnimatorStateInfo(0);
                a.Play(si.nameHash, 0, m_time / si.length);
            }
        }

        Debug.Log("seek " + m_time);
    }

    void OnValidate()
    {
        m_dirty = true;
    }
#endif
}
