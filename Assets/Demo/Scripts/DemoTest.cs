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
    public Button FadeButtonCoroutine;

    public TransitionType transitionType;

    public enum TransitionType { Clasic, Coroutine, Async }

    [SerializeField] public Transform FollowMe;

    private void Start()
    {
        FadeButton.onClick.AddListener(OnFadeExample);
        FadeButtonCoroutine.onClick.AddListener(FadeCoroutine);
    }

    private void FadeCoroutine()
    {
        string nextScene = GetNextScene();
        TransitionKit.Get.NextSceneName = nextScene;
        TransitionKit.Get.StartCoroutine(TransitionKit.Get.fadeTransition.LoadSceneRoutine());
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
