using UnityEngine;
using System.Collections;

/* USES:
 * ==============
 * Menu.cs
 * ==============
 * 
 * USAGE:
 * ======================================
 * Used to call method to open menus through 
 * UI buttons. UI button calls the method and 
 * the script sets the animator trigger bools
 * ======================================
 * 
 * Date Created: 21 Aug 2015
 * Last Modified: 21 Aug 2015
 * Authors: Francisco Carrera
 */

public class MenuManager : MonoBehaviour {

	public Menu currentMenu;

	private int clickableMask; 	// A layer mask so the raycast only hits UI elements 


	void Awake (){
		clickableMask = LayerMask.GetMask("UI");
	}

	void Start () {
		// Open Starting Menu
		ShowMenu(currentMenu);
	}

	// used to open new menus. Called by buttons
	public void ShowMenu (Menu menu){
		// Close any current menus by setting IsOpen to false
		if (currentMenu != null) currentMenu.IsOpen = false;

		// Set new menu and set IsOpen to true
		currentMenu = menu;
		currentMenu.IsOpen = true;
	}
}
