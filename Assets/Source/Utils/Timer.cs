using System;

namespace Dasher
{
    public class Timer
    {
        private float currentTime;
        private readonly float time;

        public bool IsActive { get; private set; }

        public event Action<bool> OnActivityChanged;

        public Timer(float time)
        {
            this.time = time;
        }

        public void Start()
        {
            IsActive = true;
            OnActivityChanged?.Invoke(IsActive);
        }

        public void Service(float passedTime)
        {
            if (!IsActive)
                return;

            currentTime += passedTime;
            if (currentTime >= time)
            {
                IsActive = false;
                OnActivityChanged?.Invoke(IsActive);
                currentTime = 0;
            }
        }
    }
}