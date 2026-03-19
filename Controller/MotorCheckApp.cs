using System;

namespace Controller
{
    public sealed class MotorCheckApp : ControllerBase
    {
        public MotorCheckApp(string name) :
            base(name) { }

        public float PreDownPosition;
        public float DownPosition;
        public float PreUpPosition;
        public float UpPosition;
        public float Interval;

        public DateTime MoveDownStartTime;
        public DateTime MoveDownEndTime;
        public DateTime MoveUpStartTime;
        public DateTime MoveUpEndTime;
        public float MoveUpSpeed;
        public float MoveDownSpeed;
        public float MoveCurrent;
        public float StartPos;
        public float EndPos;

        public float TempFloat1;
        public float TempFloat2;
        public float TempFloat3;

        public void SetMoveDownStartTime()
        {
            MoveDownStartTime = DateTime.Now;
        }

        public void SetMoveDownEndTime()
        {
            MoveDownEndTime = DateTime.Now;
        }

        public void SetMoveUpStartTime()
        {
            MoveUpStartTime = DateTime.Now;
        }

        public void SetMoveUpEndTime()
        {
            MoveUpEndTime = DateTime.Now;
        }

        public void ComputeInterval()
        {
            Interval = PreUpPosition - DownPosition;
        }

        public void ComputeMovwDownSpeed()
        {
            var startPos = PreDownPosition;
            var endPosition = DownPosition;

            var distance = endPosition - startPos;
            if (distance < 0)
                distance = 0 - distance;

            MoveDownSpeed = 
                distance / (float)(MoveDownEndTime - MoveDownStartTime).TotalMilliseconds;
        }

        public void ComputeMovwUpSpeed()
        {
            var startPos = PreUpPosition;
            var endPosition = UpPosition;

            var distance = endPosition - startPos;
            if (distance < 0)
                distance = 0 - distance;

            MoveUpSpeed = 
                distance / (float)(MoveUpEndTime - MoveUpStartTime).TotalMilliseconds;
        }

        public void TempSetOk()
        {
            DownPosition = 5.5f;
            UpPosition = 5.5f;
            TempFloat1 = 0.05f;
            TempFloat2 = 100f;
            TempFloat3 = 0.7f;
        }
    }
}
