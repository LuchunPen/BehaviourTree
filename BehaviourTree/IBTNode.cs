/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 01.08.2016 18:30:26
*/

using System;

namespace Nano3.Engine.Brain
{
    public interface IBTNode
    {
        long UniqueID { get; }
        event EventHandlerArg<string> NodeErrorEvent;
        string Name { get; set; }
        bool DebugMode { get; set; }

        ExecutionStatus CurrentStatus { get; }

        ExecutionStatus Execute(double time);
        void Reset(object sender, EventArgs arg);
        bool CheckNodeToError();
    }

    public interface IBTComposite
    {
        IBTNode[] GetChilds();
        void AddChildNode(params IBTNode[] nodes);
        bool RemoveChildNode(IBTNode node);
    }
}
