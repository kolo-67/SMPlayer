using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMPlayerModel
{
    //----------------------------------------------------------------------------------------------------------------------
    // class TrackInfo
    //----------------------------------------------------------------------------------------------------------------------    
    [Serializable]
    public class TrackInfo
    {
        private string fileName;
        private string directoryName;
        private string fullDirectoryName;
        private double position;
        private double maxPosition;
        private bool isFailed;
        //----------------------------------------------------------------------------------------------------------------------    
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public string DirectoryName
        {
            get { return directoryName; }
            set { directoryName = value; }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public string FullDirectoryName
        {
            get { return fullDirectoryName; }
            set { fullDirectoryName = value; }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public double Position
        {
            get { return position; }
            set { position = value; }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public double MaxPosition
        {
            get { return maxPosition; }
            set { maxPosition = value; }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public bool IsFailed
        {
            get { return isFailed; }
            set { isFailed = value; }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public override string ToString()
        {
            return DirectoryName + "\\" + FileName + (IsFailed ? " Failed" : "");
        }
        //----------------------------------------------------------------------------------------------------------------------    
    }
    //----------------------------------------------------------------------------------------------------------------------    
}
