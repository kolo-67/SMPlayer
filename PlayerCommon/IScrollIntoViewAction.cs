using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlayerCommon
{
    public interface IScrollIntoViewAction
    {
        event Action<object> MainGridScrollIntoView;
    }
}
