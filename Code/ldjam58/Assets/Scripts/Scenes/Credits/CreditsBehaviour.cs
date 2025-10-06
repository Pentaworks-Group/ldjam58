using System;

using Assets.Scripts.Scenes.Menues;

using UnityEngine;

namespace Assets.Scripts.Scenes.Credits
{
    public class CreditsBehaviour : BaseMenuBehaviour
    {
        public void PlayNoot()
        {
            GameFrame.Base.Audio.Effects.Play("Noot");
        }
    }
}
