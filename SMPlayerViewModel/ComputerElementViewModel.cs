using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMPlayerModel;

namespace SMPlayerViewModel
{
    //----------------------------------------------------------------------------------------------------------------------
    // class ComputerElementViewModel
    //----------------------------------------------------------------------------------------------------------------------    
    public class ComputerElementViewModel : FileSystemElementViewModel
    {
        //----------------------------------------------------------------------------------------------------------------------   
        public override string Name
        {
            get { return "Computer"; }
        }
        //----------------------------------------------------------------------------------------------------------------------   
        public override void FindChields()
        {
            foreach (DriveInfo drive in DriveInfo.GetDrives())
            {
                DiskElementViewModel item = new DiskElementViewModel(drive);
                Chields.Add(item);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------   
        public override ObservableCollection<DirectoryElementViewModel> GetSelectetedFolders(ObservableCollection<DirectoryElementViewModel> pList)
        {
            foreach (var c in Chields)
            {
                c.GetSelectetedFolders(pList);
            }
            return pList;
        }
        //----------------------------------------------------------------------------------------------------------------------   
    }
    //----------------------------------------------------------------------------------------------------------------------   
}
