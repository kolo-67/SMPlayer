using System;
using System.Collections;
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
    // class TrackListViewModel
    //----------------------------------------------------------------------------------------------------------------------    
    public class TrackListViewModel : ViewModelBase
    {
        private ObservableCollection<TrackInfoViewModel> traks = new ObservableCollection<TrackInfoViewModel>();
        private TrackInfoViewModel currentTrack;
        private TrackList trackList;
        private IList selectedTracks;
        
        public event Action CurrentTrackChanged;
        //----------------------------------------------------------------------------------------------------------------------    
        public string ListName
        {
            get { return trackList.ListName; }
            set
            {
                trackList.ListName = value;
                OnPropertyChanged("ListName");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public TrackList TrackList
        {
            get { return trackList; }
            set
            {
                trackList = value;
                OnPropertyChanged("TrackList");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public ObservableCollection<TrackInfoViewModel> Traks
        {
            get { return traks; }
            set
            {
                traks = value;
                OnPropertyChanged("trakLists");
            }
        }
        ////----------------------------------------------------------------------------------------------------------------------    
        //public TrackInfoViewModel CurrentTrack
        //{
        //    get { return currentTrack; }
        //    set
        //    {
        //        currentTrack = value;
        //        OnPropertyChanged("CurrentTrack");
        //        if (currentTrack != null)
        //            TrackList.CurrentTrack = currentTrack.Track;

        //    }
        //}
        //----------------------------------------------------------------------------------------------------------------------    
        public TrackInfoViewModel SelectedTrack
        {
            get
            {
                if ( SelectedIndex >= 0 && SelectedIndex < traks.Count)
                    return traks[SelectedIndex];
                return null;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public IList SelectedTracks
        {
            get { return selectedTracks; }
            set
            {
                selectedTracks = value;
                OnPropertyChanged("SelectedTracks");
                if (SelectedTracks != null && SelectedTracks.Count > 0)
                    SelectedIndex = 0;
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public int SelectedIndex
        {
            get { return TrackList.SelectedIndex; }
            set
            {
                TrackList.SelectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public TrackListViewModel(TrackList pTrackList)
        {
            trackList = pTrackList;
            Traks.Clear();
            foreach (var t in trackList.Traks)
            {
                Traks.Add(new TrackInfoViewModel(t));
            }
            //if (TrackList.CurrentTrack != null)
            //    CurrentTrack = Traks.FirstOrDefault(t => object.ReferenceEquals(TrackList.CurrentTrack, t.Track));
            Traks.CollectionChanged += Traks_CollectionChanged;
            PropertyChanged += TrackListViewModel_PropertyChanged;
        }
        //----------------------------------------------------------------------------------------------------------------------    
        void TrackListViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentTrack")
                OnCurrentTrackChanged();
        }
        //----------------------------------------------------------------------------------------------------------------------    
        protected void OnCurrentTrackChanged()
        {
            Action handler = CurrentTrackChanged;
            if (handler != null)
                handler();
        }
        //---------------------------------------------------------------------------------------------------------------------    
        private void Traks_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (TrackInfoViewModel nt in e.NewItems)
                {
                    trackList.Traks.Add(nt.Track);
                }
            if (e.Action == NotifyCollectionChangedAction.Remove)
                foreach (TrackInfoViewModel ot in e.OldItems)
                {
                    trackList.Traks.Remove(ot.Track);
                }

        }
        //---------------------------------------------------------------------------------------------------------------------    
        public void AddTrackInfo(TrackInfo pTrackInfo)
        {
            Traks.Add(new TrackInfoViewModel(pTrackInfo));
        }
        //----------------------------------------------------------------------------------------------------------------------
        public override string ToString()
        {
            return ListName + ((SelectedTrack == null) ? "" : (" - " + SelectedTrack.FileName));
        }
        //---------------------------------------------------------------------------------------------------------------------    
        internal void DeleteCurrentTrack()
        {
            //TrackInfoViewModel nextTrack = null;
            //TrackInfoViewModel lastTrack = null;
            int oldIndex = SelectedIndex;
            if (SelectedTracks != null && SelectedTracks.Count > 0)
            {
//                lastTrack = SelectedTracks[SelectedTracks.Count - 1] as TrackInfoViewModel;
                while (SelectedTracks.Count > 0)
                {
                    Traks.Remove(SelectedTracks[0] as TrackInfoViewModel);
                }
            }
            else if (SelectedTrack != null)
            {
                Traks.Remove(SelectedTrack);
//                lastTrack = CurrentTrack;
            }
            //if (lastTrack != null)
            //{
            //    int idx = Traks.IndexOf(lastTrack);
            //    if (idx < Traks.Count - 1)
            //        nextTrack = Traks[idx + 1];
            //}
            //if (nextTrack == null && Traks.Count > 0)
            //{
            //    nextTrack = Traks[0];
            //}
            //SetCurrentTrack(nextTrack);
            if (oldIndex >= Traks.Count)
                oldIndex = Traks.Count;
            SelectedIndex = oldIndex;
        }
        //---------------------------------------------------------------------------------------------------------------------    
        internal void DeleteFailedTracks()
        {
            var failedTracks = Traks.Where(t => t.IsFailed).ToList();

            foreach (var track in failedTracks)
            {
                 Traks.Remove(track);
            }
        }
        //---------------------------------------------------------------------------------------------------------------------    
        internal void DeleteAllTracks()
        {
            var deletedTracks = Traks.ToList();

            foreach (var track in deletedTracks)
            {
                Traks.Remove(track);
            }
        }
        //---------------------------------------------------------------------------------------------------------------------    
        internal void GoToNextTrack()
        {
            //int idx = Traks.IndexOf(CurrentTrack);
            SelectedTrack.Position = 0;
            //if (idx < Traks.Count - 1)
            //{
            //    CurrentTrack = Traks[idx + 1];
            //}
            //else
            //{
            //    CurrentTrack = Traks[0];
            //}
            SelectedIndex = (SelectedIndex < Traks.Count - 1) ? SelectedIndex + 1 : 0;
//            SetCurrentTrack((idx < Traks.Count - 1) ? Traks[idx + 1] : Traks[0]);
        }
        ////----------------------------------------------------------------------------------------------------------------------    
        //internal void CurrentTrackGoToStart()
        //{
        //    if (CurrentTrack != null)
        //        CurrentTrack.Position = 0;
        //}
        //----------------------------------------------------------------------------------------------------------------------    
        //private void SetCurrentTrack(TrackInfoViewModel pTrack)
        //{
        //    SetSelection(pTrack);
        //    CurrentTrack = pTrack;
        //}
        //----------------------------------------------------------------------------------------------------------------------    
        public void SetSelection(TrackInfoViewModel pTrack)
        {
            if (SelectedTracks != null && SelectedTracks.Count > 0)
            {
                SelectedTracks.Clear();
                SelectedTracks.Add(pTrack);
            }
            //else
            //{
            //    SelectedTracks = new ObservableCollection<TrackInfoViewModel>();
            //    SelectedTracks.Add(pTrack);
            //}
        }
        //----------------------------------------------------------------------------------------------------------------------    
    }
    //----------------------------------------------------------------------------------------------------------------------    
}
