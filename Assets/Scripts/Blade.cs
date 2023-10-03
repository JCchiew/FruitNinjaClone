using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    private Camera mainCamera;
    private Collider bladeCollider;
    private TrailRenderer bladeTrail;
    [SerializeField]
    private bool slicing;
    [SerializeField]
    private float minMouseVelocity = 0.01f;
    private bool audioFlag = false;

    public float sliceForce = 5f;

    public Vector3 Direction { get; private set; }

    private void Awake()
    {
        mainCamera = Camera.main;
        bladeCollider = GetComponent<Collider>();
        bladeTrail = GetComponentInChildren<TrailRenderer>();
    }

    private void OnEnable()
    {
        StopSlicing();
    }

    private void OnDisable()
    {
        StopSlicing();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StartSlicing();
        }  
        else if (Input.GetMouseButtonUp(0))
        {
            StopSlicing();
        }
        else if (slicing)
        {
            ContinueSlicing();
        }
    }

    private void StartSlicing()
    {
        Vector3 newPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = 0f;

        transform.position = newPos;

        slicing = true;
        bladeCollider.enabled = true;
        bladeTrail.enabled = true;
        bladeTrail.Clear();
    }

    private void StopSlicing()
    {
        slicing = false;
        bladeCollider.enabled = false;
        bladeTrail.enabled = false;
    }

    private void ContinueSlicing()
    {
        Vector3 newPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        newPos.z = 0f;

        Direction = newPos - transform.position;

        float velocity = Direction.magnitude / Time.deltaTime;
        bladeCollider.enabled = velocity > minMouseVelocity;

        transform.position = newPos;

        if(audioFlag == false && velocity > 100)
        {
            StartCoroutine(AudioInterval());
        }    
    }

    private IEnumerator AudioInterval()
    {
        AudioManager.Instance.PlaySoundEffect("BladeSlash");
        audioFlag = true;
        yield return new WaitForSeconds(0.5f);
        audioFlag = false;
    }
}
