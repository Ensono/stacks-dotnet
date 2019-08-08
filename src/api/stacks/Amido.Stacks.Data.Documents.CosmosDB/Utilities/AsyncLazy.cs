﻿using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Amido.Stacks.Core.Utilities
{
    /// The class AsyncLazy<T> is planned to be part of the CoreFx in the future
    /// Internal class for use by CosmosDB only, until available by CoreFX
    /// https://github.com/dotnet/corefx/issues/32552
    /// 
    /// source: https://devblogs.microsoft.com/pfxteam/asynclazyt/
    internal class AsyncLazy<T> : Lazy<Task<T>>
    {
        public AsyncLazy(Func<Task<T>> taskFactory) :
            base(() => Task.Factory.StartNew(() => taskFactory()).Unwrap())
        { }

        public TaskAwaiter<T> GetAwaiter() { return Value.GetAwaiter(); }
    }
}
