using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    private static Camera _mainCam = null;

    public static Camera MainCam
    {
        get
        {
            if (_mainCam == null)
            {
                _mainCam = Camera.main;
                UnityEngine.Debug.Log(_mainCam);
            }
            return _mainCam;
        }
    }
}
