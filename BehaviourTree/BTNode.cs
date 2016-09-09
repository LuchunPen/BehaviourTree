/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01.08.2016 20:54:11
*/

using System;
using System.Threading;

namespace Nano3.Engine.Brain
{
    public abstract class BTNode : IBTNode
    {
        private static long NODE_ID;

        public event EventHandlerArg<string> NodeErrorEvent;
        private BehaviourTree _bt;

        private int _exStatus;
        public ExecutionStatus CurrentStatus
        {
            get { return (ExecutionStatus)_exStatus; }
            protected set { Interlocked.Exchange(ref _exStatus, (int)value); }
        }

        private long _nodeID;
        public long UniqueID
        {
            get { return _nodeID; }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private bool _debugMode;
        public bool DebugMode
        {
            get { return _debugMode; }
            set { _debugMode = value; }
        }

        public BTNode(string name, BehaviourTree bt)
        {
            if (bt == null) { throw new ArgumentNullException("Behaviour tree is null"); }
            _nodeID = NODE_ID++;
            _name = name;
            _bt = bt;
            bt.RegisterNode(this);
        }

        public abstract ExecutionStatus Execute(double time);
        public abstract bool CheckNodeToError();

        public void Reset(object sender, EventArgs arg)
        {
            CurrentStatus = ExecutionStatus.None;
        }

        protected void ErrorEventTrigger(string errorMessage){
            NodeErrorEvent?.Invoke(this, errorMessage);
        }

        public override int GetHashCode()
        {
            return UniqueID.GetHashCode();
        }

        public override string ToString()
        {
            return Name + ", [" + UniqueID + "]";
        }
    }
}
