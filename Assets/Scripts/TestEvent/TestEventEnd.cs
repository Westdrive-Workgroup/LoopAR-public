using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEventEnd : MonoBehaviour
{
    public void TestEventFailed(bool failed)
    {
        if (failed)
        {
            CalibrationManager.Instance.TestDriveSuccessful();
        }
        else
        {
            CalibrationManager.Instance.TestDriveFailed();
        }
    }
    
}
