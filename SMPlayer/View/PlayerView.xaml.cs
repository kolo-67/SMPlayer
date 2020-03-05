using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using SMPlayerViewModel;
using PlayerCommon;
using SMPlayer.View;

namespace SMPlayer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    //----------------------------------------------------------------------------------------------------------------------
    // class PlayerWindow
    //----------------------------------------------------------------------------------------------------------------------    
    public partial class PlayerWindow : Window
    {
        private bool isStart = true;
        private bool isTimelineFromPosition = false;
        private int numberOfTimerRun = 0;
        private DataGrid currentDataGrid;
        //-------------------------------------------------------------------------------------------------------------------
        public static DependencyProperty CurrentTrackDeletePressCommandProperty = DependencyProperty.RegisterAttached(
                                                                        "CurrentTrackDeletePressCommand",
                                                                        typeof(ICommand),
                                                                        typeof(PlayerWindow));
        //-------------------------------------------------------------------------------------------------------------------
        public ICommand CurrentTrackDeletePressCommand
        {
            get { return (ICommand)GetValue(CurrentTrackDeletePressCommandProperty); }
            set { SetValue(CurrentTrackDeletePressCommandProperty, value); }
        }
        //-------------------------------------------------------------------------------------------------------------------
        public static DependencyProperty CurrentTrackDoubleClickCommandProperty = DependencyProperty.RegisterAttached(
                                                                        "CurrentTrackDoubleClickCommand",
                                                                        typeof(ICommand),
                                                                        typeof(PlayerWindow));
        //-------------------------------------------------------------------------------------------------------------------
        public ICommand CurrentTrackDoubleClickCommand
        {
            get { return (ICommand)GetValue(CurrentTrackDoubleClickCommandProperty); }
            set { SetValue(CurrentTrackDoubleClickCommandProperty, value); }
        }
        //-------------------------------------------------------------------------------------------------------------------
        public PlayerWindow()
        {
            InitializeComponent();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        //----------------------------------------------------------------------------------------------------------------------    
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            var viewModel = new PlayerViewModel();
            DataContext = viewModel;

            IPlayerActionQueriable playerAction = DataContext as IPlayerActionQueriable;
            if (playerAction != null)
            {

                playerAction.PlayQuery += Play;
                playerAction.PauseQuery += Pause;
                playerAction.StopQuery += Stop;
                playerAction.ListChangeQuery += PlayerActionOnListChangeQuery;
                playerAction.FolderDialogQuery += playerAction_FolderDialogQuery;
                playerAction.ChangePathDialogQuery += playerAction_ChangePathDialogQuery;
            }
            ISavable saveModel = DataContext as ISavable;
            if (saveModel != null)
            {
                this.Closing += (s, ev) => saveModel.Save(); ;
                
            }
//            workPlayer.Stretch = Stretch.;
            workPlayer.StretchDirection = StretchDirection.Both;


            Binding bindingCurrentTrackDeletePressCommand = new Binding("CurrentTrackDeletePressCommand");
            bindingCurrentTrackDeletePressCommand.Source = DataContext;
            BindingOperations.SetBinding(this, PlayerWindow.CurrentTrackDeletePressCommandProperty, bindingCurrentTrackDeletePressCommand);

            Binding bindingCurrentTrackDoubleClickCommand = new Binding("CurrentTrackDoubleClickCommand");
            bindingCurrentTrackDoubleClickCommand.Source = DataContext;
            BindingOperations.SetBinding(this, PlayerWindow.CurrentTrackDoubleClickCommandProperty, bindingCurrentTrackDoubleClickCommand);

            IScrollIntoViewAction scrollIntoViewAction = (IScrollIntoViewAction)DataContext;
            scrollIntoViewAction.MainGridScrollIntoView += i =>
            {
                ScrollIntoView();
            };
            playerAction.OnLoad();
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += timer_Tick;
            timer.Start();
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void ScrollIntoView()
        {
            tcList.ApplyTemplate();
            DataTemplate myDataTemplate = tcList.ContentTemplate;
            ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(tcList);
            try
            {
                DataGrid dg = myDataTemplate.FindName("dbTracks", contentPresenter) as DataGrid;
                IPlayerActionQueriable playerAction = DataContext as IPlayerActionQueriable;
                if (dg != null && playerAction != null)
                {
                    int idx = playerAction.TrackByList(dg.DataContext);
                    if (idx >= 0)
                    {
                        dg.ScrollIntoView(dg.Items[idx]);
                        dg.SelectedIndex = idx;
                        Keyboard.Focus(dg);
                        //                           ContentPresenter cp = dg.ItemContainerGenerator.ContainerFromItem(dg.Items[idx]) as ContentPresenter;
                        ////                           TextBox tb = FindVisualChild<TextBox>(cp);
                        //                           TextBlock tb = dg.ItemTemplate.FindName("tbFileName", cp) as TextBlock;
                        //                           if (tb != null)
                        //                           {
                        //                               // do something with tb
                        //                           }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
 
        }
        //-------------------------------------------------------------------------------------------------------------------
        private void PlayerActionOnListChangeQuery(object list, object item)
        {
            tcList.ApplyTemplate();
            DataTemplate myDataTemplate = tcList.ContentTemplate;
            ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(tcList);

            DataGrid dg = myDataTemplate.FindName("dbTracks", contentPresenter) as DataGrid;
            if (dg != null && item != null)
            {
                dg.ScrollIntoView(item);
                int idx = dg.Items.IndexOf(item);
                dg.SelectedIndex = idx;
                //dg.SelectedItems.Clear();
                //dg.SelectedItems.Add(item);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public static T FindVisualChild<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        return (T)child;
                    }

                    T childItem = FindVisualChild<T>(child);
                    if (childItem != null) return childItem;
                }
            }
            return null;
        }
        //----------------------------------------------------------------------------------------------------------------------    
        private void MediaElementMouseDown(Object sender, MouseButtonEventArgs e)
        {
            if (IsPlaying())
            {
                Pause();
            }
            else
            {
                Play();
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        bool IsPlaying()
        {
            var pos1 = workPlayer.Position;
            System.Threading.Thread.Sleep(1);
            var pos2 = workPlayer.Position;

            return pos2 != pos1;
        }
        //----------------------------------------------------------------------------------------------------------------------    
        void playerAction_FolderDialogQuery(object obj)
        {
            FolderView folderView = new FolderView();
            folderView.DataContext = obj;
            folderView.ShowDialog();
        }
        //----------------------------------------------------------------------------------------------------------------------    
        void playerAction_ChangePathDialogQuery(object obj)
        {
            ChangePathView changePathView = new ChangePathView();
            changePathView.DataContext = obj;
            changePathView.ShowDialog();
        }
        //----------------------------------------------------------------------------------------------------------------------    
        private void timer_Tick(object sender, EventArgs e)
        {
            if ((workPlayer.Source != null)) // &&  )
            {
                slTimeline.Minimum = 0;
                if (workPlayer.NaturalDuration.HasTimeSpan)
                {
                    slTimeline.Maximum = workPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
                    if (isStart)
                    {
                        isStart = false;
                        double lPosition = (DataContext as PlayerViewModel).MediaData.CurrentPlayTrack.Position;
                        slTimeline.Value = lPosition;
                        workPlayer.Position = new TimeSpan(0, 0, 0, 0, (int) lPosition);
                    }
                    else
                    {
                        isTimelineFromPosition = true;
                        slTimeline.Value = workPlayer.Position.TotalMilliseconds;
                        isTimelineFromPosition = false;
                    }
                }
            }
            if (numberOfTimerRun < 1)
            {
                if (numberOfTimerRun == 0)
                    ScrollIntoView();
                numberOfTimerRun++;

            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        private void Play()
        {

            workPlayer.Play();
            if (workPlayer.NaturalDuration.HasTimeSpan)
                slTimeline.Maximum = workPlayer.NaturalDuration.TimeSpan.TotalSeconds;

        }
        //----------------------------------------------------------------------------------------------------------------------    
        // Pause the media.
        void Pause()
        {

            workPlayer.Pause();

        }
        //----------------------------------------------------------------------------------------------------------------------    
        // Stop the media.
        void Stop()
        {

            // The Stop method stops and resets the media to be played from
            // the beginning.
            workPlayer.Stop();

        }
        //----------------------------------------------------------------------------------------------------------------------    
        private void SlTimeline_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //if (Math.Abs((workPlayer.Position.TotalMilliseconds - slTimeline.Value)) > 1000 && !isStart)
            //    workPlayer.Position = new TimeSpan(0, 0, 0, 0, (int)e.NewValue);
            if (!isTimelineFromPosition && !isStart)
                workPlayer.Position = new TimeSpan(0, 0, 0, 0, (int)e.NewValue);
        }
        //----------------------------------------------------------------------------------------------------------------------    
        private void WorkPlayer_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            var pa = DataContext as IPlayerActionQueriable;
            if (pa != null)
                pa.EndAction(false);
        }
        //----------------------------------------------------------------------------------------------------------------------    
        private void WorkPlayer_OnMediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            var pa = DataContext as IPlayerActionQueriable;
            if (pa != null)
                pa.EndAction(true);
        }
        //----------------------------------------------------------------------------------------------------------------------    
        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = e.Source as DataGridRow;
            object arg = null;
            if (row != null)
                arg = row.DataContext;

            CurrentTrackDoubleClickCommand.Execute(arg);
        }
        //----------------------------------------------------------------------------------------------------------------------    
        private void DataGrid_OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataGrid dg = sender as DataGrid;
            if (dg != null && dg.SelectedItem != null)
            {
                dg.ScrollIntoView(dg.SelectedItem);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        private void Selector_OnSelected(object sender, RoutedEventArgs e)
        {
            currentDataGrid = sender as DataGrid;
        }
        //----------------------------------------------------------------------------------------------------------------------    
        private void TcList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            return;
            tcList.ApplyTemplate();
            DataTemplate myDataTemplate = tcList.ContentTemplate;
            ContentPresenter contentPresenter = FindVisualChild<ContentPresenter>(tcList);
            try
            {
                DataGrid dg = myDataTemplate.FindName("dbTracks", contentPresenter) as DataGrid;
                IPlayerActionQueriable playerAction = DataContext as IPlayerActionQueriable;
                if (dg != null && playerAction != null)
                {
                    int idx = playerAction.TrackByList(dg.DataContext);
                    if ( idx >= 0)
                    {
                        dg.ScrollIntoView(dg.Items[idx]);
                        dg.SelectedIndex = idx;
                        //dg.SelectedItems.Clear();
                        //dg.SelectedItems.Add(item);
                    }
                }
            }
            catch
            {
                
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        private void TbFileName_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            DataGridRow row = e.Source as DataGridRow;
            object arg = null;
            if (row != null)
                arg = row.DataContext;

            if (e.ClickCount == 2)
                CurrentTrackDoubleClickCommand.Execute(arg);
        }
        //----------------------------------------------------------------------------------------------------------------------    
        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2)
            {
                System.Diagnostics.Process.Start(txtPlaingFileDirectory.Text);
                DataObject data = new DataObject(DataFormats.StringFormat,"cd " + txtPlaingFileDirectory.Text);

                Clipboard.SetDataObject(data);
            }
        }
        //----------------------------------------------------------------------------------------------------------------------    
        private void TbFileName_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
           if (e.Key == Key.Delete)
               CurrentTrackDeletePressCommand.Execute(null);
        }
        //----------------------------------------------------------------------------------------------------------------------    
    }
    //----------------------------------------------------------------------------------------------------------------------    
    public class TimelineMultiValueConvertor : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            TimeSpan ts = (TimeSpan)values[0]; 
            double dValue = ts.TotalMilliseconds;
           
            return dValue;
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            //if (value != null)
            //{
            //    double lSliderValue = (double)value;

            //    TimeSpan ts = new TimeSpan(0, 0, 0, 0, (int)lSliderValue);
            //    return  new object[] { ts, lSliderValue };
            //}
            return new object[] { null, null };
        }
    }
    //----------------------------------------------------------------------------------------------------------------------    
    [ValueConversion(typeof(DateTime), typeof(String))]
    public class TimeSpanAsStringConverter : IValueConverter
    {
        //----------------------------------------------------------------------------------------------------------------------    
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double position = (double)value;
            var ts = new TimeSpan(0, 0, 0, 0, (int) position);
            return (new TimeSpan(0, 0, 0, (int)ts.TotalSeconds, 0)).ToString();
        }
        //----------------------------------------------------------------------------------------------------------------------    
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
        //----------------------------------------------------------------------------------------------------------------------    
    }
    //----------------------------------------------------------------------------------------------------------------------    
}
//----------------------------------------------------------------------------------------------------------------------    

