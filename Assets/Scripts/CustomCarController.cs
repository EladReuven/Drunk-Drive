using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCarController : MonoBehaviour
{

    public const string HORIZONTAL = "Horizontal";
    public const string VERTICAL = "Vertical";


    [SerializeField] private DriveType currentDriveType = DriveType.BackWheelDrive;


    private float _horizontalInput;
    private float _verticalInput;
    private bool _isBreaking;
    private float _currentBreakForce;
    private float _currentSteerAngle;
    //private float _currentTimeBetweenSwerve;
    private bool _swerveReady;


    [SerializeField] private float _motorForce;
    [SerializeField] private float _breakForce;
    [SerializeField] private float _maxSteerAngle;
    [SerializeField] private float _minTimeBetweenSwerve = 2f;
    [SerializeField] private float _maxTimeBetweenSwerve = 4f;
    //[SerializeField] private float _timeBetweenSwerve = 3f;
    [SerializeField] private float _drunkSteerEffect = 0;

    [SerializeField] private WheelCollider frontLWheelCollider;
    [SerializeField] private WheelCollider frontRWheelCollider;
    [SerializeField] private WheelCollider backLWheelCollider;
    [SerializeField] private WheelCollider backRWheelCollider;

    [SerializeField] private Transform frontLWheelTransform;
    [SerializeField] private Transform frontRWheelTransform;
    [SerializeField] private Transform backLWheelTransform;
    [SerializeField] private Transform backRWheelTransform;

    private void Start()
    {
        _swerveReady = true;
    }

    private void FixedUpdate()
    {
        GetInput();
        HandleMotor(currentDriveType);
        DrunkSteering();
        HandleSteering();
        UpdateWheels();
    }

    private void GetInput()
    {
        _horizontalInput = Input.GetAxis(HORIZONTAL);
        _verticalInput = Input.GetAxis(VERTICAL);
        _isBreaking = Input.GetKey(KeyCode.Space);
    }

    private void HandleMotor(DriveType drive)
    {
        _currentBreakForce = _isBreaking ? _breakForce : 0;

        switch (drive)
        {
            case DriveType.BackWheelDrive:
                backLWheelCollider.motorTorque = _verticalInput * _motorForce;
                backRWheelCollider.motorTorque = _verticalInput * _motorForce;
                break;
            case DriveType.FourWheelDrive:
                backLWheelCollider.motorTorque = _verticalInput * _motorForce;
                backRWheelCollider.motorTorque = _verticalInput * _motorForce;
                frontLWheelCollider.motorTorque = _verticalInput * _motorForce;
                frontRWheelCollider.motorTorque = _verticalInput * _motorForce;
                break;
            case DriveType.FrontWheelDrive:
                frontLWheelCollider.motorTorque = _verticalInput * _motorForce;
                frontRWheelCollider.motorTorque = _verticalInput * _motorForce;
                break;
        }
        ApplyBreakForce();
    }

    private void ApplyBreakForce()
    {
        frontLWheelCollider.brakeTorque = _currentBreakForce;
        frontRWheelCollider.brakeTorque = _currentBreakForce;
        backLWheelCollider.brakeTorque = _currentBreakForce;
        backRWheelCollider.brakeTorque = _currentBreakForce;
    }

    //apply drunk effect here probably
    private void HandleSteering()
    {
        

        _currentSteerAngle = _maxSteerAngle * (_horizontalInput + _drunkSteerEffect);
        frontLWheelCollider.steerAngle = _currentSteerAngle;
        frontRWheelCollider.steerAngle = _currentSteerAngle;
    }

    private void DrunkSteering()
    {
        //every few seconds number changes between -1 and 1
        //the higher the absolute value the less time it remains
        //on (bigger/all?) numbers once player corrects the driving it it resets to 0

        if (!(_horizontalInput != 0))
            return;

        if (_swerveReady)
        {
            //_currentTimeBetweenSwerve = _timeBetweenSwerve;
            _drunkSteerEffect = 0;
            _swerveReady = false;
            StartCoroutine(SwerveCalculator());
        }

    }

    private IEnumerator SwerveCalculator()
    {
        if(!_swerveReady)
        {
            Debug.Log("before seconds angle: " + _drunkSteerEffect);
            yield return new WaitForSeconds(UnityEngine.Random.Range(_minTimeBetweenSwerve, _maxTimeBetweenSwerve));
            _swerveReady = true;
            _drunkSteerEffect = UnityEngine.Random.Range(-1f, 1f);
            Debug.Log("after seconds angle: " + _drunkSteerEffect);
        }
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLWheelCollider, frontLWheelTransform);
        UpdateSingleWheel(frontRWheelCollider, frontRWheelTransform);
        UpdateSingleWheel(backLWheelCollider, backLWheelTransform);
        UpdateSingleWheel(backRWheelCollider, backRWheelTransform);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private enum DriveType
    {
        BackWheelDrive,
        FourWheelDrive,
        FrontWheelDrive
    }
}
