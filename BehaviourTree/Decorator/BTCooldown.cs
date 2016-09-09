/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 02.08.2016 17:16:34
*/

using System;

namespace Nano3.Engine.Brain
{
    public class BTCooldown : BTCondition
    {
        private double _lastTime;
        private ExecutionStatus _coolDownStatus;

        private double _interval;
        public double Interval
        {
            get { return _interval; }
            set  
            { 
                if (value > 0) { _interval = value; }
            }
        }

        private double _currentTimerValue;
        public double CurrentTimerValue { get { return _currentTimerValue; } }

        public BTCooldown(string name, BehaviourTree bt, double interval, ExecutionStatus coolDownStatus = ExecutionStatus.Failure) 
            : base (name, bt)
        {
            Interval = interval;
            _coolDownStatus = coolDownStatus;
        }

        public void ResetTimer()
        {
            _currentTimerValue = 0;
        }

        public override ExecutionStatus Execute(double time)
        {
            if (_childNode == null) {
                CurrentStatus = ExecutionStatus.Error;
            }
            else {
                _currentTimerValue = time - _lastTime;

                if (_interval > _currentTimerValue) {
                    CurrentStatus = _coolDownStatus;
                }
                else {
                    CurrentStatus = _childNode.Execute(time);
                    if (CurrentStatus != ExecutionStatus.Running) {
                        _lastTime = time;
                    }
                }
                if (CurrentStatus == ExecutionStatus.Error) {
                    ErrorEventTrigger("Cooldown internal error");
                }
            }
            if (DebugMode) {
                //Debug impl
                //AppLogger.Log(this.Name + ": " + CurrentStatus);
            }
            return CurrentStatus;
        }
    }
}
