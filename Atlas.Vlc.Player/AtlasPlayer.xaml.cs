using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vlc.DotNet.Core;
using Vlc.DotNet.Core.Interops.Signatures;
using Vlc.DotNet.Wpf;

namespace Atlas.Vlc.Player
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AtlasPlayer
    {
       
        public AtlasPlayer()
        {
            InitializeComponent();
            initImgVlc();
        }
        private VlcVideoSourceProvider sourceProvider;
        private string _currentFile = string.Empty;
        private volatile bool m_isDrag;
        private void DMSkinSimpleWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.videoTimeLine.AddHandler(Thumb.DragCompletedEvent, new DragCompletedEventHandler(this.sldPosition_DragCompleted));
            this.videoTimeLine.AddHandler(Thumb.DragStartedEvent, new DragStartedEventHandler(this.sldPosition_DragStarted));
            this.sldVolume.ValueChanged +=new RoutedPropertyChangedEventHandler<double>(sldVolume_ValueChanged);
            //控件绑定
            this.myVlcPlayer.SetBinding(System.Windows.Controls.Image.SourceProperty,
                new Binding(nameof(VlcVideoSourceProvider.VideoSource)) { Source = sourceProvider });
            //播放时间事件
            this.sourceProvider.MediaPlayer.TimeChanged += 
                new EventHandler<VlcMediaPlayerTimeChangedEventArgs>(Events_TimeChanged);
            //播放进度
            this.sourceProvider.MediaPlayer.PositionChanged += 
                new EventHandler<VlcMediaPlayerPositionChangedEventArgs>(Events_PlayerPositionChanged);
            //结束
            this.sourceProvider.MediaPlayer.EndReached +=
                new EventHandler<VlcMediaPlayerEndReachedEventArgs>(Events_PlayerEndChanged);
        }
        /// <summary>
        /// 使用image做载体
        /// </summary>
        private void initImgVlc()
        {

            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            if (currentDirectory == null)
                return;


            var VlcLibDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, @"vlc\lib"));
            this.sourceProvider = new VlcVideoSourceProvider(this.Dispatcher);
            this.sourceProvider.CreatePlayer(VlcLibDirectory/* pass your player parameters here */);

        }

        /// <summary>
        /// 使用控件做载体
        /// </summary>
        private void InitVlc()
        {
          
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            if (currentDirectory == null)
                return;
            var libDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, @"vlc\lib"));
            //this.myVlcPlayer.SourceProvider.CreatePlayer(libDirectory/* pass your player parameters here */);
        }

        /// <summary>
        /// 本地视频
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_play_click(object sender, RoutedEventArgs e)
        {

            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)//注意，此处一定要手动引入System.Window.Forms空间，否则你如果使用默认的DialogResult会发现没有OK属性
            {
                _currentFile = openFileDialog.FileName;
                //Image做载体播放

                FileInfo fileInfo=new FileInfo(_currentFile);
                this.sourceProvider.MediaPlayer.SetMedia(fileInfo);
                this.sourceProvider.MediaPlayer.LengthChanged +=new EventHandler<VlcMediaPlayerLengthChangedEventArgs>(Events_LengthChanged);
                this.sourceProvider.MediaPlayer.Play();

                PlayerTitle.Text = fileInfo.Name;
               
                //this.BackgroundVideo.SetBinding(System.Windows.Controls.Image.SourceProperty,
                //    new Binding(nameof(VlcVideoSourceProvider.VideoSource)) { Source = sourceProvider });


                //控件做载体播放
              
               // this.myVlcPlayer.SourceProvider.MediaPlayer.Play(fileInfo);
            }
           

        }

        private void btn_play_stream(object sender, RoutedEventArgs e)
        {
            var url = "rtsp://123.147.113.84:554/04000001/01000000004000000000000000000193?AuthInfo=xxx&userid=";

            this.sourceProvider.MediaPlayer.Play(new Uri(url));
        }
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_play_start(object sender, RoutedEventArgs e)
        {
            if (this.sourceProvider.MediaPlayer.IsPlaying())
            {
                this.sourceProvider.MediaPlayer.Pause();
            }
            else if(this.sourceProvider.MediaPlayer.IsPausable())
            {
                this.sourceProvider.MediaPlayer.Play();
            }
        }
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_play_left(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_play_right(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_play_stop(object sender, RoutedEventArgs e)
        {
            
            try
            {
                this.Dispatcher.BeginInvoke(new Action(delegate
                    {
                      
                    this.sourceProvider?.MediaPlayer.Stop(); 
                    
                }
                ));

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 时间变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Events_LengthChanged(object sender, VlcMediaPlayerLengthChangedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate
            {
               
             long totalMs = this.sourceProvider.MediaPlayer.Length;
                labDuration.Content= TimeSpan.FromMilliseconds(totalMs).ToString().Substring(0, 8);
             //   labTime.Content=""
            }));
        }
        /// <summary>
        /// 时间变化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Events_TimeChanged(object sender, VlcMediaPlayerTimeChangedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate
            {
                labTime.Content = TimeSpan.FromMilliseconds(e.NewTime).ToString().Substring(0, 8);
            }));
        }
        /// <summary>
        /// 播放进度
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Events_PlayerPositionChanged(object sender,VlcMediaPlayerPositionChangedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate
            {
                if (!m_isDrag)
                {
                    videoTimeLine.Value = (double)e.NewPosition;
                }
            }));
        }
        /// <summary>
        /// 播放结束
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Events_PlayerEndChanged(object sender, VlcMediaPlayerEndReachedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate
            {
                InitControls();
            }));
        }

        private void InitControls()
        {
            videoTimeLine.Value = 0;
            labTime.Content = "00:00:00";
            labDuration.Content = "00:00:00";
        }
        /// <summary>
        /// 获取总时长
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Events_DurationChanged(object sender, VlcMediaDurationChangedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(new Action(delegate
            {
                labDuration.Content = TimeSpan.FromMilliseconds(e.NewDuration).ToString().Substring(0, 8);
            }));
        }

        private void sldPosition_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            this.sourceProvider.MediaPlayer.Position = (float)videoTimeLine.Value;
            m_isDrag = false;
        }

        private void sldPosition_DragStarted(object sender, DragStartedEventArgs e)
        {
            m_isDrag = true;
        }

        private void sldVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (this.sourceProvider.MediaPlayer != null)
            {
                this.sourceProvider.MediaPlayer.Audio.Volume= (int)e.NewValue;
               
                //this.sourceProvider.MediaPlayer.GetMedia() .Audio = (int)e.NewValue;
            }
        }
    }
}
