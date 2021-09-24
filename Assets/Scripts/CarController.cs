using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private float motorForce;
    [SerializeField] private float breakForce;
    [SerializeField] private float maxSteerAngle;
    [SerializeField] private bool isFrontTorgue, isRearTorgue;

    [SerializeField] private Transform frontLeftWheel;
    [SerializeField] private Transform frontRightWheel;
    [SerializeField] private Transform rearLeftWheel;
    [SerializeField] private Transform rearRightWheel;

    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider, rearLeftWheelCollider, rearRightWheelCollider;

    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private Canvas canvas;
    [SerializeField] private GameObject questPanel;
    [SerializeField] GameObject error;

    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentbreakForce;
    private bool isBreaking, isRestarted, stopped = false;

    private float kph, wheelsRPM;


    Vector3 startPosition;
    Quaternion startRotation;
    Rigidbody rb;
    QA qa;
    CheckPoint lastCheckpoint;
    string lastSuccesCheck="";
    AudioSource audioSrc;
    float minPitch = 0.5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.rotation;

        qa = canvas.GetComponent<QA>();
        audioSrc = GetComponent<AudioSource>();
        audioSrc.pitch = minPitch;
        StartCoroutine(showStartQuest());
    }

    private IEnumerator showStartQuest()
    {
        setStopeed(true);
        showQuest("Занятие первое: \n Дорожные знаки");
        yield return new WaitForSeconds(3f);
        showQuest("Вы можете управлять машиной с помощью стрелок или 'WASD'");
        yield return new WaitForSeconds(3f);
        showQuest("'Space' - ручной тормоз\n'R' - попробовать снова");
        yield return new WaitForSeconds(3f);
        showQuest("Поезжайте прямо. Cчастливого пути!");
        setStopeed(false);
    }

    private void FixedUpdate()
    {
        if (!stopped)
        {
            Calculate();
            GetInput();
            HandleMotor();
            HandleSteering();
            UpdateWheels();
        }
        if (isRestarted) Restart();
    }

    private void Calculate()
    {
        kph = transform.InverseTransformDirection(rb.velocity).z * 3.6f;

        wheelsRPM = rearLeftWheelCollider.rpm + rearRightWheelCollider.rpm + frontLeftWheelCollider.rpm + frontRightWheelCollider.rpm;

        speedText.text = Math.Round(kph).ToString();
        audioSrc.pitch = Mathf.Clamp((float)Math.Pow(Mathf.Abs(kph), 0.1f), minPitch, 5f);
    }

    public void Restart()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;        
        Stop();

        
        string lastCheckName = (lastCheckpoint) ? lastCheckpoint.getName() : "";
        if (lastSuccesCheck.Equals("Question_1") && !lastCheckName.Contains("Point") && !lastCheckName.Contains("Taboo")) TruckSpawner.isInit = false;
        if (lastSuccesCheck.Equals("Question_3")) PeopleSpawnerScript.isInit = false;
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = (kph <= 45f) ? Input.GetAxis("Vertical") : 0f ;
        isBreaking = Input.GetKey(KeyCode.Space) || (verticalInput < 0 && kph > 3f || verticalInput > 0 && kph < -3f);
        isRestarted = Input.GetKey(KeyCode.R);
    }

    private void HandleMotor()
    {
        if (isFrontTorgue)
        {
            frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
            frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        }
        if (isRearTorgue)
        {
            rearLeftWheelCollider.motorTorque = verticalInput * motorForce;
            rearRightWheelCollider.motorTorque = verticalInput * motorForce;
        }
        currentbreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();
    }

    private void ApplyBreaking()
    {
        frontRightWheelCollider.brakeTorque = currentbreakForce;
        frontLeftWheelCollider.brakeTorque = currentbreakForce;
        rearLeftWheelCollider.brakeTorque = currentbreakForce;
        rearRightWheelCollider.brakeTorque = currentbreakForce;
    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;
    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheel);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheel);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheel);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheel);
    }

    private void Stop()
    {
        audioSrc.pitch = minPitch;
        kph = 0;
        rb.velocity = Vector3.zero;

        frontRightWheelCollider.brakeTorque = breakForce;
        frontLeftWheelCollider.brakeTorque = breakForce;
        rearLeftWheelCollider.brakeTorque = breakForce;
        rearRightWheelCollider.brakeTorque = breakForce;
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Checkpoint"))
        {
            lastCheckpoint = other.GetComponent<CheckPoint>();
            if (!lastCheckpoint.isEntered())
            {
                //showQuest("ПОЕЗЖАЙ");
                string checkName = lastCheckpoint.getName();
                if (checkName.Contains("Question"))
                {
                    setStopeed(true);
                    Stop();
                    lastSuccesCheck = lastCheckpoint.getName();
                    qa.showQA(lastSuccesCheck);
                }
                else if (checkName.Contains("Point"))
                {
                    updateRestartPosition();
                }
                else if (checkName.Contains("Taboo"))
                {
                    error.GetComponent<AudioSource>().Play();
                    Stop();
                    Restart();
                }
                else if (checkName.Contains("Win"))
                {
                    setStopeed(true);
                    Stop();
                    showQuest("Поздравляем!\n Вы прошли первое занятие");
                }

            }
        }
        else if (other.tag.Equals("People"))
        {
            error.GetComponent<AudioSource>().Play();
            Restart();
        }
    }

    public void setStopeed(bool stop)
    {
        stopped = stop;
    }

    public void updateRestartPosition()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
        lastCheckpoint.setEntered();
    }

    public string getLastCheckPointName()
    {
        return lastCheckpoint.getName();
    }

    public void showQuest(string line)
    {
        StartCoroutine(showQuestText(line));
    }

    private IEnumerator showQuestText(string line)
    {
        questPanel.SetActive(true);
        questPanel.GetComponentInChildren<TextMeshProUGUI>().text = line;
        yield return new WaitForSeconds(3f);
        questPanel.SetActive(false);
    }
}