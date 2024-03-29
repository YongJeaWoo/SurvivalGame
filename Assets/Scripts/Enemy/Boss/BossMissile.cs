﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMissile : EnemyMissileTrigger
{
    public Transform target;
    NavMeshAgent nav;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        nav.SetDestination(target.position);
    }
}
