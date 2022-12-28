using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AtaGames.TransitionKit
{
    public abstract class TransitionScene
    {
        public string id = string.Empty;

        public int nextSceneIndex = -1;

        public string nextSceneName = string.Empty;

        public float transitionTime = 0.5f;

        public Material MaterialRef;

        public abstract Shader shaderForTransition();

        public abstract Mesh meshForDisplay();

        public abstract Texture2D textureForDisplay();

        public abstract IEnumerator onScreenObscured(TransitionKit transitionKit);

        /// <summary>
        /// The Issue Here is When I dont want to load a new scene
        /// I just want to make fake transition. Just Hide Screen to Do Something
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator LoadScene()
        {

            TransitionKit.BeforeSceneLoad?.Invoke();

            //OnScreenObscured.?Invoke() Could work here
            AsyncOperation asyncLoad = null;
            // Wait until the asynchronous scene fully loads
            if (string.IsNullOrEmpty(nextSceneName) == false)
            {
                asyncLoad = SceneManager.LoadSceneAsync(nextSceneName, TransitionKit.LoadMode);
            }
            else if (nextSceneIndex >= 0)
            {
                asyncLoad = SceneManager.LoadSceneAsync(nextSceneIndex, TransitionKit.LoadMode);
            }

            if (asyncLoad != null)
            {
                while (asyncLoad.isDone == false)
                {
                    yield return null;
                }
            }

            TransitionKit.AfterSceneLoad?.Invoke();

            yield return Yielders.GetRealTime(TransitionKit.DelayAfterLoad);


        }

    }
}
