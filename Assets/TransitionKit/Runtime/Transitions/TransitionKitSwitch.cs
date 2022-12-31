using AtaGames.TransitionKit;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AtaGames.TransitionKit
{
    public partial class TransitionKit : MonoBehaviour
    {
        //TransitionKit.FadeScene
        public static void FadeScene(string sceneName, float actionTime, Color color)
        {
            var fadeScene = new FadeTransition()
            {
                nextSceneName = sceneName,
                transitionTime = actionTime
            };

            TransitionKit.Instance.StartCoroutine(TransitionKit.Instance.TransitionWithDelegate(fadeScene));
        }    
        
    }
}