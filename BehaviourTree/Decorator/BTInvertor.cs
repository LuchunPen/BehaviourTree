/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01.08.2016 23:23:21
*/

using System;

namespace Nano3.Engine.Brain
{
    public class BTInvertorNode : BTCondition, IBTComposite
    {
        public BTInvertorNode(string name, BehaviourTree bt) : base(name, bt) { }

        public override ExecutionStatus Execute(double time)
        {
            if (_childNode == null) {
                CurrentStatus = ExecutionStatus.Error;
            }
            else {
                CurrentStatus = ExecutionStatus.Running;

                ExecutionStatus nStatus = _childNode.Execute(time);
                if (nStatus == ExecutionStatus.Failure) { CurrentStatus = ExecutionStatus.Success; }
                else if (nStatus == ExecutionStatus.Success) { CurrentStatus = ExecutionStatus.Failure; }
                else { CurrentStatus = nStatus; }

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
