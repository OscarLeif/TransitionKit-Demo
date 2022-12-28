using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AtaGames.TransitionKit
{
    /// <summary>
    /// Warning If The Player or Tracking Target
    /// is to Far you will only see a Solid Image
    /// 
    /// </summary>
    public class CircleCutoutTransition : TransitionScene
    {
        public Color BackgroundColor = Color.black;

        [Tooltip("Follow Target")]
        public GameObject targetGameobject;

        [Tooltip("Follow By Tag")]
        public string followTag;

        public static int _Offset = Shader.PropertyToID(nameof(_Offset));

        public override Mesh meshForDisplay()
        {
            return null;
        }

        public override Shader shaderForTransition()
        {
            return Shader.Find(Constants.CircleCutoutShader);
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
            transitionKit.material.color = BackgroundColor;
            this.MaterialRef = transitionKit.material;

            transitionKit.StartCoroutine(FollowTarget());

            float actionTime = this.transitionTime / 2f;//FadeIn - FadeOut
            //Fade In this actually depends on the shader. _progress 0 -> 1
            yield return transitionKit.StartCoroutine(transitionKit.tickProgressPropertyInMaterial(actionTime, false));

            //transitionKit.makeTextureTransparent();//mandatory or shader fails.

            //TransitionKit.BeforeSceneLoad?.Invoke();

            //AsyncOperation asyncLoad = null;

            //// Wait until the asynchronous scene fully loads
            //if (string.IsNullOrEmpty(nextSceneName) == false)
            //    asyncLoad = SceneManager.LoadSceneAsync(nextSceneName, TransitionKit.LoadMode);
            //else if (nextSceneIndex >= 0)
            //    asyncLoad = SceneManager.LoadSceneAsync(nextSceneIndex, TransitionKit.LoadMode);
            ////Throw error but it will now fail
            //if (asyncLoad != null)
            //{
            //    while (!asyncLoad.isDone)
            //    {
            //        yield return null;
            //    }
            //    yield return Yielders.GetRealTime(0.5f);
            //}

            yield return LoadScene();

            //TransitionKit.Instance.AfterSceneLoad?.Invoke();

            //Fade In this actually depends on the shader. _progress 1 -> 0
            yield return transitionKit.StartCoroutine(transitionKit.tickProgressPropertyInMaterial(actionTime, true));
            TransitionKit.IsWorking = false;
        }

        /// <summary>
        /// Use for Focus 
        /// </summary>
        /// <returns></returns>
        private IEnumerator FollowTarget()
        {
            //Define if we follow a Transform Or By TAG

            bool shouldFollowTag = string.IsNullOrEmpty(followTag) == false;

            while (true)
            {
                if (!string.IsNullOrEmpty(followTag))
                {
                    this.targetGameobject = GameObject.FindGameObjectWithTag(followTag);
                }
                if (this.targetGameobject == null)
                {
                    MaterialRef.SetVector(_Offset, new Vector2(0.5f, 0.5f));
                }
                else if (this.targetGameobject)
                {
                    //FOCUS_TAG = this.targetGameobject.tag;                    
                    Vector3 pos = Camera.main.WorldToViewportPoint(targetGameobject.transform.position);
                    MaterialRef.SetVector(_Offset, pos);
                }
                yield return null;
            }
        }

        /// <summary>
        /// Follow By Tag. It's possible that the GO could be disable or Destroyed.
        /// </summary>
        private void FollowTag()
        {

        }

        private void FollowGameObject()
        {

        }
    }
}