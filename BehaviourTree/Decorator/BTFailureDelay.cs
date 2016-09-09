/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 24.08.2016 4:41:52
*/

using System;

namespace Nano3.Engine.Brain
{
    /// <summary>
    /// This class run delay counter if child node is [Failure] and return [Running].
    /// If time expected, then called FullFailureaction method
    /// </summary>
    public class BTFailureDelay : BTCondition
    {
        private Random rand;

        private double _delayMin;
        private double _delayMax;

        private double _curDelay;

        private double _startDelay;
        private double _currentDelay;
        public double CurrentDelayValue { get { return _currentDelay; } }

        public BTFailureDelay(string name, BehaviourTree bt, double delayMin, double delayMax) 
            : base(name, bt) 
        {
            if (delayMin > delayMax) {
                double d = delayMax; delayMax = delayMin; delayMin = d;
            }
            if (delayMin < 0) { delayMin = 0; }
            if (delayMax < 0) { delayMax = 0; }
            _delayMin = delayMin;
            _delayMax = delayMax;

            rand = new Random();
            _curDelay = rand.Next((int)_delayMin, (int)_delayMax);
        }

        public override ExecutionStatus Execute(double time)
        {
            if (_childNode == null) {
                CurrentStatus = ExecutionStatus.Error;
            }
            else {
                CurrentStatus = _childNode.Execute(time);
                if (CurrentStatus == ExecutionStatus.Failure) {
                    if (_startDelay == 0) { _startDelay = time; }
                    _currentDelay = time - _startDelay;
                    if (_currentDelay < _curDelay) {
                        CurrentStatus = ExecutionStatus.Running;
                    }
                    else {
                        CurrentStatus = ExecutionStatus.Failure;
                        _curDelay = rand.Next((int)_delayMin, (int)_delayMax);
                        _startDelay = 0;
                        _currentDelay = 0;
                    }
                }
                else {
                    _startDelay = 0;
                    _currentDelay = 0;
                }

                if (CurrentStatus == ExecutionStatus.Error) {
                    ErrorEventTrigger("Invertor internal error");
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
