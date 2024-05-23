using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    private int layerIndex_Weapons; 
    Animator _animator;

    float _timer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        layerIndex_Weapons = _animator.GetLayerIndex("AttackLayer");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            _animator.SetLayerWeight(layerIndex_Weapons, 1);
            _timer = 0;
        }
        else
        {
            _timer += Time.deltaTime;
            if(_timer > 5)
            {
                _animator.SetLayerWeight(layerIndex_Weapons, 0);
            }
        }
    }
}
