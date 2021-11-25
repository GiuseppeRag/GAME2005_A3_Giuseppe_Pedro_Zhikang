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

    [Header("Physics System")]
    public CustomPhysicsSystem customPhysicsSystem;

    //Used for stopping the Y of objects when gravity is disabled
    Vector3 tempYVelocityHolder;

    // Start is called before the first frame update
    void Start()
    {
        UIPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;

        //sets the gravity slider to match the system's gravity
        gravityModifierSlider.value = -customPhysicsSystem.gravity;
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

    public void OnResetSceneButtonPressed()
    {
        //Resets the scene
        foreach (CustomPhysicsObject physicsObject in customPhysicsSystem.objectsList)
            physicsObject.ResetObjectState();
    }
}
