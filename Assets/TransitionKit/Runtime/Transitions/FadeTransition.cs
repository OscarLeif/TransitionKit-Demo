using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AtaGames.TransitionKit
{
    public class FadeTransition : TransitionScene
    {
        public new static string id = nameof(FadeTransition);

        public Color fadeToColor = Color.black;

        public override Mesh meshForDisplay()
        {
            return null;
        }

        public override Shader shaderForTransition()
        {
            return Shader.Find(Constants.FadeShader);
        }

        public override Texture2D textureForDisplay()
        {
            var tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, Color.clear);
            tex.Apply();
            return tex;
        }

        public override IEnumerator onScreenObscured(TransitionKit transitionKit)
        {
            TransitionKit.IsWorking = true;
            transitionKit.material.color = fadeToColor;

            float actionTime = this.transitionTime / 2f;//FadeIn - FadeOut
            //Fade In this actually depends on the shader. _progress 0 -> 1
            yield return transitionKit.StartCoroutine(transitionKit.tickProgressPropertyInMaterial(actionTime, false));

            transitionKit.makeTextureTransparent();//mandatory or shader fails.

            //TransitionKit.Instance.OnScreenObscured?.Invoke();            

            yield return LoadScene();            

            //TransitionKit.Instance.AfterSceneLoad?.Invoke();

            yield return Yielders.GetRealTime(TransitionKit.DelayAfterLoad);

            //Fade In this actually depends on the shader. _progress 1 -> 0
            yield return transitionKit.StartCoroutine(transitionKit.tickProgressPropertyInMaterial(actionTime, true));
            TransitionKit.IsWorking = false;
        }        
    }
}