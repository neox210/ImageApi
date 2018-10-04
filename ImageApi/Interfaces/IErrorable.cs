using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImageApi.Interfaces
{
    public interface IErrorable
    {
        IList<string> ErrorList { get; set; }
    }
}
