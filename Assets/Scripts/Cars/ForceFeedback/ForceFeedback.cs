using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;


namespace ForceFeedback
{
    public static class FFB 
    {
        // Start is called before the first frame update
        [DllImport("user32.dll")]
        public static extern IntPtr GetActiveWindow();
        [DllImport("LibWestDriveFFB.dll")]
        public static extern int InitDirectInput(IntPtr hDlg);
        [DllImport("LibWestDriveFFB.dll")]
        public static extern void FreeDirectInput();
        [DllImport("LibWestDriveFFB.dll")]
        private static extern void StopEffect();
        [DllImport("LibWestDriveFFB.dll")]
        public static extern void RunEffect();
        [DllImport("LibWestDriveFFB.dll")]
        private static extern int SetDeviceForcesXY(int x_Force, int y_Force);
        [DllImport("LibWestDriveFFB.dll")]
        public static extern bool AcquireDevice();
        
       

        public static void ForceFeedBackInit()
        {
            FFB.InitDirectInput(GetActiveWindow());
        }
        public static void SetDeviceForceFeedback(int xForce, int yForce)
        {
            FFB.SetDeviceForcesXY(xForce, yForce);
            FFB.RunEffect();
        }

        public static void StopForceFeedback()
        {
            FFB.StopEffect();
        }

        public static void Release()
        {
            StopForceFeedback();
            FFB.FreeDirectInput();
        }
        
        
    }
}

