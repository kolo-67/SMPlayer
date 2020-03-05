using PlayerCommon.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SMPlayerViewModel
{
    public class ChangePathViewModel : ViewModelBase
    {
        private bool isAccept;
        private string pathFrom;
        private string pathTo;
        //----------------------------------------------------------------------------------------------------------------------    
        public string PathFrom
        {
            get { return pathFrom; }
            set
            {
                pathFrom = value;
                OnPropertyChanged("PathFrom");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public string PathTo
        {
            get { return pathTo; }
            set
            {
                pathTo = value;
                OnPropertyChanged("PathTo");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public bool IsAccept
        {
            get { return isAccept; }
            set
            {
                isAccept = value;
                OnPropertyChanged("IsAccept");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public ChangePathViewModel()
        {
        }
        //-------------------------------------------------------------------------------------------------------------------
        private ICommand okCommand;
        //-------------------------------------------------------------------------------------------------------------------
        public ICommand OkCommand
        {
            get
            {
                if (okCommand == null)
                {
                    okCommand = new DelegateCommand(OkAction, CanOkAction);
                }
                return okCommand;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void OkAction()
        {
            IsAccept = true;
        }
        //-------------------------------------------------------------------------------------------------------------------
        private bool CanOkAction()
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------------
        private ICommand cancelCommand;
        //-------------------------------------------------------------------------------------------------------------------
        public ICommand CancelCommand
        {
            get
            {
                if (cancelCommand == null)
                {
                    cancelCommand = new DelegateCommand(CancelAction, CanCancelAction);
                }
                return cancelCommand;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void CancelAction()
        {
            IsAccept = false;
        }
        //-------------------------------------------------------------------------------------------------------------------
        private bool CanCancelAction()
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------    
    }
}
