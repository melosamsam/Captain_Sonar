using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class Captain : Role
{
    public enum Direction { North, East, South, West, None }
    public Direction ChosenCourse { get; private set; }
    public override bool ActionDone { 
        get => base.ActionDone;
        protected set
        {
            if (base.ActionDone != value)
            {
                base.ActionDone = value;
                OnActionStatusChanged();
            }
        }
    }


    // Awake is called when an enabled script instance is being loaded
    private void Awake()
    {
        SetDescription();
    }

    // Start is called before the first frame update
    void Start()
    {
        PerformRoleAction();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void PerformRoleAction()
    {
        // setting the turn as not done when it just began
        ActionDone = false;
        Debug.Log("Captain role started\n" + Description);
    }

    protected override void SetDescription()
    {
        Description =
            "The captain is the central element of the entire crew.\n" +
            "In addition to being responsible for the trajectory taken by the submarine, they must be the link between all other posts."
            ;
    }

    /// <summary>
    /// Method called when the turn of the Captain finishes or ends.
    /// </summary>
    void OnActionStatusChanged()
    {
        ToggleUI();
    }

    /// <summary>
    /// Updates the Captain's given course to direct the whereabouts of the submarine.
    /// </summary>
    /// <param name="courseChar">The letter indicating which direction to go</param>
    public void OrderSubmarineCourse(string courseChar)
    {
        Direction course = Direction.None;
        switch (courseChar)
        {
            case "N":
                course = Direction.North;
                break;
            case "S":
                course = Direction.South;
                break;
            case "E":
                course = Direction.East;
                break;

            case "W":
                course = Direction.West;
                break;
        }

        // if course in Submarine.GetPossibleCourses
        ChosenCourse = course;
        // else, show error message and allow another input

        Debug.Log($"Chosen course: {ChosenCourse}");
        ActionDone = true;
    }

    /// <summary>
    /// Calling the submarine to surface before doing anything else
    /// </summary>
    public void OrderSurface()
    {
        Debug.Log("Surface ordered");
        // call the Surface() method in the Submarine class


        ActionDone = true;
    }

    /// <summary>
    /// Test method to emulate a turn starting and finishing
    /// </summary>
    public void ToggleActionDone()
    {
        ActionDone = !ActionDone;
        Debug.Log($"Action done: {ActionDone}");
    }

    /// <summary>
    /// Method enabling/disabling the UI elements used to perform role actions according to whether the turn is done or not.
    /// </summary>
    public void ToggleUI()
    {
        GameObject actions = GameObject.Find("Actions");
        Button[] ui = actions.GetComponentsInChildren<Button>();
        foreach (Button button in ui)
        {
            button.enabled = !ActionDone;
        }
    }
}
