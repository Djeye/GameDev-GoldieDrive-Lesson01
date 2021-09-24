using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField]
    private string _name;
    private bool _isEntered = false;

    public string getName() {
        return _name;
    }

    public bool isEntered()
    {
        return _isEntered;
    }

    public void setEntered()
    {
        _isEntered = true;
    }
}
