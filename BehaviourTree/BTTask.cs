/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01.08.2016 23:43:14
*/

using System;

namespace Nano3.Engine.Brain
{
    public class BTTask : BTNode
    {
        private Func<ExecutionStatus> _executor;

        public BTTask(string name, BehaviourTree bt, Func<ExecutionStatus> executor) : base(name, bt)
        {
            if (executor == null) { throw new ArgumentNullException("Act function is null"); }
            _executor = executor;
        }

        public override ExecutionStatus Execute(double time)
        {
            try { CurrentStatus = _executor(); }
            catch (Exception ex){
                CurrentStatus = ExecutionStatus.Error;
                //Debug impl
                //AppLogger.Log(ex, LogType.Exception);
            }
            if (CurrentStatus == ExecutionStatus.Error){
                ErrorEventTrigger("Execution internal error");
            }

            if (DebugMode) {
                //Debug impl
                //AppLogger.Log(this.Name + ": " + CurrentStatus);
            }
            return CurrentStatus;
        }

        public override bool CheckNodeToError()
        {
            if (_executor == null){
                ErrorEventTrigger("execution action is not realized");
                return false;
            }
            return true;
        }
    }
}
