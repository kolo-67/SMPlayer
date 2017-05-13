using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayerCommon;
using SMPlayerModel;

namespace SMPlayerViewModel
{
    //----------------------------------------------------------------------------------------------------------------------
    // class TrackInfoViewModel
    //----------------------------------------------------------------------------------------------------------------------    
    public class TrackInfoViewModel : ViewModelBase
    {
        private TrackInfo trackInfo;
        //----------------------------------------------------------------------------------------------------------------------    
        public TrackInfo Track
        {
            get { return trackInfo; }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public string FileName
        {
            get { return trackInfo.FileName; }
            set
            {
                trackInfo.FileName = value;
                OnPropertyChanged("FileName");
                OnPropertyChanged("FullName");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public string DirectoryName
        {
            get { return trackInfo.DirectoryName; }
            set
            {
                trackInfo.DirectoryName = value;
                OnPropertyChanged("DirectoryName");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public string FullDirectoryName
        {
            get { return trackInfo.FullDirectoryName; }
            set
            {
                trackInfo.FullDirectoryName = value;
                OnPropertyChanged("FullDirectoryName");
                OnPropertyChanged("FullName");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public string FullName
        {
            get
            {
                return trackInfo.FullDirectoryName + "\\" + trackInfo.FileName;
            }
        }

        //----------------------------------------------------------------------------------------------------------------------    
        public bool IsFailed
        {
            get { return trackInfo.IsFailed; }
            set
            {
                trackInfo.IsFailed = value;
                OnPropertyChanged("IsFailed");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public double Position
        {
            get { return trackInfo.Position; }
            set
            {
                trackInfo.Position = value;
                OnPropertyChanged("Position");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public double MaxPosition
        {
            get { return trackInfo.MaxPosition; }
            set
            {
                trackInfo.MaxPosition = value;
                OnPropertyChanged("MaxPosition");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public TrackInfoViewModel(TrackInfo pTrackInfo)
        {
            trackInfo = pTrackInfo;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public override string ToString()
        {
            return FileName + " / " + Position.ToString() + (IsFailed ? " Failed" : "");
        }

        //----------------------------------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------------------------------    
}
