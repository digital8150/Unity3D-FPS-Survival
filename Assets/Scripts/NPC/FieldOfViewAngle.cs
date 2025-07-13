using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewAngle : MonoBehaviour
{
    [Header("기본 정보 입력")]
    [SerializeField]
    private float viewAngle;
    [SerializeField]
    private float viewDistance;
    [SerializeField]
    private LayerMask targetMask;

    private Pig thePig;

    private void Start()
    {
        thePig = GetComponent<Pig>();
    }

    private void Update()
    {
        View();
    }

    Vector3 BoundaryAngle(float _angle)
    {
        _angle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad), 0f, Mathf.Cos(_angle * Mathf.Deg2Rad));
    }

    void View()
    {
        Vector3 _leftBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 _rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        Debug.DrawRay(transform.position + transform.up, _leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, _rightBoundary, Color.red);

        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        foreach (Collider collider in _target)
        {
            Transform _targetTf = collider.transform;
            if(_targetTf.name == "Player")
            {
                Vector3 _direction = (_targetTf.position - transform.position).normalized;
                float _angle = Vector3.Angle(_direction, transform.forward);

                if(_angle < viewAngle * 0.5f)
                {
                    RaycastHit _hitInfo;
                    if (Physics.Raycast(transform.position + transform.up, _direction, out _hitInfo, viewDistance))
                    {
                        if (_hitInfo.transform.name == "Player")
                        {
                            Debug.Log("플레이어가 돼지 시야 내에 있습니다.");
                            Debug.DrawRay(transform.position + transform.up, _direction, Color.blue);
                            thePig.Run(_hitInfo.transform.position);
                        }
                    }
                }
            }
        }
    }
}
