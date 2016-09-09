/*
Copyright (c) Luchunpen (bwolf88).  All rights reserved.
Date: 25/05/2016 13:00
*/
using System;

namespace System
{
    [Serializable] [System.Runtime.InteropServices.ComVisible(true)]
    public delegate void EventHandlerArg<TValue>(object sender, TValue arg);

    [Serializable][System.Runtime.InteropServices.ComVisible(true)]
    public delegate void EventHandlerArg<TValueID, TValueArg>(TValueID senderID, TValueArg arg);
}

