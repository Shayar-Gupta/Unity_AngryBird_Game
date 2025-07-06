using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    public int MaxNumberOfShots = 3;
    private int _usedNumberOfShots;
    public static GameManager instance;
    private IconHandler _iconHandler;

    private List<Baddie> _baddies = new List<Baddie>();

    [SerializeField] private float _secondsToWaitBeforeDeathCheck = 3f;
    [SerializeField] private GameObject _restartScreenObject;
    [SerializeField] private Image _nextLevelImg;
    [SerializeField] private SlingShotHandler _slingShotHandler;
    [SerializeField] private TextMeshProUGUI _levelText;


    public void Awake()
    {
        if (instance == null) instance = this;

        _iconHandler = FindObjectOfType<IconHandler>();
        Baddie[] baddies = FindObjectsOfType<Baddie>();
        for (int i = 0; i < baddies.Length; i++) _baddies.Add(baddies[i]);

        _nextLevelImg.enabled = false;
        _restartScreenObject.SetActive(false);

        int levelIndex = SceneManager.GetActiveScene().buildIndex + 1;
        _levelText.text = "" + levelIndex;

    }

    public void UseShot()
    {
        _usedNumberOfShots++;
        _iconHandler.UseShot(_usedNumberOfShots);
        CheckForLastShot();
    }

    public bool hasEnoughShots()
    {
        return _usedNumberOfShots < MaxNumberOfShots;
    }

    public void CheckForLastShot()
    {
        if (_usedNumberOfShots == MaxNumberOfShots)
        {
            StartCoroutine(CheckAfterWaitTime());
        }
    }

    private IEnumerator CheckAfterWaitTime()
    {
        yield return new WaitForSeconds(_secondsToWaitBeforeDeathCheck); //wait for 2f as _timeBtweenBirdRespwans -= 2f 

        if (_baddies.Count == 0) WinGame();
        else RestartGame();

    }

    public void RemoveBaddie(Baddie baddie)
    {
        _baddies.Remove(baddie);
        CheckForAllDeadBaddies();
    }

    private void CheckForAllDeadBaddies()
    {
        if (_baddies.Count == 0) WinGame();
    }

    #region win/lose

    private void WinGame()
    {
        _restartScreenObject.SetActive(true);
        _slingShotHandler.enabled = false;

        int currSceneIdx = SceneManager.GetActiveScene().buildIndex;
        int maxLevels = SceneManager.sceneCountInBuildSettings;

        if (currSceneIdx + 1 < maxLevels) _nextLevelImg.enabled = true;
    }

    public void RestartGame()
    {
        DOTween.Clear(true);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    #endregion
}
