using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AtaGames.TransitionKit
{
    public class TransitionKit : MonoBehaviour
    {
        /// <summary>
        /// Because Sometime We use Callbacks
        /// Those action need some seconds to Start.
        /// Here we can Globally set a Delay Before Fade out The 
        /// Transition Effect.
        /// </summary>
        [Tooltip("Wait Exactly After Scene is Loaded.")]
        public static float DelayAfterLoad = 1.0f;

        /// <summary>
        /// 
        /// </summary>
        public static bool IsWorking = false;
        /// <summary>
        /// By Default We will Keep the Transition Kit GO 
        /// In Dont destroy on load (is very small).
        /// </summary>
        public static bool keepTransitionKitInstance = true;

        ///Transition Callbacks
        public static Action OnTransitionStart;
        public static Action BeforeSceneLoad;
        public static Action AfterSceneLoad;
        public static Action OnTransitionCompleted;


        public static LoadSceneMode LoadMode = LoadSceneMode.Single;


        public static TransitionKit _instance;

        /// <summary>
        /// Support 
        /// No Domain Reload
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void FirstSetup()
        {
            IsWorking = false;
            _instance = null;
        }

        /// <summary>
        /// Auto Initialize In Any Scene.
        /// Very Helpful.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutoInitialize()
        {
            //Auto Create this TransitionKit GO
            GameObject gameObject = new GameObject(nameof(TransitionKit));
            _instance = gameObject.AddComponent<TransitionKit>();
            DontDestroyOnLoad(gameObject);
        }

        //Only One Instance of this Should Exist
        public static TransitionKit Instance
        {
            get
            {
                if (!_instance)
                {
                    // check if there is a TransitionKit instance already available in the scene graph before creating one
                    _instance = FindObjectOfType(typeof(TransitionKit)) as TransitionKit;

                    if (!_instance)
                    {
                        GameObject GO = new GameObject(nameof(TransitionKit));
                        _instance = GO.AddComponent<TransitionKit>();
                        return _instance;
                    }
                }
                return _instance;
            }
        }

        [Header("Canvas Mode")]

        public Material material;

        public RawImage rawImage;

        public bool useCanvasMode = true;

        public bool useUnscaledDeltaTime = true;

        [Header("Camera Mode")]

        public Camera transitionKitCamera;

        //private TransitionKitDelegate _transitionKitDelegate;

        private TransitionScene _transitionScene;

        public float deltaTime
        {
            get { return useUnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime; }
        }

        private Canvas canvas;

        private void Awake()
        {
            material = new Material(Shader.Find(Constants.FadeShader));
        }

        public void SetupScreenSpace()
        {

        }


        private void Initialize()
        {
            //Need to validate if we already have a merial first
            material.shader = _transitionScene.shaderForTransition();

            if (useCanvasMode)
            {
                _instance.StartCoroutine(setupCanvasAndTexture());
            }
            else
            {

            }
        }

        public void makeTextureTransparent()
        {
            var tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, Color.clear);
            tex.Apply();
            material.mainTexture = tex;
        }

        public void transitionWithDelegate(TransitionScene transitionKitDelegate)
        {
            gameObject.SetActive(true);
            _transitionScene = transitionKitDelegate;
            Initialize();
        }

        private IEnumerator setupCanvasAndTexture()
        {
            OnTransitionStart?.Invoke();

            canvas = getOrAddComponent<Canvas>();
            this.canvas.enabled = true;
            yield return Yielders.EndOfFrame;

            material.mainTexture = _transitionScene.textureForDisplay() ?? getScreenshotTexture();

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 128;
            canvas.enabled = true;

            var Image = getOrAddComponent<RawImage>();
            Image.texture = material.mainTexture;
            Image.material = material;

            //OnScreenObscured?.Invoke();//It's not called to early before actually obscure the screen ?

            yield return StartCoroutine(_transitionScene.onScreenObscured(this));
            Cleanup();
        }


        private Texture2D getScreenshotTexture()
        {
            var screenSnapshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false, false);
            screenSnapshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0, false);
            screenSnapshot.Apply();

            return screenSnapshot;
        }

        private void Cleanup()
        {
            OnTransitionCompleted?.Invoke();

            OnTransitionStart = null;
            BeforeSceneLoad = null;
            AfterSceneLoad = null;
            OnTransitionCompleted = null;

            if (keepTransitionKitInstance)
            {
                //this.material.mainTexture = null;
                gameObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
                _instance = null;
            }
        }

        public IEnumerator tickProgressPropertyInMaterial(float duration, bool reverseDirection = false)
        {
            var start = reverseDirection ? 1f : 0f;
            var end = reverseDirection ? 0f : 1f;

            var elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += deltaTime;
                var step = Mathf.Lerp(start, end, Mathf.Pow(elapsed / duration, 2f));
                material.SetFloat(Constants._Progress, step);
                yield return null;
            }
        }


        //This will fail if for some reason there's multiple components.
        private T getOrAddComponent<T>() where T : Component
        {
            var component = gameObject.GetComponent<T>();
            if (component == null)
            {
                component = gameObject.AddComponent<T>();
            }
            return component;
        }

        #region Static Calls

        public static void FadeScene(string sceneNameLoad, float time, Color color)
        {
            var fadeScene = new FadeTransition()
            {
                nextSceneName = sceneNameLoad,
                transitionTime = time,
                fadeToColor = color
            };
            TransitionKit.Instance.transitionWithDelegate(fadeScene);
        }

        public static void FadeScene(int sceneIndexLoad, float time, Color color)
        {
            var fadeScene = new FadeTransition()
            {
                nextSceneIndex = sceneIndexLoad,
                transitionTime = time,
                fadeToColor = color
            };
            TransitionKit.Instance.transitionWithDelegate(fadeScene);
        }

        public static void CircleTransition(string game, float time, Color color, string TagFollow)
        {
            var circleTransition = new CircleCutoutTransition()
            {
                nextSceneName = game,
                transitionTime = time,
                BackgroundColor = color,
                followTag = TagFollow
            };
            TransitionKit.Instance.transitionWithDelegate(circleTransition);
        }

        #endregion
    }
}