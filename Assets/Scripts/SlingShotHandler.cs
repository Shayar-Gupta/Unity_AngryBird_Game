using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlingShotHandler : MonoBehaviour
{
    [Header("Line Renderers")]
    [SerializeField] private LineRenderer _leftLineRenderer;
    [SerializeField] private LineRenderer _rightLineRenderer;


    [Header("Transform References")]
    [SerializeField] private Transform _leftStartPosition;
    [SerializeField] private Transform _rightStartPosition;
    [SerializeField] private Transform _centerPosition;
    [SerializeField] private Transform _idlePosition;


    [Header("SlingShot Stats")]
    [SerializeField] private float _maxDistance = 3.5f;
    [SerializeField] private float _shotForce = 5f;
    [SerializeField] private float _timeBtweenBirdRespwans = 2f;

    [Header("Scripts")]
    [SerializeField] private SlingShotArea _slingShotArea;

    [Header("Bird")]
    [SerializeField] private AngryBird _angryBirdPrefab;
    [SerializeField] private float _angryBirdPositionOffest = 0.275f;

    private Vector2 _slingShotLinesPosition;
    private Vector2 _direction;
    private Vector2 _directionNormalized;

    private bool _clickedWithinArea;
    private bool _birdOnSlingshot;

    private AngryBird _spawnedAngryBird;

    private void Awake()
    {
        _leftLineRenderer.enabled = false;
        _rightLineRenderer.enabled = false;
        SpawnAngryBird();
    }

    private void Update()
    {

        if (InputManager.WasLeftMouseButtonPressed && _slingShotArea.IsWithinSlingShotArea())
        {
            _clickedWithinArea = true;
        }

        if (InputManager.IsLeftMousePressed && _clickedWithinArea && _birdOnSlingshot)
        {
            //Debug.Log("mouse was clicked");
            DrawSlingShot();
            positionAndRotateAngryBird();
        }

        if (InputManager.WasLeftMouseButtonReleased && _birdOnSlingshot)
        {
            if(GameManager.instance.hasEnoughShots()){
                _clickedWithinArea = false;
                _birdOnSlingshot = false;
                
                _spawnedAngryBird.LaunchBird(_direction, _shotForce);
                
                GameManager.instance.UseShot();

                SetLines(_centerPosition.position);

                if(GameManager.instance.hasEnoughShots()) StartCoroutine(SpawnAngryBirdAfterTime());
            }
            
        }

    }

    #region SlingShot methods

    private void DrawSlingShot()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);
        _slingShotLinesPosition = _centerPosition.position + Vector3.ClampMagnitude(touchPosition - _centerPosition.position, _maxDistance);
        SetLines(_slingShotLinesPosition);

        _direction = (Vector2)_centerPosition.position - _slingShotLinesPosition;
        _directionNormalized = _direction.normalized;
    }

    private void SetLines(Vector2 position)
    {
        if (!_leftLineRenderer.enabled && !_rightLineRenderer.enabled)
        {
            _leftLineRenderer.enabled = true;
            _rightLineRenderer.enabled = true;
        }

        _leftLineRenderer.SetPosition(0, position);
        _leftLineRenderer.SetPosition(1, _leftStartPosition.position);

        _rightLineRenderer.SetPosition(0, position);
        _rightLineRenderer.SetPosition(1, _rightStartPosition.position);
    }

    #endregion

    #region Angry Bird Methods

    private void SpawnAngryBird()
    {
        SetLines(_idlePosition.position);
        Vector2 dir = (_centerPosition.position - _idlePosition.position).normalized;
        Vector2 spawnPosition = (Vector2)_idlePosition.position + dir * _angryBirdPositionOffest;
        _spawnedAngryBird = Instantiate(_angryBirdPrefab, spawnPosition, Quaternion.identity);
        _spawnedAngryBird.transform.right = dir;

        _birdOnSlingshot = true;
    }

    private void positionAndRotateAngryBird()
    {
        _spawnedAngryBird.transform.position = _slingShotLinesPosition + _directionNormalized * _angryBirdPositionOffest;
        _spawnedAngryBird.transform.right = _directionNormalized;
    }

    private IEnumerator SpawnAngryBirdAfterTime(){
        yield return new WaitForSeconds(_timeBtweenBirdRespwans); //wait for 2f as _timeBtweenBirdRespwans -= 2f 

        SpawnAngryBird();

    }

    #endregion
}


//1.02 hrs why setlines has vector2 as an argument datatype but passing vector3 datatype parameter.