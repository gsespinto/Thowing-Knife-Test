using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrowKnife : MonoBehaviour
{
    #region Variables
    [Header("Needed Components / Prefabs")]
    [SerializeField] GameObject knifePrefab;
    [SerializeField] Transform handSocket;
    [SerializeField] GameObject playerModel;
    [Space(10)]

    [Header("Throw Force")]
    [SerializeField] float maxForce = 20.0f;
    [SerializeField] float minForce = 1.0f;
    [SerializeField] float forceIncreaseFactor = 1.0f;
    [Space(10)]

    [Header("Visual Cues")]
    [SerializeField] private GameObject knifePointer;
    [SerializeField] private Slider throwSlider;
    [Space(10)]

    [SerializeField] Vector3 teleportOffset = Vector3.zero;

    Vector3 throwDirection = Vector3.zero;
    float force = 0.0f;
    private GameObject currentKnife;
    [HideInInspector] public bool bLookingAtThrow = false;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        knifePointer.SetActive(false);
        throwSlider.gameObject.SetActive(false);
        throwSlider.minValue = minForce / maxForce;
    }

    // Update is called once per frame
    void Update()
    {
        // Sets initial values for throw, enabling visual cues
        if (Input.GetButtonDown("Fire1"))
        {
            force = minForce;
            knifePointer.SetActive(true);
            throwSlider.gameObject.SetActive(true);
        }

        // Calculates the direction of throw, rotates player model towards that direction, increases throw force until max
        if (Input.GetButton("Fire1"))
        {
            knifePointer.transform.position = GetWorldRascastPoint();
            throwDirection = (GetWorldRascastPoint() - handSocket.position).normalized;
            throwSlider.value = force / maxForce;

            CalculateThrowForce();
            RotateModel();
        }

        // Instantiates a knife (destroying another if existent), adds force to knife and disables visual cues
        if (Input.GetButtonUp("Fire1"))
        {
            bLookingAtThrow = false;
            knifePointer.SetActive(false);
            throwSlider.gameObject.SetActive(false);

            Throw();
        }

        // Teleports player character to knife position
        if (Input.GetButtonDown("Jump"))
        {
            TeleportToKnife();
        }
    }

    void Throw()
    {
        Destroy(currentKnife);

        currentKnife = GameObject.Instantiate(knifePrefab);
        currentKnife.transform.position = handSocket.transform.position;

        Rigidbody rigidbody = currentKnife.GetComponent<Rigidbody>();
        rigidbody.AddForce(throwDirection * force);
    }

    void RotateModel()
    {
        bLookingAtThrow = true;
        Vector3 directionAux = throwDirection;
        directionAux.y = 0.0f;
        playerModel.transform.LookAt(playerModel.transform.position + directionAux);
    }

    void CalculateThrowForce()
    {
        if (force < maxForce)
        {
            force += forceIncreaseFactor;
        }
    }

    Vector3 GetWorldRascastPoint()
    {
        RaycastHit hit;
        Vector3 hitPos = Vector3.zero;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            hitPos = hit.point;
        }

        return hitPos;
    }

    void TeleportToKnife()
    {
        if (currentKnife == null)
        {
            return;
        }

        if (!currentKnife.GetComponent<Knife>().bHasHitGround)
        {
            return;
        }

        this.transform.position = currentKnife.transform.position + teleportOffset;
    }
}
