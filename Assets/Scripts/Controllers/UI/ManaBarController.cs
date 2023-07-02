using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Controllers.UI
{
    public class ManaBarController : ResourceBarController
    {
        protected override Resource Resource => Player.PlayerModel.Mana;
    }
}