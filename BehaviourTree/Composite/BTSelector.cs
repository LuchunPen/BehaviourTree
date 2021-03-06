﻿/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01.08.2016 19:59:33
*/

using System;
using System.Collections.Generic;

namespace Nano3.Engine.Brain
{
    public class BTSelectorNode : BTNode, IBTComposite
    {
        protected List<IBTNode> _nodes = new List<IBTNode>();

        public BTSelectorNode(string name, BehaviourTree bt) : base(name, bt) { }

        public override ExecutionStatus Execute(double time)
        {
            if (_nodes.Count == 0) { CurrentStatus = ExecutionStatus.Failure; }
            else
            {
                CurrentStatus = ExecutionStatus.Running;
                for (int i = 0; i < _nodes.Count; i++)
                {
                    if (_nodes[i] == null) {
                        CurrentStatus = ExecutionStatus.Error;
                        return CurrentStatus;
                    }

                    ExecutionStatus nStatus = _nodes[i].Execute(time);
                    if (nStatus != ExecutionStatus.Failure){
                        CurrentStatus = nStatus;
                        return CurrentStatus;
                    }
                    if (nStatus == ExecutionStatus.Error){
                        ErrorEventTrigger("Selector internal error by node " + _nodes[i].Name);
                    }
                }
                CurrentStatus = ExecutionStatus.Failure;
            }
            return CurrentStatus;
        }

        public override bool CheckNodeToError()
        {
            bool result = true;
            if (_nodes.Count == 0) {
                ErrorEventTrigger("no child nodes");
                result = false;
            }
            for (int i = 0; i < _nodes.Count; i++)
            {
                if (_nodes[i] == null) {
                    ErrorEventTrigger("child[" + i +"] is null");
                    result = false;
                }
            }
            return result;
        }

        public void AddChildNode(params IBTNode[] nodes)
        {
            if (nodes == null || nodes.Length == 0) return;
            for (int i = 0; i < nodes.Length; i++)
            {
                if (nodes[i] == null || nodes[i] == this) continue;
                _nodes.Add(nodes[i]);
            }
        }

        public IBTNode[] GetChilds()
        {
            return _nodes.ToArray();
        }

        public bool RemoveChildNode(IBTNode node)
        {
            if (node == null) return false;
            return _nodes.Remove(node);
        }
    }
}
