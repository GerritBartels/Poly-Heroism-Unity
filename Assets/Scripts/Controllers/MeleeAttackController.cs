using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Controllers.Enemy;

namespace Controllers
{
    public class MeleeAttackController : PlayerAttackControllerBase
    {
        private void Awake()
        {
            lifeSpan = 0.1f;
            damage = 100f;
        }
    }
}