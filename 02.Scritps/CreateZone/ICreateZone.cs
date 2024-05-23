using System;
using UnityEngine;

public interface ICreateZone
{
    int ZoneIndex
    {
        get; set;
    }

    UnitData unitData
    { 
        get; set; 
    }
    GameObject unit
    {
        get; set;
    }

    Transform mytransform
    {
        get;
    }

    event Action<bool> HasUnit;
}