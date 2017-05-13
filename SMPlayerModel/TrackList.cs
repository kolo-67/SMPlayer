using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMPlayerModel
{
    //----------------------------------------------------------------------------------------------------------------------
    // class TrackList
    //----------------------------------------------------------------------------------------------------------------------    
    [Serializable]
    public class TrackList
    {
        private List<TrackInfo> traks = new List<TrackInfo>();
        private TrackInfo currentTrack;
        private int selectedIndex;
        private string listName;
        //----------------------------------------------------------------------------------------------------------------------    
        public string ListName
        {
            get { return listName; }
            set { listName = value; }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public List<TrackInfo> Traks
        {
            get { return traks; }
            set { traks = value; }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public TrackInfo CurrentTrack
        {
            get { return currentTrack; }
            set { currentTrack = value; }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set { selectedIndex = value; }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public override string ToString()
        {
            return ListName;
        }
        //----------------------------------------------------------------------------------------------------------------------    
    }
    //----------------------------------------------------------------------------------------------------------------------    
}
