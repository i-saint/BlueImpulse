using UnityEngine;
using System.Collections;

public class CoreCubes : CubeRoutine
{
    public override void Update()
    {
        base.Update();
    }

    void OnDrawGizmos()
    {
        if (m_instances == null) return;

        Gizmos.color = Color.white;
        for (int i = 0; i < m_instances.Length; ++i )
        {
            Gizmos.DrawWireCube(m_instances[i].translation, Vector3.one*m_instances[i].scale);
        }
    }
}
