/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01.08.2016 21:26:32
*/

using System;
using Nano3.Collection;

namespace Nano3.Engine.Brain
{
    public class BehaviourTree : IBTComposite
    {
        private event EventHandler ResetEvent;
        public event EventHandlerArg<string> BehaviourTreeMessager;

        public FastDictionaryM2<long, IBTNode> _treeNodes;
        
        private bool _isRun;
        public bool IsRun { get { return _isRun; } }

        private string _btName;
        public string Name
        {
            get { return _btName; }
            set { _btName = value; }
        }

        private double _lastUpdate;
        private double _updInterval = 16;

        private bool _debug = false;
        public bool DebugMode
        {
            get { return _debug; }
            set {
                if ((value && !_debug) || (!value && _debug)) {
                    IBTNode[] nodes = _treeNodes.GetValues();
                    for (int i = 0; i < nodes.Length; i++) {
                        nodes[i].DebugMode = value;
                    }
                    _debug = value;
                }
            }
        }

        /// <summary>
        /// interval in milliseconds
        /// </summary>
        public double Interval
        {
            get { return _updInterval; }
            set
            {
                if (value > 0) { _updInterval = value; }
            }
        }

        private BTSelectorNode _rootSelector;
        public BehaviourTree()
        {
            _treeNodes = new FastDictionaryM2<long, IBTNode>();
            _rootSelector = new BTSelectorNode("Root", this);
        }

        public void Start()
        {
            _isRun = true;
        }
        public void Stop()
        {
            _isRun = false;
        }
        public void Update(double time)
        {
            if (!IsRun) return;

            if (_updInterval + _lastUpdate < time)
            {
                _lastUpdate = time;

                ResetEvent?.Invoke(this, null);
                _rootSelector.Execute(time);
            }
        }

        public void RegisterNode(IBTNode node)
        {
            if (node != null)
            {
                ResetEvent -= node.Reset;
                ResetEvent += node.Reset;

                node.NodeErrorEvent -= OnNodeErrorMessageHandler;
                node.NodeErrorEvent += OnNodeErrorMessageHandler;
                
                _treeNodes[node.UniqueID] = node;
            }
        }
        public void UnregisterNode(long nodeID)
        {
            IBTNode node = null;
            _treeNodes.TryGetAndRemove(nodeID, out node);
            if (node != null)
            {
                node.NodeErrorEvent -= OnNodeErrorMessageHandler;
                this.ResetEvent -= node.Reset;

                IBTNode[] allNodes = _treeNodes.GetValues();
                for (int i = 0; i < allNodes.Length; i++)
                {
                    IBTComposite cNode = allNodes[i] as IBTComposite;
                    if (cNode != null)
                    {
                        bool removed = cNode.RemoveChildNode(node);
                        if (removed) { return; }
                    }
                }
            }
        }

        public IBTNode[] GetChilds()
        {
            return _rootSelector.GetChilds();
        }
        public void AddChildNode(params IBTNode[] nodes)
        {
            _rootSelector.AddChildNode(nodes);
        }
        public bool RemoveChildNode(IBTNode node)
        {
            return _rootSelector.RemoveChildNode(node);
        }
        public bool TestAllNodesToError()
        {
            IBTNode[] nodes = _treeNodes.GetValues();
            bool noError = true;
            for (int i = 0; i < nodes.Length; i++)
            {
                bool result = nodes[i].CheckNodeToError();
                if (result)
                {
                    string m = "Node: " + nodes[i].ToString() + " STATUS OK";
                    //Your debug impl
                    //AppLogger.Log(m, LogType.Log);
                }
                else { noError = false; }
            }
            return noError;
        }

        public void OnNodeErrorMessageHandler(object sender, string errorMessage)
        {
            BehaviourTreeMessager?.Invoke(sender, errorMessage);
            this.Stop();
        }
        public void Clear()
        {
            Stop();
            ResetEvent?.Invoke(this, null);

            IBTNode[] allNodes = _treeNodes.GetValues();
            for (int i = 0; i < allNodes.Length; i++)
            {
                IBTNode cNode = allNodes[i];
                if (cNode != null)
                {
                    cNode.NodeErrorEvent -= OnNodeErrorMessageHandler;
                    this.ResetEvent -= cNode.Reset;
                }
            }
            _treeNodes.Clear();
        }
    }
}
