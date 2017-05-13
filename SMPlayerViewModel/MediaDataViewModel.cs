using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using SMPlayerModel;

namespace SMPlayerViewModel
{

    //----------------------------------------------------------------------------------------------------------------------
    // class MediaDataViewModel
    //----------------------------------------------------------------------------------------------------------------------    
    public class MediaDataViewModel : ViewModelBase
    {
        private ObservableCollection<TrackListViewModel> trakLists = new ObservableCollection<TrackListViewModel>();
        private TrackListViewModel currentList;
        private TrackInfoViewModel currentPlayTrack;
        private MediaData data;
        public event Action CurrentTrackChanged;
        public event Action<TrackListViewModel> CurrentListChanged;
        private CollectionViewSource trackCollectionViewSource;
        //----------------------------------------------------------------------------------------------------------------------
        public CollectionViewSource TrackCollectionViewSource
        {
            get
            {
                return trackCollectionViewSource;
            }
            set
            {
                trackCollectionViewSource = value;
                OnPropertyChanged("TrackCollectionViewSource");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public ObservableCollection<TrackListViewModel> TrakLists
        {
            get { return trakLists; }
            set
            {
                trakLists = value;
                OnPropertyChanged("TrakLists");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public double VolumeSound
        {
            get
            {
                return data.VolumeSound;
            }
            set
            {
                data.VolumeSound = value;
                OnPropertyChanged("VolumeSound");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public TrackListViewModel CurrentList
        {
            get { return currentList; }
            set
            {
                currentList = value;
                OnPropertyChanged("CurrentList");
                if (currentList != null)
                {
                    data.CurrentTrackList = currentList.TrackList; 
                    OnCurrentListChanged(currentList);
                }
                    
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public TrackInfoViewModel CurrentPlayTrack
        {
            get { return currentPlayTrack; }
            set
            {
                currentPlayTrack = value;
                OnPropertyChanged("CurrentPlayTrack");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public MediaData Data
        {
            get { return data; }
            set
            {
                data = value;
                OnPropertyChanged("Data");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public MediaDataViewModel(MediaData pData)
        {
            data = pData;
            foreach (var l in Data.TrakLists)
            {
                var tlv = new TrackListViewModel(l);
                TrakLists.Add(tlv);
                tlv.CurrentTrackChanged += () => OnCurrentTrackChanged();
            }
            CurrentList = TrakLists.FirstOrDefault(t => object.ReferenceEquals(Data.CurrentTrackList, t.TrackList));
            if (CurrentList == null && TrakLists.Count > 0)
                CurrentList = TrakLists[0];
//            if (CurrentList == null )
//                OnCurrentListChanged(CurrentList);
            //TrackCollectionViewSource = new CollectionViewSource();
            //if (CurrentList != null)
            //    TrackCollectionViewSource.Source = CurrentList.Traks;
            PropertyChanged += MediaDataViewModel_PropertyChanged;
            TrakLists.CollectionChanged += TrakLists_CollectionChanged;
        }
        //----------------------------------------------------------------------------------------------------------------------    
        void TrakLists_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (TrackListViewModel nt in e.NewItems)
                {
                    data.TrakLists.Add(nt.TrackList);
                }
            if (e.Action == NotifyCollectionChangedAction.Remove)
                foreach (TrackListViewModel ot in e.OldItems)
                {
                    data.TrakLists.Remove(ot.TrackList);
                }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        void MediaDataViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "CurrentList")
            {
                //ICollectionView cvCurrentList = CollectionViewSource.GetDefaultView(CurrentList.Traks);
                //if (cvCurrentList != null && cvCurrentList.CanGroup == true)
                //{
                //    cvCurrentList.GroupDescriptions.Clear();
                //    cvCurrentList.GroupDescriptions.Add(new PropertyGroupDescription("DirectoryName"));
                //}

                OnCurrentTrackChanged();
            }
               
        }
        //----------------------------------------------------------------------------------------------------------------------    
        protected void OnCurrentTrackChanged()
        {
            Action handler = CurrentTrackChanged;
            if (handler != null)
                handler();
        }
        //----------------------------------------------------------------------------------------------------------------------    
        protected void OnCurrentListChanged(TrackListViewModel pList)
        {
            //if (pList.CurrentTrack != null)
            //{
            //    int i = pList.Traks.IndexOf(pList.CurrentTrack);
            //    if (i >= 0)
            //        pList.SelectedIndex = i;
            //}
            //Action<TrackListViewModel> handler = CurrentListChanged;
            //if (handler != null)
            //    handler(pList);
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public void NewPlayList()
        {
            string newListMame = "List" + (TrakLists.Count + 1).ToString();
            TrackList tl = new TrackList(){ListName = newListMame};
            TrackListViewModel tlv = new TrackListViewModel(tl);
            TrakLists.Add(tlv);
            CurrentList = tlv;
        }
        //----------------------------------------------------------------------------------------------------------------------    
        internal void DeleteCurrentTrack()
        {
            if (CurrentList != null)
                CurrentList.DeleteCurrentTrack();
        }
        //----------------------------------------------------------------------------------------------------------------------    
        internal void DeleteFailedTracks()
        {
            if (CurrentList != null)
                CurrentList.DeleteFailedTracks();
        }
        //----------------------------------------------------------------------------------------------------------------------    
        internal void DeleteAllTracks()
        {
            if (CurrentList != null)
                CurrentList.DeleteAllTracks();
        }
        //----------------------------------------------------------------------------------------------------------------------    
        internal void DeleteCurrentPlayList()
        {
            if (CurrentList != null)
                TrakLists.Remove(CurrentList);
            if (TrakLists.Count > 0)
            {
                CurrentList = TrakLists[0];
                //OnCurrentListChanged(CurrentList);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        internal void GoToNextTrack()
        {
            if (CurrentList != null)
                CurrentList.GoToNextTrack();
        }
        //----------------------------------------------------------------------------------------------------------------------    
        internal void CurrentTrackGoToStart()
        {
            //if (CurrentList != null)
            //    CurrentList.CurrentTrackGoToStart();
            CurrentPlayTrack.Position = 0;
        }
        //----------------------------------------------------------------------------------------------------------------------    
        internal void SyncCurrentPlayTrack()
        {
            CurrentPlayTrack = CurrentList.SelectedTrack;
            CurrentList.SetSelection(CurrentPlayTrack);
        }
        //----------------------------------------------------------------------------------------------------------------------    
    }
    //----------------------------------------------------------------------------------------------------------------------    
}
