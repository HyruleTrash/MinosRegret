using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]

public class Player : MonoBehaviour
{
    public string[] collidables;

    public GameObject Hat;
    public GameObject HatRigid;
    public Color[] colorModifier;
    public float[] speedModifierHat;

    public  List<float> currentSpeedModifiers = new List<float>();
    int hatsOn = 0;

    public float speedModifierPowerup = 0.3f;
    public float PowerupTimeOut = 2f;
    public int currentPowerUps = 0;

    public float Coins = 0f;

    public List<Item> Items;
    public GameObject[] ItemsVisualHold;
    public int currentlyholding = -1;
    public float currentlyholdingTemp = -1;
    public float Scrollspeed = 1;
    public GameObject UI;
    public GameObject GameVars;
    private KeyCode[] keyCodes = {
         KeyCode.Alpha1,
         KeyCode.Alpha2,
         KeyCode.Alpha3,
         KeyCode.Alpha4,
         KeyCode.Alpha5,
         KeyCode.Alpha6,
         KeyCode.Alpha7,
         KeyCode.Alpha8,
         KeyCode.Alpha9,
     };

    public float timeBetweenPickUpMax = 0.35f;
    float timeBetweenPickUp = 0f;
    float pickUpStep = 1f;
    public bool pickUpCounter = false;

    public float walkingSpeedNormal = 0.15f;
    public float MaxwalkingSpeedNormal = 5f;
    public float SlowDownHNormal = 0.1f;
    public float SlowDownVNormal = 0.05f;

    public float walkingSpeedAirBorn = 0.2f;
    public float MaxwalkingSpeedAirBorn = 10f;
    public float SlowDownHAirBorn = 0.12f;
    public float SlowDownVAirBorn = 0.04f;

    public float jumpSpeed = 8.0f;
    bool falling = false;
    bool jumped = false;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    float curSpeedX = 0;
    float curSpeedY = 0;

    public Text cointex;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    public bool canMove = true;




    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        FindObjectOfType<AudioManager>().Play("Wind");
    }

    void Update()
    {
        UpdateInventory();
        UpdatePowerupStatus();

        float speedModifierTemp = 1;
        for (int i = 0; i < currentSpeedModifiers.Count; i++)
        {
            speedModifierTemp += currentSpeedModifiers[i];
        }
        speedModifierTemp -= 0.2f * (hatsOn - 1);
        if (speedModifierTemp < 0)
        {
            speedModifierTemp = 0;
        }

        if (pickUpCounter == true)
        {
            timeBetweenPickUp += pickUpStep * Time.deltaTime;
            if (timeBetweenPickUp >= timeBetweenPickUpMax)
            {
                pickUpCounter = false;
                timeBetweenPickUp = 0;
            }
        }

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        float walkingSpeed = (characterController.isGrounded ? walkingSpeedNormal : walkingSpeedAirBorn) * speedModifierTemp;
        float MaxwalkingSpeed = (characterController.isGrounded ? MaxwalkingSpeedNormal : MaxwalkingSpeedAirBorn) * speedModifierTemp;
        float SlowDownH = (characterController.isGrounded ? SlowDownHNormal : SlowDownHAirBorn);
        float SlowDownV = (characterController.isGrounded ? SlowDownVNormal : SlowDownVAirBorn);
        //WASD & arrow movement
        curSpeedX += canMove ? walkingSpeed * Input.GetAxis("Vertical") : 0;
        curSpeedY += canMove ? walkingSpeed * Input.GetAxis("Horizontal") : 0;
        //speed cap
        curSpeedX = curSpeedX > MaxwalkingSpeed ? MaxwalkingSpeed : (curSpeedX < -MaxwalkingSpeed ? -MaxwalkingSpeed : curSpeedX);
        curSpeedY = curSpeedY > MaxwalkingSpeed ? MaxwalkingSpeed : (curSpeedY < -MaxwalkingSpeed ? -MaxwalkingSpeed : curSpeedY);
        //when not moving slide
        curSpeedX -= Input.GetAxis("Vertical") == 0 ? curSpeedX * SlowDownV : 0;
        curSpeedY -= Input.GetAxis("Horizontal") == 0 ? curSpeedY * SlowDownH : 0;


        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
            FindObjectOfType<AudioManager>().Play("Jump");
            jumped = true;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (characterController.velocity.y < -5)
        {
            falling = true;
        }
        if (characterController.isGrounded == true)
        {
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                FindObjectOfType<AudioManager>().Play("Walk");
            }
            else
            {
                FindObjectOfType<AudioManager>().Stop("Walk");
            }
        }
        else
        {
            FindObjectOfType<AudioManager>().Stop("Walk");
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove && UI.GetComponent<UIscript>().GamePaused == false)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
        

        // Inventory Input
        if (Input.GetAxis("Mouse ScrollWheel") < 0.0f && (currentlyholdingTemp - Scrollspeed) > (-1 - Scrollspeed))
        {
            currentlyholdingTemp -= Scrollspeed;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0.0f && (int)Math.Round((currentlyholdingTemp + Scrollspeed), 0) < Items.Count)
        {
            currentlyholdingTemp += Scrollspeed;
        }

        if (currentlyholding != (int)Math.Round(currentlyholdingTemp, 0))
        {
            FindObjectOfType<AudioManager>().Play("Select");
        }
        currentlyholding = (int)Math.Round(currentlyholdingTemp, 0);

        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]) && i < Items.Count)
            {
                currentlyholding = i;
                currentlyholdingTemp = i;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pickUpCounter == false)
        {
            switch (other.name)
            {
                case "BullCoin Variant(Clone)":
                    GameVars.GetComponent<GameVariables>().Coins++;
                    pickUpCounter = true;
                    other.transform.parent.parent.GetComponent<LineSpawner>().Spawning = true;
                    Destroy(other.gameObject);
                    Coins++;
                    cointex.text = "Coins: " + Coins;
                    FindObjectOfType<AudioManager>().Play("Coin");
                    break;
                case "PowerUp Variant(Clone)":
                    GameVars.GetComponent<GameVariables>().PowerUpsCollected++;
                    pickUpCounter = true;
                    currentSpeedModifiers.Add(speedModifierPowerup);
                    currentPowerUps++;
                    this.gameObject.AddComponent<PowerupRemove>();
                    FindObjectOfType<AudioManager>().Play("PowerUp");
                    Destroy(other.gameObject);
                    break;
                case "hat Variant(Clone)":
                    pickUpCounter = true;
                    if (Items.Count < UI.GetComponent<UIscript>().Inventory.Count)
                    {
                        Items.Add(new Item("Hat", other.GetComponent<Hat>().grade, false));
                        FindObjectOfType<AudioManager>().Play("Hat");
                        Destroy(other.gameObject);
                    }
                    break;
                case "EndTrigger":
                    GameVars.GetComponent<GameVariables>().SetPlayTime();
                    SceneManager.LoadScene("EndGame");
                    break;
                case "Tutorial End Trigger":
                    other.gameObject.GetComponent<TutorialEnd>().StartGame();
                    break;
            }
        }
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Collider other = hit.collider;
        string colliderName = other.name;
        for (int i = 0; i < collidables.Length; i++)
        {
            if (colliderName.Contains(collidables[i]) == true)
            {
                colliderName = collidables[i];
            }
        }
        switch (colliderName)
        {
            case "hatRigidVariant Variant(Clone)":
                if (pickUpCounter == false)
                {
                    pickUpCounter = true;
                    if (Items.Count < UI.GetComponent<UIscript>().Inventory.Count)
                    {
                        FindObjectOfType<AudioManager>().Play("Hat");
                        Items.Add(new Item("Hat", other.GetComponent<Hat>().grade, false));
                        Destroy(other.gameObject);
                    }
                }
                break;
            case "FountainMiddlePiece":
            case "Pillar":
            case "FountainCirclePiece":
            case "Lever":
            case "Ring":
            case "PuzzlePieaceHolder":
            case "Map":
                if (characterController.isGrounded == true)
                {
                    if (jumped == true && falling == false)
                    {
                        jumped = false;
                        FindObjectOfType<AudioManager>().Play("Land");
                    }
                    if (falling == true)
                    {
                        falling = false;
                        FindObjectOfType<AudioManager>().Play("Land");
                    }
                }
                break;
        }
        
    }

    public void Use()
    {
        if (currentlyholding <= Items.Count && currentlyholding >= 0)
        {
            switch (Items[currentlyholding].name)
            {
                case "PuzzlePiece|0":
                    break;
                case "PuzzlePiece|1":
                    break;
                case "PuzzlePiece|2":
                    break;
                case "PuzzlePiece|3":
                    break;
                case "Hat":
                    FindObjectOfType<AudioManager>().Play("Equip");
                    Items[currentlyholding].equiped = !Items[currentlyholding].equiped;
                    if (Items[currentlyholding].equiped == true)
                    {
                        hatsOn++;
                        currentSpeedModifiers.Add(speedModifierHat[Items[currentlyholding].grade]);
                        GameObject tempHat = Instantiate(Hat, this.transform.GetChild(2).gameObject.transform.GetChild(this.transform.GetChild(2).childCount - 1).gameObject.transform);
                        tempHat.transform.parent = this.transform.GetChild(2).gameObject.transform;
                        tempHat.transform.position += new Vector3(0f, 0.477f, 0f);
                        tempHat.transform.localScale = new Vector3(0.08f, 0.08f, 0.08f);
                    }
                    else
                    {
                        hatsOn--;
                        currentSpeedModifiers.Remove(speedModifierHat[Items[currentlyholding].grade]);
                        Destroy(this.transform.GetChild(2).gameObject.transform.GetChild(this.transform.GetChild(2).childCount - 1).gameObject);
                    }
                    break;
            }
        }
    }
    public void Drop(float force, Vector3 startPositionOverride, float range)
    {
        if (currentlyholding <= Items.Count && currentlyholding >= 0)
        {
            Transform cameraLookDirection = this.transform.GetChild(0).transform;
            Vector3 hatSize = cameraLookDirection.forward * 0.75f;
            Vector3 dropPosition = this.transform.position + (range * cameraLookDirection.forward - hatSize);
            if (startPositionOverride != new Vector3(0,0,0))
            {
                dropPosition = startPositionOverride - hatSize;
            }
            dropPosition += new Vector3(0, 0.34f, 0);
            switch (Items[currentlyholding].name)
            {
                case "PuzzlePiece|0":
                    break;
                case "PuzzlePiece|1":
                    break;
                case "PuzzlePiece|2":
                    break;
                case "PuzzlePiece|3":
                    break;
                case "Hat":
                    FindObjectOfType<AudioManager>().Play("Throw");
                    if (Items[currentlyholding].equiped == true)
                    {
                        hatsOn--;
                        currentSpeedModifiers.Remove(speedModifierHat[Items[currentlyholding].grade]);
                        Destroy(this.transform.GetChild(2).gameObject.transform.GetChild(this.transform.GetChild(2).childCount - 1).gameObject);
                        force -= 50;
                    }
                    GameObject tempHat = Instantiate(HatRigid, dropPosition, Quaternion.Euler(0, 0, 0));
                    tempHat.GetComponent<Hat>().GameVars = GameVars;
                    tempHat.GetComponent<Hat>().grade = Items[currentlyholding].grade;
                    tempHat.GetComponent<Rigidbody>().AddForce(force * cameraLookDirection.forward + characterController.velocity * 30f);
                    Items.RemoveAt(currentlyholding);
                    if (currentlyholding == Items.Count)
                    {
                        currentlyholding = Items.Count - 1;
                        currentlyholdingTemp = Items.Count - 1f;
                    }
                    break;
            }
        }
    }

    public void UpdateInventory()
    {
        if (Input.GetKeyDown(KeyCode.P) == true)
        {
            UI.GetComponent<UIscript>().GamePaused = !UI.GetComponent<UIscript>().GamePaused;
            if (UI.GetComponent<UIscript>().GamePaused == false)
            {
                UI.GetComponent<UIscript>().Continue();
            }
        }
        if (UI.GetComponent<UIscript>().GamePaused == true)
        {
            Time.timeScale = 0;
            UI.GetComponent<UIscript>().ActivatePauseMenu();
        }
        else
        {
            Time.timeScale = 1;
        }

        if (currentlyholding <= Items.Count && currentlyholding >= 0)
        {
            if (Items[currentlyholding].equiped == false)
            {
                switch (Items[currentlyholding].name)
                {
                    case "PuzzlePiece|0":
                    case "PuzzlePiece|1":
                    case "PuzzlePiece|2":
                    case "PuzzlePiece|3":
                        ItemsVisualHold[0].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                        ItemsVisualHold[0].transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
                        ItemsVisualHold[1].GetComponent<MeshRenderer>().enabled = true;
                        break;
                    case "Hat":
                        ItemsVisualHold[0].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                        ItemsVisualHold[0].transform.GetChild(1).GetComponent<MeshRenderer>().enabled = true;
                        ItemsVisualHold[1].GetComponent<MeshRenderer>().enabled = false;

                        switch (Items[currentlyholding].grade)
                        {
                            default:
                                ItemsVisualHold[0].transform.GetChild(0).GetComponent<Renderer>().material.color = colorModifier[Items[currentlyholding].grade];
                                ItemsVisualHold[0].transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_Metallic", 0f);
                                ItemsVisualHold[0].transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_Glossiness", 0f);
                                ItemsVisualHold[0].transform.GetChild(1).GetComponent<Renderer>().material.color = colorModifier[Items[currentlyholding].grade];
                                ItemsVisualHold[0].transform.GetChild(1).GetComponent<Renderer>().material.SetFloat("_Metallic", 0f);
                                ItemsVisualHold[0].transform.GetChild(1).GetComponent<Renderer>().material.SetFloat("_Glossiness", 0f);
                                break;
                            case 4:
                                ItemsVisualHold[0].transform.GetChild(0).GetComponent<Renderer>().material.color = colorModifier[Items[currentlyholding].grade];
                                ItemsVisualHold[0].transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_Metallic", 1f);
                                ItemsVisualHold[0].transform.GetChild(0).GetComponent<Renderer>().material.SetFloat("_Glossiness", 0.555f);
                                ItemsVisualHold[0].transform.GetChild(1).GetComponent<Renderer>().material.color = colorModifier[Items[currentlyholding].grade];
                                ItemsVisualHold[0].transform.GetChild(1).GetComponent<Renderer>().material.SetFloat("_Metallic", 1f);
                                ItemsVisualHold[0].transform.GetChild(1).GetComponent<Renderer>().material.SetFloat("_Glossiness", 0.555f);
                                break;
                        }
                        break;
                    default:
                        ItemsVisualHold[0].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                        ItemsVisualHold[0].transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
                        ItemsVisualHold[1].GetComponent<MeshRenderer>().enabled = false;
                        break;
                }
            }
            else
            {
                ItemsVisualHold[0].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                ItemsVisualHold[0].transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
                ItemsVisualHold[1].GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else
        {
            ItemsVisualHold[0].transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
            ItemsVisualHold[0].transform.GetChild(1).GetComponent<MeshRenderer>().enabled = false;
            ItemsVisualHold[1].GetComponent<MeshRenderer>().enabled = false;
        }

        UI.GetComponent<UIscript>().Selection = currentlyholding;
        for (int i = 0; i < UI.GetComponent<UIscript>().Inventory.Count; i++)
        {
            int ItemID = 0;
            bool equiped = false;
            if (i < Items.Count)
            {
                switch (Items[i].name)
                {
                    case "PuzzlePiece|0":
                        ItemID = 1;
                        break;
                    case "PuzzlePiece|1":
                        ItemID = 2;
                        break;
                    case "PuzzlePiece|2":
                        ItemID = 3;
                        break;
                    case "PuzzlePiece|3":
                        ItemID = 4;
                        break;
                    case "Hat":
                        ItemID = 5 + Items[i].grade;
                        equiped = Items[i].equiped;
                        break;
                }
            }
            UI.GetComponent<UIscript>().change(i, ItemID, equiped);
        }
    }
    public void UpdatePowerupStatus()
    {
        if (currentPowerUps != 0)
        {
            if (this.transform.GetChild(0).transform.GetChild(4).GetComponent<ParticleSystem>().isPlaying == false)
            {
                this.transform.GetChild(0).transform.GetChild(4).GetComponent<ParticleSystem>().Play();
            }
            this.transform.GetChild(0).transform.GetChild(5).GetComponent<PostProcessVolume>().enabled = true;
        }
        else
        {
            FindObjectOfType<AudioManager>().Stop("WindPowerUP");
            this.transform.GetChild(0).transform.GetChild(4).GetComponent<ParticleSystem>().Stop();
            this.transform.GetChild(0).transform.GetChild(5).GetComponent<PostProcessVolume>().enabled = false;
        }
    }
}
