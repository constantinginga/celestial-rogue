using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathUpdater : MonoBehaviour
{
    void Update()
    {
        var graphToScan = AstarPath.active.data.gridGraph;
        AstarPath.active.Scan(graphToScan);
    }
}
