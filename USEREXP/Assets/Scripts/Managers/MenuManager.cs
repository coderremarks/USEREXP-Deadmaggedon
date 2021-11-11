using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] public static MenuManager instance;
    [SerializeField] public List<MenuCanvas> menuCanvasList = new List<MenuCanvas>();
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Start()
    {
       
    }
    public void RegisterMenu(MenuCanvas menuCanvas)
    {
        int i = 0;

        while( i <= menuCanvasList.Count)
        {
            if (i >= menuCanvasList.Count)
            {
                menuCanvasList.Add(menuCanvas);
                break;
            }
            if (menuCanvasList[i] == menuCanvas)
            {
                Debug.Log("wtf why is this bloody not working");
                break;
            }
           
            
            i++;
         
        }
      
      
        if (menuCanvas.MenuType != MenuType.Universal && menuCanvas.MenuType != MenuType.Tutorial && menuCanvas.MenuType != MenuType.Transition)
        {
            menuCanvas.Hide();
        }
        

    }
    public void PreinitializeAllCanvas()
    {

        foreach (MenuCanvas currentMenuCanvas in menuCanvasList)
        {
            currentMenuCanvas.Preinitialize();
        }
    }
    public void InitializeAllCanvas()
    {
        
        foreach (MenuCanvas currentMenuCanvas in menuCanvasList)
        {
            currentMenuCanvas.Initialize();
        }
    }
    public void HideAll()
    {
        foreach (MenuCanvas menuCanvas in menuCanvasList)
        {
            if (menuCanvas.MenuType != MenuType.Universal && menuCanvas.MenuType != MenuType.Tutorial && menuCanvas.MenuType != MenuType.Pause && menuCanvas.MenuType != MenuType.Transition)
            {
                menuCanvas.Hide();
            }
        }
    }


    public void ShowCanvas(MenuType menuType)
    {
        if (menuType != MenuType.Pause)
        {
            HideAll();
        }
        

        foreach (MenuCanvas menuCanvas in menuCanvasList)
        {
            if (menuCanvas.MenuType == menuType)
            {
                menuCanvas.Show();
                
            }
         
        }
    }
    public GameObject GetCanvas(MenuType menuType)
    {

        foreach (MenuCanvas menuCanvas in menuCanvasList)
        {
            if (menuCanvas?.MenuType == menuType)
            {
                return menuCanvas.gameObject;
                
            }
        }
        return null;
    }
}
