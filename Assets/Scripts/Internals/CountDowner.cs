using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace LoopAr.Internals
{
    public class CountDowner : MonoBehaviour
    {
        private float timeLeft;
        private int maximumTime;
        private bool countDownRun;


        private void Update()
        {
            Debug.Log("CountDown!" + countDownRun);
            if (countDownRun)
            {
                timeLeft -= Time.deltaTime;
            }
        }

        public void SetMaximumTime(int maximumTime)
        {
            this.maximumTime = maximumTime;
            this.timeLeft = maximumTime;
        }

        public bool TimeErased()
        {
            Debug.Log("TimeLeft: " + timeLeft);
            return timeLeft < 0;
        }

        internal void SwitchCountDown()
        {
            countDownRun = !countDownRun;
        }

        public void SetSpecificCountDownStatus(bool countDownStatus)
        {
            countDownRun = countDownStatus;
        }

        public void ResetCoundDown()
        {
            timeLeft = maximumTime;
        }
    }
}