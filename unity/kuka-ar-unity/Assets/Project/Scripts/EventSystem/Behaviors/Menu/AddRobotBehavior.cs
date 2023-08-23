using System.Collections;
using Project.Scripts.EventSystem.Enums;
using UnityEngine;

public class AddRobotBehavior : MonoBehaviour
{
    [SerializeField]
    private float pullAddMenuMaxHeight = 0.018f;
    
    private AddRobotController robotController;
    private JogsControlService service;
    private GameObject selectOptions;
    private Vector3 homePosition;
    
    private bool isDialogFullyOpen;
    void Start()
    {
        robotController = GetComponent<AddRobotController>();
        service = JogsControlService.Instance;
        selectOptions = robotController.addDialog.transform.Find("SelectOptions")
            .GetComponent<RectTransform>().gameObject;
        
        homePosition = robotController.addDialog.transform.position;
        
        isDialogFullyOpen = false;
        
        robotController.addDialog.SetActive(false);
    }
    
    void Update()
    {   
        if (robotController.IsSliderHold)
        {
            StartCoroutine(DragSlider());
        }
        
        if (robotController.DialogState == LogicStates.Running)
        {
            StartCoroutine(ShowAddDialog());
            selectOptions.SetActive(isDialogFullyOpen);
            service.IsAddRobotDialogOpen = true;
        }
        else if (robotController.DialogState == LogicStates.Hiding)
        {
            StartCoroutine(HideAddDialog());
            service.IsAddRobotDialogOpen = false;
        }
    }

    private IEnumerator ShowAddDialog()
    {
        robotController.addDialog.SetActive(true);
        
        var translation = Vector3.up * (Time.deltaTime * robotController.TransformFactor);
        var newPose = robotController.addDialog.transform.position + translation;
        if (newPose.y > Screen.height * pullAddMenuMaxHeight)
        {
            robotController.addDialog.transform.position = new Vector3(
                robotController.addDialog.transform.position.x, Screen.height * pullAddMenuMaxHeight);
            robotController.DialogState = LogicStates.Waiting;
            
            isDialogFullyOpen = true;
        }

        if (newPose.y > Screen.height * pullAddMenuMaxHeight / 2)
        {
            robotController.bottomNav.SetActive(false);
        }
        robotController.addDialog.transform.Translate(translation);
        yield return null;
    }

    private IEnumerator HideAddDialog()
    {
        var translation = Vector3.down * (Time.deltaTime * robotController.TransformFactor);
        var newPose = robotController.addDialog.transform.position + translation;
        
        if (newPose.y < homePosition.y)
        {
            translation = new Vector3();
            robotController.DialogState = LogicStates.Waiting;
        }

        if (newPose.y < Screen.height * pullAddMenuMaxHeight / 2 )
        {
            robotController.bottomNav.SetActive(true);
        }
        
        isDialogFullyOpen = false;
        robotController.addDialog.transform.Translate(translation);
        yield return null;
    }

    private IEnumerator DragSlider()
    {
        var menuPosition = new Vector3(homePosition.x ,Input.mousePosition.y);
        if (menuPosition.y > Screen.height * pullAddMenuMaxHeight)
        {
            menuPosition.y = Screen.height * pullAddMenuMaxHeight;
        }
        
        if (menuPosition.y < Screen.height * pullAddMenuMaxHeight / 2)
        {
            robotController.DialogState = LogicStates.Hiding;   
            yield return null;
        }
        robotController.addDialog.transform.position = menuPosition;
        yield return null;
    }
}
