using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("UI Objects")]
    public GameObject UIPanel;
    public Toggle enableGravity;
    public Button resetSceneButton;
    public Slider gravityModifierSlider;
    public Slider VelocityModifierSlider;
    public Text BallVelocityIndicator;
    public Dropdown BallSelection;

    [Header("Physics System")]
    public CustomPhysicsSystem customPhysicsSystem;

    [Header("Throw Controller")]
    public List<GameObject> Prefabs;
    public ThrowObjectController throwObjectController;

    //Used to store items to be delete
    List<CustomPhysicsObject> tempDeleteList = new List<CustomPhysicsObject>();

    //Used for stopping the Y of objects when gravity is disabled
    Vector3 tempYVelocityHolder;

    // Start is called before the first frame update
    void Start()
    {
        UIPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;

        //sets the gravity slider to match the system's gravity
        gravityModifierSlider.value = -customPhysicsSystem.gravity;


        throwObjectController.StartingVelocity = VelocityModifierSlider.value;
        BallVelocityIndicator.text = VelocityModifierSlider.value.ToString();

        if (BallSelection.value <= Prefabs.Count)
            throwObjectController.SpawnPrefab = Prefabs[BallSelection.value];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            UIPanel.SetActive(!UIPanel.activeInHierarchy);

            Cursor.lockState = (UIPanel.activeInHierarchy ? CursorLockMode.None : CursorLockMode.Locked);
        }
    }

    public void OnEnableGravityToggle()
    {
        //toggles the slider. There is no need to adjust a slider for a value that is turned off
        gravityModifierSlider.enabled = !gravityModifierSlider.enabled;

        //sets the gravity to 0 or back to the sliders value depending on if it was toggled or not
        if (enableGravity.isOn)
            customPhysicsSystem.gravity = -gravityModifierSlider.value;
        else
            customPhysicsSystem.gravity = 0.0f;

        //Toggles Velocity for all physicsObject so they stop moving in the Y axis entirely
        foreach (CustomPhysicsObject physicsObject in customPhysicsSystem.objectsList)
            physicsObject.ToggleGravity(enableGravity.isOn);
    }

    public void OnGravityScaleModified()
    {
        //adjusts the gravity scale for the physics system
        customPhysicsSystem.gravity = -gravityModifierSlider.value;
    }

    // adjusts the throwing velocity of the ball based on the scale
    public void OnBallVelocityModified()
    {
        throwObjectController.StartingVelocity = VelocityModifierSlider.value;
        BallVelocityIndicator.text = VelocityModifierSlider.value.ToString();
    }

    // Changes the ball when the UI selector is changed
    public void OnBallChange()
    {
        if(BallSelection.value <= Prefabs.Count)
            throwObjectController.SpawnPrefab = Prefabs[BallSelection.value];
    }
    public void OnResetSceneButtonPressed()
    {
        //Resets the scene
        foreach (CustomPhysicsObject physicsObject in customPhysicsSystem.objectsList)
        {
            if (physicsObject.CompareTag("DeleteWhenReset"))
                tempDeleteList.Add(physicsObject);
            else
                physicsObject.ResetObjectState();
        }

        //Delete any object that are tag with DeletWhenReset (Helps with performance and cleanup)
        foreach(CustomPhysicsObject deleteObject in tempDeleteList)
        {
            customPhysicsSystem.objectsList.Remove(deleteObject);
            Destroy(deleteObject.gameObject);
        }
        tempDeleteList.Clear();
    }
}
