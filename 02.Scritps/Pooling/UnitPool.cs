using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPool : PoolAble
{
    public override void ReleaseObject()
    {
        transform.parent = null;
        base.ReleaseObject();
    }
}
