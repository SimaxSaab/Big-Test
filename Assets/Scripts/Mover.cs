using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField] private float _speed = 1;
    private Vector3 _targetPoint;
    private GameObject navMeshSurface;
    private NavMeshAgent navMeshAgent;
    public GameObject capsuleObject;
    private LayerMask grassLayer;
    private LayerMask sandLayer;

    void Start()
    {
        grassLayer = 1 << LayerMask.NameToLayer("Grass");
        sandLayer = 1 << LayerMask.NameToLayer("Sand");
        _rigidbody = GetComponent<Rigidbody>();

        if (_rigidbody == null)
        {
            _rigidbody = gameObject.AddComponent<Rigidbody>();
        }
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        _rigidbody.useGravity = false;
        //_targetPoint = transform.position;
        navMeshSurface = GameObject.Find("NavMeshSurfaceManager");
        NavMeshSurface nMeshSurface = navMeshSurface.GetComponent<NavMeshSurface>();
        nMeshSurface.collectObjects = CollectObjects.Children;
        nMeshSurface.BuildNavMesh();
        capsuleObject = GameObject.Find("Capsule(Clone)");
        capsuleObject.AddComponent<BoxCollider>();
        if (capsuleObject != null)
        {
            // Добавьте компонент NavMeshAgent к объекту
            navMeshAgent = capsuleObject.AddComponent<NavMeshAgent>();
        }
        else
        {
            Debug.LogError("Object with name 'Capsule(Clone)' not found.");
        }
    }

    void Update()
    {
        RaycastHit hit;
        //_rigidbody.AddForce(Vector3.forward * _speed); просто двинаю вперед - эта функция без делта тайма идет
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            

            

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Кликнули на объекте: " + hit.transform.name);
                Debug.Log("Координаты точки попадания: " + hit.point);
                if (hit.transform.name == "Wall(Clone)")
                {
                    Debug.Log(2);
                    return;
                }
                /*_targetPoint = hit.transform.position; это попытка начала реализации без нав меша 1
                _targetPoint.y = 1.5f;
                transform.position = Vector3.Lerp(transform.position, _targetPoint, _speed * Time.deltaTime);*/

                navMeshAgent.SetDestination(hit.point);
            }
        }

        Vector3 capsulePosition = capsuleObject.transform.position;

        // Проверяем, находится ли центр капсулы над зеленым кубом (травой)
        if (Physics.Raycast(capsulePosition, Vector3.down, out _, Mathf.Infinity, grassLayer))
        {
            Debug.Log("Капсула над зеленым кубом (травой).");
            navMeshAgent.speed = 1.0f;
        }
        // Проверяем, находится ли центр капсулы над желтым кубом (песком)
        else if (Physics.Raycast(capsulePosition, Vector3.down, out _, Mathf.Infinity, sandLayer))
        {
            Debug.Log("Капсула над желтым кубом (песком).");
            navMeshAgent.speed = 0.5f;
        }
        /*
        if (transform.position != _targetPoint)  это попытка начала реализации без нав меша 2
        {
            transform.position = Vector3.Lerp(transform.position, _targetPoint, _speed * Time.deltaTime);
        }*/
    }
}