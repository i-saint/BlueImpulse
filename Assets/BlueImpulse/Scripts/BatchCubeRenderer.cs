using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;


public class BatchCubeRenderer : CustumDataBatchRenderer<BatchCubeRenderer.InstanceData>
{
    [System.Serializable]
    public struct InstanceData
    {
        public const int size = 40;

        public Vector3 translation;
        public Quaternion rotation;
        public float scale;
        public float time;
        public float id;
    }


    public override void OnEnable()
    {
        SetInstanceDataSize(InstanceData.size);
        base.OnEnable();
    }
}
