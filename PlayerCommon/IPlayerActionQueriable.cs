using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace PlayerCommon
{
    //-------------------------------------------------------------------------------------------------------------------
    // interface IPlayerActionQueriable
    //-------------------------------------------------------------------------------------------------------------------
    public interface IPlayerActionQueriable
    {
        event Action PlayQuery;
        event Action PauseQuery;
        event Action StopQuery;
        event Action<object, object> ListChangeQuery;
        event Action<object> FolderDialogQuery;
        void EndAction(bool isFailed);
        int TrackByList(object pList);
        void OnLoad();
    }
    //-------------------------------------------------------------------------------------------------------------------
}
