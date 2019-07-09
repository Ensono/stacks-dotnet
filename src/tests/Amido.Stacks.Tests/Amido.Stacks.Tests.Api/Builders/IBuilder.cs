using System;
using System.Collections.Generic;
using System.Text;

namespace Amido.Stacks.Tests.Api
{
    public interface IBuilder<T>
    {
        T Build();
    }
}
