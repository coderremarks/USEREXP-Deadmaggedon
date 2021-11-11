using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MenuType
{
    TitleScreen,
    HUD,
    GameOver,
    DayActions,
    Build,
    Universal,
    Tutorial,
    Pause,
    Transition,
}

public class MenuCanvas : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] MenuType menuType;
    public MenuType MenuType
    {
        get { return menuType; }
    }


    protected virtual void Start()
    {
       
        MenuManager.instance.RegisterMenu(this);
        Initialize();
    }

    public virtual void Preinitialize()
    {

    }
    public virtual void Initialize()
    {

    }

    public void Show()
    {
        if (!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        if (this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(false);
        }
    }
}
