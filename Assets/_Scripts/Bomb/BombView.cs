using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombView : MonoBehaviour
{
    [SerializeField] private ParticleSystem _explodeVFX;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _explodeStartAudio;
    [SerializeField] private AudioClip _explodeEndAudio;

    [SerializeField] private float _minExplodeSize;
    [SerializeField] private float _maxExplodeSize;

    private const string TimeToExplode = "_TimeToExplode";

    private float _currentSize;

    private Bomb _bomb;

    private List<Material> _materials;

    private Coroutine _scaleUpCoroutine;
    private Coroutine _scaleDownCoroutine;

    public void Initialize(Bomb bomb)
    {
        _bomb = bomb;

        Renderer renderer = GetComponent<Renderer>();
        _materials = new List<Material>(renderer.materials);

        _currentSize = _minExplodeSize = _materials[0].GetFloat(TimeToExplode);

        _bomb.StartedExplosion += OnStartedExplosion;
        _bomb.StopedExplosion += OnStopedExplosion;
        _bomb.Exploded += OnExploded;
    }

    private void OnDisable()
    {
        _bomb.StartedExplosion -= OnStartedExplosion;
        _bomb.StopedExplosion -= OnStopedExplosion;
        _bomb.Exploded -= OnExploded;
    }

    private void OnExploded()
    {
        _audioSource.PlayOneShot(_explodeEndAudio);

        gameObject.SetActive(false);
        Destroy(_bomb.gameObject, _explodeEndAudio.length);


        Instantiate(_explodeVFX, transform.position, transform.rotation, null);
    }

    private void OnStopedExplosion()
    {
        if (_scaleUpCoroutine != null)
        {
            StopCoroutine(_scaleUpCoroutine);
            _scaleUpCoroutine = null;
        }

        _scaleDownCoroutine ??= StartCoroutine(ScaleDown());
    }

    private void OnStartedExplosion()
    {
        _audioSource.PlayOneShot(_explodeStartAudio);

        if (_scaleDownCoroutine != null)
        {
            StopCoroutine(_scaleDownCoroutine);
            _scaleDownCoroutine = null;
        }

        _scaleUpCoroutine ??= StartCoroutine(ScaleUp());
    }

    private IEnumerator ScaleUp()
    {
        while (_currentSize < _maxExplodeSize)
        {
            _currentSize += Time.deltaTime;

            foreach (var material in _materials)
            {
                material.SetFloat(TimeToExplode, _currentSize);
            }

            yield return null;
        }
    }

    private IEnumerator ScaleDown()
    {
        while (_currentSize > _minExplodeSize)
        {
            _currentSize -= Time.deltaTime;

            foreach (var material in _materials)
            {
                material.SetFloat(TimeToExplode, _currentSize);
            }

            yield return null;
        }
    }
}
