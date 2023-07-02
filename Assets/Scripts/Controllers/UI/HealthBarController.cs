using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Controllers.UI
{
    public class HealthBarController : ResourceBarController
    {
        protected override Resource Resource => Player.PlayerModel.Health;
    }
}