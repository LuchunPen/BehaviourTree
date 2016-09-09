/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 02.08.2016 17:06:31
*/

using System;

namespace Nano3.Engine.Brain
{
    public abstract class BTCondition : BTNode, IBTComposite
    {
        protected IBTNode _childNode;

        public BTCondition(string name, BehaviourTree bt) : base(name, bt) { }

        public void AddChildNode(params IBTNode[] nodes)
        {
            if (nodes != null && nodes.Length == 0) return;
            if (nodes[0] == null || nodes[0] == this) return;

            _childNode = nodes[0];
        }

        public IBTNode[] GetChilds()
        {
            return new IBTNode[1] { _childNode };
        }
        public bool RemoveChildNode(IBTNode node)
        {
            if (_childNode != null && _childNode == node)
            {
                _childNode = null;
                return true;
            }
            return false;
        }

        public override bool CheckNodeToError()
        {
            bool result = true;
            if (_childNode == null) {
                ErrorEventTrigger("no child node");
                result = false;
            }
            return result;
        }
    }
}
