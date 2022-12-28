using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtaGames.TransitionKit
{
    public class Constants
    {
        //Shader Names
        public const string FadeShader = "prime[31]/Transitions/Fader";
        public const string CircleCutoutShader = "Circle";

        //Common ID to Transition Shaders
        public static int _Progress = Shader.PropertyToID(nameof(_Progress));

        //Common Tags
        public const string Player = nameof(Player);//Common TAG
    }
}