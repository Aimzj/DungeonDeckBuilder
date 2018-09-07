using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMovement : MonoBehaviour {
    public Transform PlayerDeck, EnemyDeck;
    public Transform PlayerDiscard, EnemyDiscard;
    public Transform PlayerTrash, EnemyTrash;

    public Texture FrontPlayerCard, BackPlayerCard;

    //list of x positions for the cards in the player's hand
    public List<float> HandPositions;
    //distance between the cards in the player's hand.
    private float x;
    //the x value that the cards should not cross. The distance between cards is made smaller when this value is crossed
    private float boundaryValue;

    [SerializeField]
    private int numCardsInHand;

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
    private Transform _targetTransform;

    private float _positionDamp = .2f;

    private float _rotationDamp = .2f;

    // Use this for initialization
    void Start () {
        numCardsInHand= 0;

        x = 3;
        boundaryValue = 6;
	}
	
    public void AddCardToHand()
    {
        //  Instantiate(cardObj, PlayerDeck.transform.position, Quaternion.identity);
        numCardsInHand++;
        HandPositions.Add(0);
        float newCardPos = 0;
        //check if number of cards is odd or even
        if(numCardsInHand>1)
        {
            //number is even
            float multiplier = (numCardsInHand - 1) / 2;
            //check to see that the boundary value hasn't been crossed
            //if it has been crossed, make the distance between cards smaller
            newCardPos = multiplier* x;
            while (newCardPos > boundaryValue)
            {
                x -= 0.1f;
                newCardPos = multiplier * x;
            }

            //start on the right hand side
            //move from right to left
            for (int i = 0; i < numCardsInHand - 1; i++)
            {
                newCardPos = multiplier * x;
                HandPositions[i] = newCardPos;
                multiplier -= 1f;
               /* if (i< (numCardsInHand / 2) - 1)
                {
                    //right hand side
                    multiplier -= 1
                    HandPositions[i] = newCardPos;
                }
                else
                {
                    //left hand side
                    HandPositions[i] = -newCardPos;
                }*/
                
            }
        }
        /*else
        {
            //number is odd
            if (numCardsInHand > 1)
            {
                float multiplier = (numCardsInHand - 1)/2;

                newCardPos = multiplier * x;
                while (newCardPos > boundaryValue)
                {
                    x -= 0.1f;
                    newCardPos = multiplier * x;
                }
            }
            else
            {
                //only one card in hand
                HandPositions[0] = 0;
            }
        }*/
    }

	// Update is called once per frame
	void Update () {
        SmoothToTargetPositionRotation();

        if (Input.GetKeyDown(KeyCode.D))
        {
            _targetTransform = PlayerDeck;
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
