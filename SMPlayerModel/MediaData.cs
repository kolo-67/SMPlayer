using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMPlayerModel
{
    //----------------------------------------------------------------------------------------------------------------------
    // class MediaData
    //----------------------------------------------------------------------------------------------------------------------  
    [Serializable]
    public class MediaData
    {
        private List<TrackList> trakLists= new List<TrackList>();
        private TrackList currentTrack;
        private double volumeSound;
        //----------------------------------------------------------------------------------------------------------------------    
        public List<TrackList> TrakLists
        {
            get { return trakLists; }
            set { trakLists = value; }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public TrackList CurrentTrackList
        {
            get { return currentTrack; }
            set { currentTrack = value; }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public double VolumeSound 
        {
            get { return volumeSound; }
            set { volumeSound = value; }
        }
        //----------------------------------------------------------------------------------------------------------------------    
    }
    //----------------------------------------------------------------------------------------------------------------------    
}
