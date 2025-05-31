using UnityEngine;

public class Main : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ViewController.GetViewController(typeof(RoleListViewController), (ViewController view) => { view.show(); });
        ViewController.GetViewController(typeof(RoleContentViewController), (ViewController view) => { view.show(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
