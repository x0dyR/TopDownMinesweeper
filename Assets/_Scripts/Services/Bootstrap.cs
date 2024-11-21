using Cinemachine;
using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Character _characterPrefab;
    [SerializeField] private CharacterView _characterViewPrefab;
    [SerializeField] private Transform _characterSpawnPoint;
    [SerializeField] private int _characterMaxHealth;

    [SerializeField] private PathGoalObject _pathGoalObject;

    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    [SerializeField] private MovePositionMediator _movePositionMediator;

    private PathGoalPoint _goalVisualizer;

    private InputSystem _input;
    private Raycaster _raycaster;

    [SerializeField] private PathGoalPoint _pathGoalVisualizer;

    private IMover _characterMover;

    private Health _characterHealth;

    private void Awake()
    {
        Character character = Instantiate(_characterPrefab, _characterSpawnPoint.position, Quaternion.identity, null);

        CharacterView view = Instantiate(_characterViewPrefab, character.transform.position, character.transform.rotation, character.transform);

        _input = new InputSystem();
        _raycaster = new Raycaster();
        _characterMover = new NavMeshMover(character.NavAgent);
        _characterHealth = new Health(_characterMaxHealth, _characterMaxHealth);

        character.Initialize(_input, _raycaster, _characterMover, _characterHealth);

        view.Initialize(character);

        _pathGoalVisualizer.Initialize(_pathGoalObject);

        _movePositionMediator.Initialize(character, _pathGoalVisualizer);

        _virtualCamera.Follow = character.transform;
    }
}