using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
    public bool isHovering;
    public bool isInHand;
    private Vector3 origHandPos;
    [SerializeField]
    private float hoverHeight;
    [SerializeField]
    private float zShift;

    //position tracking
    public int posInHand;

    //play area
    public bool isPlayed, hasBurnEffect;

    private HandManager handManagerScript;
    private AreaSensor areaSensorScript;
    private AreaManager areaManagerScript;
    private CardEffectManager cardEffectScript;
    private StatsManager statManagerScript;
    private GameManager gameManagerScript;

    //sound
    private SoundManager soundScript;

    //if the card is an enemy card, it MUST NOT be manipulated by the player
    public bool isEnemyCard;

    //bool is used to prevent player from interacting with scripts when they shouldn't
    public bool isFrozen;

    public bool isKindling;

    //for Tut
    private Dummy dummyScript;
    private CardGenerator cardGeneratorScript;

    // Use this for initialization
    void Start () {
        handManagerScript = GameObject.Find("GameManager").GetComponent<HandManager>();
        areaSensorScript = GameObject.Find("GameManager").GetComponent<AreaSensor>();
        areaManagerScript = GameObject.Find("GameManager").GetComponent<AreaManager>();
        cardEffectScript = GameObject.Find("GameManager").GetComponent<CardEffectManager>();
        statManagerScript = GameObject.Find("GameManager").GetComponent<StatsManager>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();

        soundScript = GameObject.Find("SoundMaker").GetComponent<SoundManager>();
        
     
        cardGeneratorScript = GameObject.Find("GameManager").GetComponent<CardGenerator>();
        if(cardGeneratorScript.level == 0)
        {
            dummyScript = GameObject.Find("GameManager").GetComponent<Dummy>();
        }
      

        isFollowing = false;

        isHovering = false;

        hoverHeight = 2f;
        zShift = 0.5f;

        isPlayed= false;

        isFrozen = false;

       // isEnemyCard = false;
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
            var target = Quaternion.Euler((float)tiltAroundX+90, 0, -(float)tiltAroundZ);
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
        if (!isFrozen)
        {
            //check if the card is in the player's hand
            if (!isEnemyCard)
            {
                if (isInHand)
                {
                    //play sound
                    soundScript.PlaySound_PickUpCard();

                    isFollowing = true;
                    handManagerScript.isPlayerHoldingCard = isFollowing;
                    isHovering = false;

                    //change the order in layer of card and text
                    ChangeOrder(42);
                }
            }
        }
       
    }

    public void ChangeOrder(int num)
    {
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = num;

        gameObject.transform.Find("Title").GetComponent<TextMeshPro>().sortingOrder = num + 1;
        gameObject.transform.Find("DiscardCost").GetComponent<TextMeshPro>().sortingOrder = num + 1;
        gameObject.transform.Find("BurnCost").GetComponent<TextMeshPro>().sortingOrder = num + 1;
        gameObject.transform.Find("DiscardEffect").GetComponent<TextMeshPro>().sortingOrder = num + 1;
        gameObject.transform.Find("BurnEffect").GetComponent<TextMeshPro>().sortingOrder = num + 1;
        gameObject.transform.Find("AttackCost").GetComponent<TextMeshPro>().sortingOrder = num + 1;
        gameObject.transform.Find("DefenseCost").GetComponent<TextMeshPro>().sortingOrder = num + 1;
        gameObject.transform.Find("Sigil").GetComponent<SpriteRenderer>().sortingOrder = num + 1;
        gameObject.transform.Find("Image").GetComponent<SpriteRenderer>().sortingOrder = num + 1;

    }
    

    public void PlayEnemyCard()
    {
        //play sound
        soundScript.PlaySound_PlayCard();

        //remove the card from the Hand List
        handManagerScript.Call_RemoveCardFromHand(this.posInHand, "enemy");
        posInHand = -1;

        isPlayed = true;
        isInHand = false;

        areaManagerScript.Call_PlayCard(this.gameObject, "enemy");

        handManagerScript.ReorderHandLayers("enemy");
        //play card with effects
       // cardEffectScript.PlayCard(this.gameObject, false);
    }

    public void DiscardEnemyCard()
    {
        //play discard sound
        soundScript.PlaySound_PlayCard();

        //remove the card from the hand list
        handManagerScript.Call_RemoveCardFromHand(this.posInHand, "enemy");
        posInHand = -1;

        isPlayed = true;
        isInHand = false;

        areaManagerScript.Call_DiscardCard(this.gameObject, "enemy");

        handManagerScript.ReorderHandLayers("enemy");
    }

    public void TrashEnemyCard()
    {
        //play sound
        soundScript.PlaySound_PlayCard();

        //remove the card from the Hand List
        handManagerScript.Call_RemoveCardFromHand(this.posInHand, "enemy");
        posInHand = -1;

        isPlayed = true;
        isInHand = false;

        areaManagerScript.Call_TrashCard(this.gameObject, "enemy");

        handManagerScript.ReorderHandLayers("enemy");
    }

 


    public void PlayPlayerCard()
    {
        //place the card on the table

        //only if the hand size is not exceeded
        //and if the player can "afford" it
        if (cardGeneratorScript.level == 0)
        {
            print("TEST");
            if (dummyScript.TurnCount == 0)
            {
                if (gameObject.GetComponent<CardObj>().CardName == "Strike" || gameObject.GetComponent<CardObj>().CardName == "Lucky Charm" || gameObject.GetComponent<CardObj>().CardName == "Guard")
                {
                    playerPlayCardFilter();
                }
            }
            else if (dummyScript.TurnCount == 1)
            {
                if (gameObject.GetComponent<CardObj>().CardName == "Strike" || gameObject.GetComponent<CardObj>().CardName == "Focused Strike")
                {
                    playerPlayCardFilter();
                }
            }
        }
        else
        {
            playerPlayCardFilter();
        }
       
    }


    void playerPlayCardFilter()
    {
        if (!handManagerScript.isExceedingHandSize
           && gameObject.GetComponent<CardObj>().DiscardCost <= statManagerScript.numDiscard_player)
        {
            //play sound
            soundScript.PlaySound_PlayCard();

            //remove the card from the Hand List
            handManagerScript.Call_RemoveCardFromHand(this.posInHand, "player");
            posInHand = -1;

            isPlayed = true;
            isInHand = false;

            areaManagerScript.Call_PlayCard(this.gameObject, "player");

            //play the card's standard effects
            cardEffectScript.PlayCard(this.gameObject, false);

            //check if the card has burn effects and if the player can afford them
            int burnCost = this.gameObject.GetComponent<CardObj>().BurnCost;
            if (burnCost > 0
                && statManagerScript.numBurn_player >= burnCost)
            {
                //ask the player if they want to use the effect
                gameManagerScript.DisplayBurnUI(this.gameObject);
            }

        }
    }
    private void OnMouseUp()
    {
        if (!isEnemyCard
            && !isFrozen
            && !isPlayed)
        {
            isFollowing = false;
            handManagerScript.isPlayerHoldingCard = isFollowing;
            isHovering = false;

            ChangeOrder(15);

            //check to see if the card was released in an area of importance 
            if (areaSensorScript.cardIsPresent)
            {

                //check which area of importance
                //PLAY
                if (areaSensorScript.isPlay)
                {

                    PlayPlayerCard();
                }
                //DISCARD
                else if (areaSensorScript.isDiscard)
                {
                    //play sound
                    soundScript.PlaySound_PlayCard();

                    //remove the card from the Hand List
                    handManagerScript.Call_RemoveCardFromHand(this.posInHand,"player");
                    posInHand = -1;

                    isPlayed = true;
                    isInHand = false;

                    areaManagerScript.Call_DiscardCard(this.gameObject, "player");
                }
                //TRASH
                else if (areaSensorScript.isTrash)
                {
                    //play sound
                    soundScript.PlaySound_PlayCard();

                    //remove the card from the Hand List
                    handManagerScript.Call_RemoveCardFromHand(this.posInHand, "player");
                    posInHand = -1;

                    isPlayed = true;
                    isInHand = false;

                    areaManagerScript.Call_TrashCard(this.gameObject, "player");
                }
            }
            else
            {
                if (SceneManager.GetActiveScene().name == "BetaScene")
                {
                    isHovering = true;
                    _targetTransform.position = new Vector3(_targetTransform.position.x, _targetTransform.position.y, -0.83f);
                }

            }
        }
        
    }

    private void OnMouseEnter()
    {
        //if the card is in the player's hand and they mouse over it, the card should rise towards the player.
        //player must not be holding the card already
        //card must be in the player's hand

        if (!handManagerScript.isPlayerHoldingCard 
            && isInHand
            && !isEnemyCard
            && !isFrozen)
        {
            //play sound
            soundScript.PlaySound_HoverCard();

            isHovering = true;
            _targetTransform.position = new Vector3(_targetTransform.position.x, hoverHeight, zShift);
            ChangeOrder(41);
        }
        
    }

    private void OnMouseExit()
    {
        // move the card to it's original transform only if it is already hovering
        if (isHovering
            && !isEnemyCard
            && !isFrozen)
        {
            isHovering = false;
            _targetTransform.position = new Vector3(_targetTransform.position.x, _targetTransform.position.y-hoverHeight, -0.83f);

            handManagerScript.ReorderHandLayers("player");
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
