using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneIndex : MonoBehaviour
{
    CreateZone[] _createZone;

    private void Awake()
    {
        _createZone = GetComponentsInChildren<CreateZone>();
    }

    private void Start()
    {
        for(int i = 0;  i < _createZone.Length; i++)
        {
            _createZone[i].ZoneIndex = i;
        }
    }
}
