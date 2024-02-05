using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallColision : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private BallController _ballController;
    private BallDetails _ballDetails;

    public List<BallDetails> _collisionSameBalls=new List<BallDetails>();
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _ballController = GetComponent<BallController>();
        _ballDetails=GetComponent<BallDetails>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag.Contains("Ball"))
        {
            // _collisionSameBalls.Clear();
            // //ball should stuck with the other balls
            // _rigidbody.velocity=Vector3.zero;
            // _rigidbody.angularVelocity=Vector3.zero;
            //
            // //check for the same ball
            // //loop over for all the neighbours and store in the list
            // GetMyNeighbour(other.collider.GetComponent<BallDetails>());
            //
            //
            // //if same ball then check the 
            // if (_collisionSameBalls.Count > 1)
            // {
            //     StartCoroutine(DestroyMyBall());
            // }
            // else
            // {
            //     //ball details add neighbour
            //     _ballController._BallMode = BallController.BallMode.Stick;
            //     _ballDetails.CheckAndInitializeNeighbour();
            // }
        }
        else if (other.collider.tag.Contains("Cube"))
        {
            other.gameObject.GetComponent<CubeDetails>().DecreaseNumber();
            MusicController.Instance.PlayAudio();
        }
    }

    void GetMyNeighbour(BallDetails _ballDetails)
    {
        if (!_collisionSameBalls.Contains(_ballDetails))
        {
            _collisionSameBalls.Add(_ballDetails);
            foreach (var neighbour in _ballDetails._neighbourBalls)
            {
                GetMyNeighbour(neighbour);
            }
        }
    }


    IEnumerator DestroyMyBall()
    {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<SphereCollider>().enabled = false;
            yield return new WaitForSeconds(0.3f);
            while (_collisionSameBalls.Count > 0)
            {
                yield return new WaitForSeconds(0.3f);
                Destroy(_collisionSameBalls[0].gameObject);
                _collisionSameBalls.RemoveAt(0);
            }
            _ballController._BallMode = BallController.BallMode.Stick;
            Destroy(gameObject);
        }
}
