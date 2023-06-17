using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Controllers.UI
{
    public class StaminaBarController : ResourceBarController
    {
        protected new void Awake()
        {
            base.Awake();
            Resource = GameObject.Find("Player").GetComponent<PlayerController>().PlayerModel.Stamina;
        }
    }
}