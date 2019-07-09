using System;
using System.Collections.Generic;
using System.Text;

namespace xxAMIDOxx.xxSTACKSxx.Tests.Api.Builders
{
    public interface IBuilder<T>
    {
        T Build();
    }
}
