using AtaGames.TransitionKit;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Canvas Test Example
/// </summary>
public class DemoTest : MonoBehaviour
{
    public const string DemoSceneA = "DemoSceneA";
    public const string DemoSceneB = "DemoSceneB";

    public bool DebugMessage = false;

    public Button FadeButton;
    public Button OpenCircleButton;

    public TransitionType transitionType;

    public enum TransitionType { Clasic, Coroutine, Async }

    [SerializeField] public Transform FollowMe;

    private void Start()
    {
        DontDestroyOnLoad(this);
        FadeButton.onClick.AddListener(OnFadeExample);
        OpenCircleButton.onClick.AddListener(OpenCircleScene);
    }

    private void OpenCircleScene()
    {
        if (TransitionKit.IsWorking) return;

        string nextScene = GetNextScene();

        TransitionKit.OnTransitionStart += OnTransitionKitStart;
        TransitionKit.OnTransitionCompleted += OnTransitionKitEnd;

        TransitionKit.BeforeSceneLoad += BeforeSceneLoad;
        TransitionKit.AfterSceneLoad += AfterSceneLoad;

        //FollowTransform
        //TransitionKit.OpenCircle(nextScene, 5f, Color.black, FollowMe);

        //FollowTag
        //TransitionKit.OpenCircle(nextScene, 5f, Color.black, "Player");
    }

    /// <summary>
    /// Fade From Scene 
    /// </summary>
    private void OnFadeExample()
    {
        if (TransitionKit.IsWorking) return;

        string nextScene = GetNextScene();

        TransitionKit.OnTransitionStart += OnTransitionKitStart;
        TransitionKit.OnTransitionCompleted += OnTransitionKitEnd;

        TransitionKit.BeforeSceneLoad += BeforeSceneLoad;
        TransitionKit.AfterSceneLoad += AfterSceneLoad;


        switch (transitionType)
        {
            //Clasic Way
            case TransitionType.Clasic:
                TransitionKit.FadeScene(nextScene, 1f, Color.black);
                break;
            case TransitionType.Coroutine://Coroutine Version
                var fader = new FadeTransition();
                fader.transitionTime = 1f;
                fader.fadeToColor = Color.black;
                fader.nextSceneName = GetNextScene();
                TransitionKit.Instance.StartCoroutine(TransitionKit.Instance.TransitionWithDelegate(fader));
                break;
            case TransitionType.Async://Async Version...Advantage of No isues with Enable Disable GameObject                
                var faderAsync = new FadeTransition();
                faderAsync.transitionTime = 1f;
                faderAsync.fadeToColor = Color.black;
                faderAsync.nextSceneName = GetNextScene();
                TransitionKit.Instance.TransitionWithDelegateTask(faderAsync);
                break;
        }
    }

    private void OnTransitionKitStart()
    {
        if (DebugMessage)
            Debug.Log("Transition Kit Start");
    }

    private void OnTransitionKitEnd()
    {
        if (DebugMessage)
            Debug.Log("Transition Kit End");
    }

    private void BeforeSceneLoad()
    {
        if (DebugMessage)
            Debug.Log("Transition Before Load Scene");
    }

    private void AfterSceneLoad()
    {
        if (DebugMessage)
            Debug.Log("Transition After Load Scene");
    }

    private string GetNextScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene.Equals(DemoSceneB))
        {
            return DemoSceneA;
        }
        else
        {
            return DemoSceneB;
        }
    }
}
