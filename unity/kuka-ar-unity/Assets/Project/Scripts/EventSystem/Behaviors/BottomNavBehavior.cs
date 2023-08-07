using UnityEngine;
using UnityEngine.UI;

public class BottomNavBehavior : MonoBehaviour
{
    private BottomNavController bottomNav;
    private Image constantPanel;
    private Image overlayPanel;
    [SerializeField] private float pullMenuScreenMaxHeight = 0.34f;
    private Vector3 dockPosition;
    void Start()
    {
        bottomNav = GetComponent<BottomNavController>();
        constantPanel = bottomNav.transform.Find("ConstantPanel").GetComponent<Image>();
        overlayPanel = bottomNav.transform.Find("OverlayPanel").GetComponent<Image>();
        
        dockPosition = bottomNav.transform.position;
    }

    void Update()
    {
        if (bottomNav.IsSliderHold)
        {
            bottomNav.transform.position = BottomMenuPositionHandler();
        }
        else
        {
            AutoDestinationPull();
        }
        ConstantPanelVisibilityHandler();
    }

    private Vector3 BottomMenuPositionHandler()
    {
        var menuPosition = Vector3.up ;
        menuPosition.y *= Input.mousePosition.y - (Screen.height * 0.105f - dockPosition.y);
        menuPosition.x = dockPosition.x;

        if (menuPosition.y > Screen.height * pullMenuScreenMaxHeight)
        {
            menuPosition.y = Screen.height * pullMenuScreenMaxHeight;
        }

        if (menuPosition.y < dockPosition.y)
        {
            menuPosition.y = dockPosition.y;
        }
        return menuPosition;
    }

    private void AutoDestinationPull()
    {
        if (!bottomNav.transform.position.y.Equals(dockPosition.y))
        {
            Vector3 translation;
            if (transform.position.y > (Screen.height * pullMenuScreenMaxHeight + dockPosition.y) / 2)
            {
                translation = Vector3.up * (Time.deltaTime * bottomNav.transformFactor);
            }
            else
            {
                translation = Vector3.down * (Time.deltaTime * bottomNav.transformFactor);
            }
            
            var newPosition = bottomNav.transform.position + translation;
            if (newPosition.y > Screen.height * pullMenuScreenMaxHeight)
            {
                translation.y = 0;
            }
            
            if (newPosition.y < dockPosition.y)
            {
                translation.y = 0;
            }
                    
            bottomNav.transform.Translate(translation);
        }
    }

    private void ConstantPanelVisibilityHandler()
    {
        if (bottomNav.transform.position.y > (Screen.height * pullMenuScreenMaxHeight + dockPosition.y) / 2)
        {
            constantPanel.gameObject.SetActive(false);
            overlayPanel.gameObject.SetActive(true);
        }
        else
        {
            constantPanel.gameObject.SetActive(true);
            overlayPanel.gameObject.SetActive(false);
        }
    }
}
