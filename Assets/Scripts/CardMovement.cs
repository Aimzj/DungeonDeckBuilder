using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMovement : MonoBehaviour {

    public Texture FrontPlayerCard, BackPlayerCard;

    public Transform TargetTransform
    {
        get
        {
            if (_targetTransform == null)
            {
                GameObject gameObject = new GameObject(this.name + "Target");
                _targetTransform = gameObject.GetComponent<Transform>();
                _targetTransform.position = transform.position;
                _targetTransform.forward = transform.forward;
            }
            return _targetTransform;
        }
    }
    public Transform _targetTransform;

    private float _positionDamp = .2f;

    private float _rotationDamp = .2f;

    //mouse follow
    private bool isFollowing;
    private double angle = 15.0;
    private double smooth = 5.0;

    //hovering
    private bool isHovering;
    public bool isInHand;
    private Vector3 origHandPos;
    [SerializeField]
    private float hoverHeight;
    [SerializeField]
    private float zShift;

    private HandManager handManagerScript;
    private PlayAreaSensor areaSensorScript;

    // Use this for initialization
    void Start () {
        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();
        areaSensorScript = GameObject.Find("PlayArea").GetComponent<PlayAreaSensor>();

        isFollowing = false;

        isHovering = false;

        hoverHeight = 2f;
        zShift = 0.5f;
        //isInHand = false;

    }

	// Update is called once per frame
	void Update () {
        
        //mouse follow
        if (isFollowing)
        {
            transform.position = new Vector3((Input.mousePosition.x - Screen.width / 2) / Screen.width / 0.05f, 2, (Input.mousePosition.y - Screen.width / 8) / Screen.width / 0.05f);

            // Smoothly tilts a transform towards a target rotation.
            double tiltAroundZ = Input.GetAxis("Mouse X") * angle * 2;
            double tiltAroundX = Input.GetAxis("Mouse Y") * angle * 2;
            var target = Quaternion.Euler((float)tiltAroundX, 0, -(float)tiltAroundZ);
            // Dampen towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * (float)smooth);
        }
        else
        {
            SmoothToTargetPositionRotation();
        }
    }

    private void OnMouseDown()
    {
        isFollowing = !isFollowing;
        handManagerScript.isHoldingCard = isFollowing;

        //check to see if the card was released in an area of importance 
        if (areaSensorScript.cardIsPresent)
        {
            //place the card on the table

        }
    }

    private void OnMouseOver()
    {
        //if the card is in the player's hand and they mouse over it, the card should rise towards the player.
        //player must not be holding the card already
        //card must be in the player's hand
        Debug.Log(isInHand);
        if (!handManagerScript.isHoldingCard && isInHand)
        {
            Debug.Log("hover!");
            isHovering = true;
            _targetTransform.position = new Vector3(_targetTransform.position.x, hoverHeight, zShift);
        }
        
    }

    private void OnMouseExit()
    {
        // move the card to it's original transform only if it is already hovering
        if (isHovering)
        {
            Debug.Log("un-hover");
            isHovering = false;
            _targetTransform.position = new Vector3(_targetTransform.position.x, _targetTransform.position.y-hoverHeight, -0.83f);
        }
    }

    private void SmoothToTargetPositionRotation()
    {
        if (TargetTransform.position != transform.position || TargetTransform.eulerAngles != transform.eulerAngles)
        {
            SmoothToPointAndDirection(TargetTransform.position, _positionDamp, TargetTransform.rotation, _rotationDamp);
        }
    }

    private void SmoothToPointAndDirection(Vector3 point, float moveSmooth, Quaternion rotation, float rotSmooth)
    {
        transform.position = Vector3.SmoothDamp(transform.position, point, ref _smoothVelocity, moveSmooth);
        Quaternion newRotation;
        newRotation.x = Mathf.SmoothDamp(transform.rotation.x, rotation.x, ref _smoothRotationVelocity.x, rotSmooth);
        newRotation.y = Mathf.SmoothDamp(transform.rotation.y, rotation.y, ref _smoothRotationVelocity.y, rotSmooth);
        newRotation.z = Mathf.SmoothDamp(transform.rotation.z, rotation.z, ref _smoothRotationVelocity.z, rotSmooth);
        newRotation.w = Mathf.SmoothDamp(transform.rotation.w, rotation.w, ref _smoothRotationVelocity.w, rotSmooth);
        transform.rotation = newRotation;
        TestVisibility();
    }
    private Vector3 _smoothVelocity;
    private Vector4 _smoothRotationVelocity;

    private void TestVisibility()
    {
        float angle = Vector3.Angle(Camera.main.transform.forward, transform.forward);
        if (angle < 90)
        {
            FrontBecameVisible();
        }
        else
        {
            FrontBecameHidden();
        }
    }

    private void FrontBecameVisible()
    {
       // AssetBundle cardBundle = BundleSingleton.Instance.LoadBundle(SourceAssetBundlePath);
      //  GetComponent<Renderer>().material.mainTexture = FrontPlayerCard;
    }

    private void FrontBecameHidden()
    {
      //  Resources.UnloadAsset(GetComponent<Renderer>().material.mainTexture);
       // GetComponent<Renderer>().material.mainTexture = BackPlayerCard;
    }
}
