using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMPlayerModel;

namespace SMPlayerViewModel
{
    //----------------------------------------------------------------------------------------------------------------------
    // class FileSystemElementViewModel
    //----------------------------------------------------------------------------------------------------------------------    
    public abstract class FileSystemElementViewModel : ViewModelBase
    {
        protected ObservableCollection<FileSystemElementViewModel> chields = new ObservableCollection<FileSystemElementViewModel>();
        protected bool isSelected;
        protected bool isExpanded;
        protected bool isAlreadyExpanded = false;
        //----------------------------------------------------------------------------------------------------------------------    
        public ObservableCollection<FileSystemElementViewModel> Chields
        {
            get { return chields; }
            set
            {
                chields = value;
                OnPropertyChanged("Chields");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                isExpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------   
        public abstract string Name { get; }
        //----------------------------------------------------------------------------------------------------------------------   
        public abstract void FindChields();
        //----------------------------------------------------------------------------------------------------------------------   
        public abstract ObservableCollection<DirectoryElementViewModel> GetSelectetedFolders(ObservableCollection<DirectoryElementViewModel> pList);
        //----------------------------------------------------------------------------------------------------------------------   
        public  void FindChieldChields()
        {
            foreach (var c in Chields)
            {
                c.FindChields();
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public FileSystemElementViewModel()
        {
            PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == "IsExpanded" && !isAlreadyExpanded)
                {
                    FindChieldChields();
                    isAlreadyExpanded = true;
                }
            };
        }

        //----------------------------------------------------------------------------------------------------------------------    
    }
    //----------------------------------------------------------------------------------------------------------------------    
}
