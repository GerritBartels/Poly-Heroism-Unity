using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    public class ExplosionController : PlayerAttackControllerBase
    {
        private void Awake()
        {
            lifeSpan = 0.4f;
        }
    }
}