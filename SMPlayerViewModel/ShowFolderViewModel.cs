using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PlayerCommon.ViewModel;
using SMPlayerModel;

namespace SMPlayerViewModel
{
    //----------------------------------------------------------------------------------------------------------------------
    // class ShowFolderViewModel
    //----------------------------------------------------------------------------------------------------------------------    
    public class ShowFolderViewModel : ViewModelBase
    {
        private ComputerElementViewModel computerFiles;
        private bool isAccept;
        private ObservableCollection<DirectoryElementViewModel> selectedDirectories;
        //----------------------------------------------------------------------------------------------------------------------    
        public ComputerElementViewModel ComputerFiles
        {
            get { return computerFiles; }
            set
            {
                computerFiles = value;
                OnPropertyChanged("ComputerFiles");
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
        public ShowFolderViewModel()
        {
            ComputerFiles = new ComputerElementViewModel();
            computerFiles.FindChields();
            computerFiles.FindChieldChields();
            selectedDirectories = new ObservableCollection<DirectoryElementViewModel>();
        }
        //----------------------------------------------------------------------------------------------------------------------   
        public ObservableCollection<DirectoryElementViewModel> GetSelectetedFolders()
        {
            return selectedDirectories;
        }
        //-------------------------------------------------------------------------------------------------------------------
        private ICommand expandedCommand;
        //-------------------------------------------------------------------------------------------------------------------
        public ICommand ExpandedCommand
        {
            get
            {
                if (expandedCommand == null)
                {
                    expandedCommand = new DelegateCommand<object>(ExpandedAction, CanExpandedAction);
                }
                return expandedCommand;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void ExpandedAction(object pObject)
        {
            FileSystemElementViewModel fsElement = pObject as FileSystemElementViewModel;
            if (fsElement != null)
                fsElement.FindChieldChields();
        }
        //-------------------------------------------------------------------------------------------------------------------
        private bool CanExpandedAction(object pObject)
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------    
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
            ComputerFiles.GetSelectetedFolders(selectedDirectories);
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
    //----------------------------------------------------------------------------------------------------------------------    
}
