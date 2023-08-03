using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public Values values;
    public Components components;
    public Events events;

    // Start is called before the first frame update
    void Start()
    {
        components.menuStart.SetActive(true);
        components.menuInfo.SetActive(false);
        components.menuGame.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator AwaitGameEnd()
    {
        yield return new WaitForSeconds(values.gameTime);
        events.gameEnded.Invoke();
    }

    #region Buttons

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
        public GameObject menuStart;
        public GameObject menuInfo;
        public GameObject menuGame;
    }

    [System.Serializable]
    public class Events
    {
        public UnityEvent startGame;
        public UnityEvent gameEnded;
        public UnityEvent resetGame;
    }

    [System.Serializable]
    public class Values
    {
        public float gameTime = 30;
    }
}
