using System;
using System.Collections.Generic;
using System.Text;

namespace Amido.Stacks.E2e.Tests.Api
{
    public interface IBuilder<T>
    {
        T Build();
    }
}
