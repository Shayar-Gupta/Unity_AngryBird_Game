using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

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
    [SerializeField] private Transform _elasticTransform;


    [Header("SlingShot Stats")]
    [SerializeField] private float _maxDistance = 3.5f;
    [SerializeField] private float _shotForce = 5f;
    [SerializeField] private float _timeBtweenBirdRespwans = 2f;
    [SerializeField] private float _elasticDivider = 1.2f;
    [SerializeField] private float _maxAnimationTime = 1f;
    [SerializeField] private AnimationCurve _elasticCurve;


    [Header("Scripts")]
    [SerializeField] private SlingShotArea _slingShotArea;
    [SerializeField] private CameraManager _cameraManager;


    [Header("Bird")]
    [SerializeField] private AngryBird _angryBirdPrefab;
    [SerializeField] private float _angryBirdPositionOffest = 0.275f;

    [Header("Sounds")]
    [SerializeField] private AudioClip _elasticPulledClip;
    [SerializeField] private AudioClip[] _elasticReleasedClip;


    private Vector2 _slingShotLinesPosition;
    private Vector2 _direction;
    private Vector2 _directionNormalized;

    private bool _clickedWithinArea;
    private bool _birdOnSlingshot;

    private AngryBird _spawnedAngryBird;

    private AudioSource _audioSource;

    private void Awake()
    {

        _audioSource = GetComponent<AudioSource>();
        _leftLineRenderer.enabled = false;
        _rightLineRenderer.enabled = false;
        SpawnAngryBird();
    }

    private void Update()
    {

        if (InputManager.WasLeftMouseButtonPressed && _slingShotArea.IsWithinSlingShotArea())
        {
            _clickedWithinArea = true;
            if (_birdOnSlingshot)
            {
                SoundManager.instance.PlayClip(_elasticPulledClip, _audioSource);
                _cameraManager.SwitchToFollowCam(_spawnedAngryBird.transform);
            }
        }

        if (InputManager.IsLeftMousePressed && _clickedWithinArea && _birdOnSlingshot)
        {
            //Debug.Log("mouse was clicked");
            DrawSlingShot();
            positionAndRotateAngryBird();
        }

        if (InputManager.WasLeftMouseButtonReleased && _birdOnSlingshot && _clickedWithinArea)
        {
            if (GameManager.instance.hasEnoughShots())
            {
                _clickedWithinArea = false;
                _birdOnSlingshot = false;

                _spawnedAngryBird.LaunchBird(_direction, _shotForce);

                SoundManager.instance.PlayRandomClip(_elasticReleasedClip, _audioSource);

                GameManager.instance.UseShot();

                // SetLines(_centerPosition.position);
                AnimatesSlingShot();

                if (GameManager.instance.hasEnoughShots()) StartCoroutine(SpawnAngryBirdAfterTime());
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
        _elasticTransform.DOComplete();
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

    private IEnumerator SpawnAngryBirdAfterTime()
    {
        yield return new WaitForSeconds(_timeBtweenBirdRespwans); //wait for 2f as _timeBtweenBirdRespwans -= 2f 

        SpawnAngryBird();

        _cameraManager.SwitchToIdleCam();

    }

    #endregion

    #region Animate SlingShot

    private void AnimatesSlingShot()
    {
        _elasticTransform.position = _leftLineRenderer.GetPosition(0);

        float dist = Vector2.Distance(_elasticTransform.position, _centerPosition.position);

        float time = dist / _elasticDivider;

        _elasticTransform.DOMove(_centerPosition.position, time).SetEase(_elasticCurve);
        StartCoroutine(AnimatesSlingShotLines(_elasticTransform, time));
    }

    private IEnumerator AnimatesSlingShotLines(Transform trans, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time && elapsedTime < _maxAnimationTime)
        {
            elapsedTime += Time.deltaTime;
            SetLines(trans.position);
            yield return null;
        }
    }

    #endregion
}


//1.02 hrs why setlines has vector2 as an argument datatype but passing vector3 datatype parameter.