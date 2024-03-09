using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private Rigidbody _rigidbody;
    [SerializeField] private float _speed = 1;
    private Vector3 _targetPoint;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        if (_rigidbody == null)
        {
            _rigidbody = gameObject.AddComponent<Rigidbody>();
        }
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        _rigidbody.useGravity = false;
        _targetPoint = transform.position;
    }

    void Update()
    {
        //_rigidbody.AddForce(Vector3.forward * _speed);
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log("Кликнули на объекте: " + hit.transform.name);
                Debug.Log("Координаты точки попадания: " + hit.point);
                if (hit.transform.name == "Wall(Clone)")
                {
                    Debug.Log(2);
                    return;
                }
                _targetPoint = hit.transform.position;
                _targetPoint.y = 1.5f;
                transform.position = Vector3.Lerp(transform.position, _targetPoint, _speed * Time.deltaTime);
            }
        }
        
        if (transform.position != _targetPoint)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPoint, _speed * Time.deltaTime);
        }
    }
}