using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BatchCubeRenderer))]
public class CubeRoutine : MonoBehaviour
{
    protected static int s_id_seed;

    public BatchCubeRenderer.InstanceData[] m_instances;
    protected BatchCubeRenderer m_renderer;


    public int GenID() { return ++s_id_seed; }

    public virtual void OnEnable()
    {
        m_renderer = GetComponent<BatchCubeRenderer>();
        for (int i = 0; i < m_instances.Length; ++i )
        {
            m_instances[i].id = GenID();
        }
    }

    public virtual void Update()
    {
        float dt = Time.deltaTime;
        for (int i = 0; i < m_instances.Length; ++i)
        {
            m_instances[i].time += dt;
        }
        m_renderer.AddInstances(m_instances);
    }
}
