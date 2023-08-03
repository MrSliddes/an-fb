using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages (somewhat) the game state (mostly just UI now)
/// </summary>
public class GameManager : MonoBehaviour
{
    public Values values;
    public Components components;
    public Events events;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 90; // nice and smooth

        components.menuStart.SetActive(true);
        components.menuInfo.SetActive(false);
        components.menuGame.SetActive(false);
    }

    /// <summary>
    /// Await game end to trigger event
    /// </summary>
    /// <returns></returns>
    private IEnumerator AwaitGameEnd()
    {
        yield return new WaitForSeconds(values.gameTime);
        events.gameEnded.Invoke();
    }

    #region UI Buttons callbacks (toggels menus)

    public void ButtonStart()
    {
        components.menuStart.SetActive(false);
        components.menuInfo.SetActive(true);
    }

    public void ButtonExit()
    {
        Application.Quit();
    }

    public void ButtonPlay()
    {
        components.menuInfo.SetActive(false);
        components.menuGame.SetActive(true);
        events.startGame.Invoke();
        StartCoroutine(AwaitGameEnd());
    }

    public void ButtonMainMenu()
    {
        events.resetGame.Invoke();
        components.menuStart.SetActive(true);
        components.menuInfo.SetActive(false);
        components.menuGame.SetActive(false);
    }

    #endregion

    [System.Serializable]
    public class Components
    {
        [Tooltip("Reference to UI menu start")]
        public GameObject menuStart;
        [Tooltip("Reference to UI menu info")]
        public GameObject menuInfo;
        [Tooltip("Reference to UI menu game")]
        public GameObject menuGame;
    }

    [System.Serializable]
    public class Events
    {
        [Tooltip("When the game starts")]
        public UnityEvent startGame;
        [Tooltip("When the game ends")]
        public UnityEvent gameEnded;
        [Tooltip("When the game gets reset")]
        public UnityEvent resetGame;
    }

    [System.Serializable]
    public class Values
    {
        [Tooltip("How long the game takes")]
        public float gameTime = 30;
    }
}
