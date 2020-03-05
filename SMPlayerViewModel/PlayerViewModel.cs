using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using PlayerCommon;
using PlayerCommon.ViewModel;
using SMPlayerModel;
using System.Windows;


namespace SMPlayerViewModel
{
    //----------------------------------------------------------------------------------------------------------------------
    // class PlayerViewModel
    //----------------------------------------------------------------------------------------------------------------------    
    public class PlayerViewModel : ViewModelBase, IPlayerActionQueriable, ISavable, IScrollIntoViewAction
    {
        private const string PlayerDataFileName = "SMPlayerData.bin";
        private string selectedTreckUri;
        private double timelineValue;
        private string mediaElementSource;
        private MediaDataViewModel mediaData;
        private int selectedIndex;
        public event Action PauseQuery;
        public event Action PlayQuery;
        public event Action StopQuery;
        public event Action<object,object> ListChangeQuery;
        public event Action<object> FolderDialogQuery;
        public event Action<object> ChangePathDialogQuery;
        public event Action<Object> MainGridScrollIntoView;
        //----------------------------------------------------------------------------------------------------------------------
        public MediaDataViewModel MediaData
        {
            get
            {
                return mediaData;
            }
            set
            {
                mediaData = value;
                OnPropertyChanged("MediaData");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public string SelectedTreckUri
        {
            get
            {
                return selectedTreckUri;
            }
            set
            {
                selectedTreckUri = value;
                OnPropertyChanged("SelectedTreckUri");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                selectedIndex = value;
                OnPropertyChanged("SelectedIndex");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public double TimelineValue
        {
            get { return timelineValue; }
            set
            {
                timelineValue = value;
                OnPropertyChanged("TimelineValue");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public string MediaElementSource
        {
            get { return mediaElementSource; }
            set
            {
                mediaElementSource = value;
                OnPropertyChanged("MediaElementSource");
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        public PlayerViewModel()
        {
            MediaData data;
            if (File.Exists(PlayerDataFileName))
            {
                data = Read();
                MediaData = new MediaDataViewModel(data);
            }
            else
            {
                MediaElementSource = @"E:\Data\Sound\OC-2\VIA Eolika(1980-1988)\Noktjurn.mp3";
                data = new MediaData();
                TrackList list = new TrackList() { ListName = "Video" };

                data.TrakLists.Add(list);

                data.CurrentTrackList = data.TrakLists[0];
                MediaData = new MediaDataViewModel(data);
                Save();
            }

            PropertyChanged += PlayerViewModel_PropertyChanged;
            MediaData.CurrentTrackChanged += MediaData_CurrentTrackChanged;
            MediaData.CurrentListChanged += MediaDataOnCurrentListChanged;
            
            if (MediaData.CurrentList != null)
            {
//                SelectedIndex = 0;
                MediaData.SyncCurrentPlayTrack();
                OnMainGridScrollIntoView(MediaData.CurrentList.SelectedTrack);
            }
                
            //MediaData.CurrentPlayTrack = MediaData.CurrentList.CurrentTrack;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public void OnLoad()
        {
            if (MediaData.CurrentList != null)
            {
                OnMainGridScrollIntoView(MediaData.CurrentList.SelectedTrack);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------
        private void MediaDataOnCurrentListChanged(TrackListViewModel pList)
        {
            OnListChangeQuery(pList, pList.SelectedTrack);
        }
        //----------------------------------------------------------------------------------------------------------------------
        void MediaData_CurrentTrackChanged()
        {
            //if (this.MediaData.CurrentList != null && this.MediaData.CurrentList.CurrentTrack != null)
            //{
            //    MediaElementSource = this.MediaData.CurrentList.CurrentTrack.FullDirectoryName + "\\" +
            //        this.MediaData.CurrentList.CurrentTrack.FileName;
            //}

            //StopAction();
            //PlayAction();
        }
        //----------------------------------------------------------------------------------------------------------------------
        void PlayerViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "")
            {
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        public int TrackByList(object pList)
        {
            if (MediaData.CurrentList != null && object.ReferenceEquals(pList, MediaData.CurrentList))
            {
                return MediaData.CurrentList.SelectedIndex;
            }
            return -1;
        }
        //-------------------------------------------------------------------------------------------------------------------
        public MediaData Read()
        {
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(PlayerDataFileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            MediaData data = (MediaData)formatter.Deserialize(stream);
            stream.Close();
            return data;
        }
        //-------------------------------------------------------------------------------------------------------------------
        public void Save()
        {
            MediaData data = MediaData.Data;
            IFormatter formatter = new BinaryFormatter();
            Stream stream = new FileStream(PlayerDataFileName, FileMode.Create, FileAccess.Write, FileShare.None);
            formatter.Serialize(stream, data);
            stream.Close();
        }
        //-------------------------------------------------------------------------------------------------------------------
        public void OnPauseQuery()
        {
            Action handle = PauseQuery;
            if (handle != null)
                handle();
        }
        //-------------------------------------------------------------------------------------------------------------------
        public void OnPlayQuery()
        {
            Action handle = PlayQuery;
            if (handle != null)
                handle();
        }
        //-------------------------------------------------------------------------------------------------------------------
        public void OnStopQuery()
        {
            Action handle = StopQuery;
            if (handle != null)
                handle();
        }
        //-------------------------------------------------------------------------------------------------------------------
        public void OnListChangeQuery(object pList, object pTrack)
        {
            Action<object,object> handle = ListChangeQuery;
            if (handle != null)
                handle(pList, pTrack);
        }
        //-------------------------------------------------------------------------------------------------------------------
        public void OnFolderDialogQuery(ShowFolderViewModel showFolderViewModel)
        {
            Action<object> handle = FolderDialogQuery;
            if (handle != null)
                handle(showFolderViewModel);
        }
        //-------------------------------------------------------------------------------------------------------------------
        public void OnChangePathDialogQuery(ChangePathViewModel changePathViewModel)
        {
            ChangePathDialogQuery?.Invoke(changePathViewModel);
        }
        //-------------------------------------------------------------------------------------------------------------------
        private ICommand playCommand;
        //-------------------------------------------------------------------------------------------------------------------
        public ICommand PlayCommand
        {
            get
            {
                if (playCommand == null)
                {
                    playCommand = new DelegateCommand(PlayAction, CanPlayAction);
                }
                return playCommand;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void PlayAction()
        {
//            OnStopQuery();
            OnPlayQuery();
            if (MediaData != null && MediaData.CurrentList != null)
                MediaData.CurrentPlayTrack = MediaData.CurrentList.SelectedTrack; 
        }
        //-------------------------------------------------------------------------------------------------------------------
        private bool CanPlayAction()
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------------
        private ICommand pauseCommand;
        //-------------------------------------------------------------------------------------------------------------------
        public ICommand PauseCommand
        {
            get
            {
                if (pauseCommand == null)
                {
                    pauseCommand = new DelegateCommand(PauseAction, CanPauseAction);
                }
                return pauseCommand;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void PauseAction()
        {
            OnPauseQuery();
        }
        //-------------------------------------------------------------------------------------------------------------------
        private bool CanPauseAction()
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------------
        private ICommand stopCommand;
        //-------------------------------------------------------------------------------------------------------------------
        public ICommand StopCommand
        {
            get
            {
                if (stopCommand == null)
                {
                    stopCommand = new DelegateCommand(StopAction, CanStopAction);
                }
                return stopCommand;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void StopAction()
        {
            OnStopQuery();
        }
        //-------------------------------------------------------------------------------------------------------------------
        private bool CanStopAction()
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------------
        private ICommand playToStartCommand;
        //-------------------------------------------------------------------------------------------------------------------
        public ICommand PlayToStartCommand
        {
            get
            {
                if (playToStartCommand == null)
                {
                    playToStartCommand = new DelegateCommand(PlayToStartAction, CanPlayToStartAction);
                }
                return playToStartCommand;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void PlayToStartAction()
        {
            MediaData.CurrentTrackGoToStart();
            OnPlayQuery();
        }
        //-------------------------------------------------------------------------------------------------------------------
        private bool CanPlayToStartAction()
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------------
        private ICommand addFoldersCommand;
        //-------------------------------------------------------------------------------------------------------------------
        public ICommand AddFoldersCommand
        {
            get
            {
                if (addFoldersCommand == null)
                {
                    addFoldersCommand = new DelegateCommand(AddFoldersAction, CanAddFoldersAction);
                }
                return addFoldersCommand;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void AddFoldersAction()
        {
            string[] fileExtentions = new string[] { "mp3", "wav", "avi", "mpeg", "mpeg2", "wma", "wmv","mp4", "mpg", "mkv" };
            ShowFolderViewModel showFolderViewModel = new ShowFolderViewModel();
            OnFolderDialogQuery(showFolderViewModel);
            var sf = showFolderViewModel.GetSelectetedFolders();
            foreach (var d in sf)
            {
                foreach (string ext in fileExtentions)
                {
                    foreach (var f in d.Directory.GetFiles("*." + ext, SearchOption.AllDirectories))
                    {
                        TrackInfo ti = new TrackInfo()
                        {
                            DirectoryName = f.DirectoryName,
                            FileName = f.Name,
                            FullDirectoryName = f.Directory.FullName,
                            Position = 0
                        };
                        MediaData.CurrentList.AddTrackInfo(ti);
                    }
                }
            }
            Save();
        }
        //-------------------------------------------------------------------------------------------------------------------
        private bool CanAddFoldersAction()
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------------
        private ICommand changePathCommand;
        //-------------------------------------------------------------------------------------------------------------------
        public ICommand ChangePathCommand
        {
            get
            {
                if (changePathCommand == null)
                {
                    changePathCommand = new DelegateCommand(ChangePathAction, CanChangePathAction);
                }
                return changePathCommand;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void ChangePathAction()
        {
            if (MediaData.CurrentList == null || (MediaData.CurrentList?.Traks?.Count ?? 0) == 0)
                return;
            ChangePathViewModel changePathViewModel = new ChangePathViewModel() {
                PathFrom = MediaData.CurrentList.Traks[0].FullDirectoryName,
                PathTo = ""
            };
            OnChangePathDialogQuery(changePathViewModel);

            if (changePathViewModel.IsAccept && !string.IsNullOrEmpty(changePathViewModel.PathFrom) && !string.IsNullOrEmpty(changePathViewModel.PathTo))
            {
                for (int i = 0; i < MediaData.CurrentList.Traks.Count; i++)
                {
                    MediaData.CurrentList.Traks[i].FullDirectoryName = MediaData.CurrentList.Traks[i].FullDirectoryName.Replace(changePathViewModel.PathFrom, changePathViewModel.PathTo);
                }
                Save();
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        private bool CanChangePathAction()
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------------
        private ICommand addFilesCommand;
        //-------------------------------------------------------------------------------------------------------------------
        public ICommand AddFilesCommand
        {
            get
            {
                if (addFilesCommand == null)
                {
                    addFilesCommand = new DelegateCommand(AddFilesAction, CanAddFilesAction);
                }
                return addFilesCommand;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void AddFilesAction()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Multiselect = true;
            dlg.FileName = "Document"; // Default file name
//            dlg.DefaultExt = ".mp3"; // Default file extension
            dlg.Filter = "Text documents (.*)|*.*"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                foreach (var f in dlg.FileNames)
                {
                    TrackInfo ti = new TrackInfo(){
                        DirectoryName = Path.GetFileName(Path.GetDirectoryName(f)),
                        FileName = Path.GetFileName(f),
                        FullDirectoryName = Path.GetDirectoryName(f),
                        Position = 0
                    };
                    MediaData.CurrentList.AddTrackInfo(ti);
                }
            }
            Save();
        }
        //-------------------------------------------------------------------------------------------------------------------
        private bool CanAddFilesAction()
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------------
        private ICommand deleteFileCommand;
        //-------------------------------------------------------------------------------------------------------------------
        public ICommand DeleteFileCommand
        {
            get
            {
                if (deleteFileCommand == null)
                {
                    deleteFileCommand = new DelegateCommand(DeleteFileAction, CanDeleteFileAction);
                }
                return deleteFileCommand;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void DeleteFileAction()
        {
            MediaData.DeleteCurrentTrack();
        }
        //-------------------------------------------------------------------------------------------------------------------
        private bool CanDeleteFileAction()
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------------
        private ICommand deleteFailedFileCommand;
        //-------------------------------------------------------------------------------------------------------------------
        public ICommand DeleteFailedFileCommand
        {
            get
            {
                if (deleteFailedFileCommand == null)
                {
                    deleteFailedFileCommand = new DelegateCommand(DeleteFailedFileAction, CanDeleteFailedFileAction);
                }
                return deleteFailedFileCommand;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void DeleteFailedFileAction()
        {
            MediaData.DeleteFailedTracks();
        }
        //-------------------------------------------------------------------------------------------------------------------
        private bool CanDeleteFailedFileAction()
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------------
        private ICommand deleteAllFileCommand;
        //-------------------------------------------------------------------------------------------------------------------
        public ICommand DeleteAllFileCommand
        {
            get
            {
                if (deleteAllFileCommand == null)
                {
                    deleteAllFileCommand = new DelegateCommand(DeleteAllFileAction, CanDeleteAllFileAction);
                }
                return deleteAllFileCommand;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void DeleteAllFileAction ()
        {
            if (MessageBox.Show("Are you sure?","All track will be deleted.",MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                MediaData.DeleteAllTracks();
        }
        //-------------------------------------------------------------------------------------------------------------------
        private bool CanDeleteAllFileAction()
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------------
        private ICommand addPlayListsCommand;
        //-------------------------------------------------------------------------------------------------------------------
        public ICommand AddPlayListsCommand
        {
            get
            {
                if (addPlayListsCommand == null)
                {
                    addPlayListsCommand = new DelegateCommand(AddPlayListsAction, CanAddPlayListsAction);
                }
                return addPlayListsCommand;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void AddPlayListsAction()
        {
            MediaData.NewPlayList();
        }
        //-------------------------------------------------------------------------------------------------------------------
        private bool CanAddPlayListsAction()
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------------
        private ICommand deletePlayListCommand;
        //-------------------------------------------------------------------------------------------------------------------
        public ICommand DeletePlayListCommand
        {
            get
            {
                if (deletePlayListCommand == null)
                {
                    deletePlayListCommand = new DelegateCommand(DeletePlayListAction, CanDeletePlayListAction);
                }
                return deletePlayListCommand;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void DeletePlayListAction()
        {
            OnMainGridScrollIntoView(MediaData.CurrentList.SelectedTrack);
            if (MessageBox.Show("Are you sure?", "List will be deleted.", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                MediaData.DeleteCurrentPlayList();
        }
        //-------------------------------------------------------------------------------------------------------------------
        private bool CanDeletePlayListAction()
        {
            return true;
        }
        private DelegateCommand<object> currentTrackDoubleClickCommand;
        //-------------------------------------------------------------------------------------------------------------------
        public ICommand CurrentTrackDoubleClickCommand
        {
            get
            {
                if (currentTrackDoubleClickCommand == null)
                {
                    currentTrackDoubleClickCommand = new DelegateCommand<object>(CurrentTrackDoubleClickAction, CanCurrentTrackDoubleClickAction);
                }
                return currentTrackDoubleClickCommand;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void CurrentTrackDoubleClickAction(object obj)
        {
            OnPlayQuery();
            if (MediaData != null && MediaData.CurrentList != null)
                MediaData.CurrentPlayTrack = MediaData.CurrentList.SelectedTrack; 
        }
        //-------------------------------------------------------------------------------------------------------------------
        private bool CanCurrentTrackDoubleClickAction(object obj)
        {
            return true;
        }
        private DelegateCommand<object> currentTrackDeletePressCommand;
        //-------------------------------------------------------------------------------------------------------------------
        public ICommand CurrentTrackDeletePressCommand
        {
            get
            {
                if (currentTrackDeletePressCommand == null)
                {
                    currentTrackDeletePressCommand = new DelegateCommand<object>(CurrentTrackDeletePressckAction, CanCurrentTrackDeletePressAction);
                }
                return currentTrackDeletePressCommand;
            }
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void CurrentTrackDeletePressckAction(object obj)
        {
            MediaData.DeleteCurrentTrack();
        }
        //-------------------------------------------------------------------------------------------------------------------
        private bool CanCurrentTrackDeletePressAction(object obj)
        {
            return true;
        }
        //----------------------------------------------------------------------------------------------------------------------
        public void EndAction(bool isFailed)
        {
            if (MediaData.CurrentList != null && MediaData.CurrentList.SelectedTrack != null)
            {
                MediaData.CurrentList.SelectedTrack.IsFailed = isFailed;
                MediaData.GoToNextTrack();
                OnMainGridScrollIntoView(MediaData.CurrentList.SelectedTrack);
            }
            PlayAction();
//            OnPlayQuery();
        }
        //-------------------------------------------------------------------------------------------------------------------
        protected virtual void OnMainGridScrollIntoView(object item)
        {
            Action<object> handler = MainGridScrollIntoView;
            if (handler != null)
                handler(item);
        }        
        //----------------------------------------------------------------------------------------------------------------------
    }
    //----------------------------------------------------------------------------------------------------------------------
}
