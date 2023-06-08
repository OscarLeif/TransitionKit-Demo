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

    public Button FadeButton;
    public Button OpenCircleButton;

    public TransitionType transitionType;

    public enum TransitionType { Clasic, Coroutine, Async }

    [SerializeField] public Transform FollowMe;    

    private void Start()
    {        
        FadeButton.onClick.AddListener(OnFadeExample);
        OpenCircleButton.onClick.AddListener(OpenCircleScene);
    }

    private void OpenCircleScene()
    {
        //if (TransitionKit.IsWorking) return;

        string nextScene = GetNextScene();

        //TransitionKit.OnTransitionStart += OnTransitionKitStart;
        //TransitionKit.OnTransitionCompleted += OnTransitionKitEnd;

        //TransitionKit.BeforeSceneLoad += BeforeSceneLoad;
        //TransitionKit.AfterSceneLoad += AfterSceneLoad;

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
        string nextScene = GetNextScene();

        TransitionKit.Get.FadeScene(nextScene, 1f, Color.black);             
    }

    private void OnTransitionKitStart()
    {
        Debug.Log("Transition Kit Start");
    }

    private void OnTransitionKitEnd()
    {
        Debug.Log("Transition Kit End");
    }

    private void BeforeSceneLoad()
    {
        Debug.Log("Transition Before Load Scene");
    }

    private void AfterSceneLoad()
    {
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
