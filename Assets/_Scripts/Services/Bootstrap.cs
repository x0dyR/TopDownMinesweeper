using Cinemachine;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Character _characterPrefab;
    [SerializeField] private CharacterView _characterViewPrefab;
    [SerializeField] private Transform _characterSpawnPoint;
    [SerializeField] private int _characterMaxHealth;

    [SerializeField] private PathGoalObject _pathGoalObject;

    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    [SerializeField] private MovePositionMediator _movePositionMediator;

    [SerializeField] private PathGoalPoint _pathGoalVisualizer;

    [SerializeField] private AudioMixerGroup _masterGroup;

    [SerializeField] private Button _toggleMisucButton;
    [SerializeField] private Button _toggleVFXButton;

    private PathGoalPoint _goalVisualizer;

    private InputSystem _input;
    private Raycaster _raycaster;

    private IMover _characterMover;

    private Health _characterHealth;

    private AudioHandler _audioHandler;

    [SerializeField]private bool _musicState;
    [SerializeField]private bool _vfxState;

    private void Awake()
    {
        CreateServices();

        _musicState = _audioHandler.IsMusicOn();
        _vfxState = _audioHandler.ISVFXOn();

        Character character = CreateCharacter();
        CreateCharacterView(character);

        InitializePathVisualizator(character);

        _virtualCamera.Follow = character.transform;

        _toggleMisucButton.onClick.AddListener(ToggleMusic);
        _toggleVFXButton.onClick.AddListener(ToggleVFX);
    }

    private void OnDisable()
    {
        _toggleMisucButton.onClick.RemoveListener(ToggleMusic);
        _toggleVFXButton.onClick.RemoveListener(ToggleVFX);
    }

    private void InitializePathVisualizator(Character character)
    {
        _pathGoalVisualizer.Initialize(_pathGoalObject);

        _movePositionMediator.Initialize(character, _pathGoalVisualizer);
    }

    private void CreateServices()
    {
        _input = new InputSystem();
        _raycaster = new Raycaster();

        _audioHandler = new(_masterGroup);
        _audioHandler.Initialize();
    }

    private void CreateCharacterView(Character character)
    {
        CharacterView view = Instantiate(_characterViewPrefab, character.transform.position, character.transform.rotation, character.transform);
        view.Initialize(character);
    }

    private Character CreateCharacter()
    {
        Character character = Instantiate(_characterPrefab, _characterSpawnPoint.position, Quaternion.identity, null);
        _characterMover = new NavMeshMover(character.NavAgent);
        _characterHealth = new Health(_characterMaxHealth, _characterMaxHealth);
        character.Initialize(_input, _raycaster, _characterMover, _characterHealth);
        return character;
    }

    private void ToggleMusic()
    {
        _musicState = !_musicState;

        if (_musicState)
            _audioHandler.OnMusic();
        else
            _audioHandler.OffMusic();
    }

    private void ToggleVFX()
    {
        _vfxState = !_vfxState;

        if (_vfxState)
            _audioHandler.OnVFX();
        else
            _audioHandler.OffVFX();
    }
}