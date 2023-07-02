using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers
{
    public class RangedAttackController : PlayerAttackControllerBase
    {
        [SerializeField] private float bulletSpeed = 10f;

        private void Awake()
        {
            lifeSpan = 4f;
        }

        public new void Update()
        {
            base.Update();
            transform.Translate(Vector3.forward * (bulletSpeed * Time.deltaTime));
        }
    }
}