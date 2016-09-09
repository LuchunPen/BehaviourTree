/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 02.08.2016 17:51:47
*/

using System;

namespace Nano3.Engine.Brain
{ 
    public class BTConditionBoolean : BTCondition
    {
        private Func<bool> _condition;

        public BTConditionBoolean(string name, BehaviourTree bt, Func<bool> condition) : base(name, bt)
        {
            if (condition == null) { throw new ArgumentNullException("Condition is null"); }
            _condition = condition;
        }

        public override ExecutionStatus Execute(double time)
        {
            if (_childNode == null) {
                CurrentStatus = ExecutionStatus.Error;
            }
            else {
                if (_condition()) { CurrentStatus = _childNode.Execute(time); }
                else { CurrentStatus = ExecutionStatus.Failure; }

                if (CurrentStatus == ExecutionStatus.Error) {
                    ErrorEventTrigger("Condition internal error");
                }
            }

            if (DebugMode) {
                //Debug impl
                //AppLogger.Log(this.Name + ": " + CurrentStatus);
            }
            return CurrentStatus;
        }

        public override bool CheckNodeToError()
        {
            bool result = true;

            if (_condition == null){
                ErrorEventTrigger("condition not realized");
                result = false;
            }
            if (_childNode == null){
                ErrorEventTrigger("no child node");
                result = false;
            }
            return result;
        }
    }
}
