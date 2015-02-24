using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BatchCubeRenderer))]
public class CubeRoutine : MonoBehaviour
{
    protected static int s_id_seed;

    public BatchCubeRenderer.InstanceData[] m_instances;
    public Vector3 m_sphere_center;
    public float m_sphere_radius;
    protected BatchCubeRenderer m_renderer;


    public int GenID() { return ++s_id_seed; }

    public virtual void Generate() { }

    public virtual void OnEnable()
    {
        m_renderer = GetComponent<BatchCubeRenderer>();
        Generate();
        if (m_instances == null)
        {
            m_instances = new BatchCubeRenderer.InstanceData[0];
        }
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
        m_renderer.EachMaterials((m) => {
            m.SetVector("g_sphere", new Vector4(m_sphere_center.x, m_sphere_center.y, m_sphere_center.z, m_sphere_radius));
        });
    }
}
