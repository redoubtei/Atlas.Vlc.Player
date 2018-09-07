using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Vlc.DotNet.Wpf;

namespace Atlas.Vlc.Player
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AtlasPlayer
    {
        private VlcVideoSourceProvider sourceProvider;
        public AtlasPlayer()
        {
            InitializeComponent();
            initImgVlc();
        }

        /// <summary>
        /// 使用image做载体
        /// </summary>
        private void initImgVlc()
        {
            file.Text = @"d:\video\test.avi";


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
            file.Text = @"d:\video\test.avi";
            var currentAssembly = Assembly.GetEntryAssembly();
            var currentDirectory = new FileInfo(currentAssembly.Location).DirectoryName;
            if (currentDirectory == null)
                return;
            var libDirectory = new DirectoryInfo(System.IO.Path.Combine(currentDirectory, @"vlc\lib"));
            //this.myVlcPlayer.SourceProvider.CreatePlayer(libDirectory/* pass your player parameters here */);
        }

        private void btn_play_click(object sender, RoutedEventArgs e)
        {
            var txt = file.Text;
            if (string.IsNullOrEmpty(txt))
            {
                MessageBox.Show("请选择文件");
                return;

            }
            //Image做载体播放
            this.sourceProvider.MediaPlayer.Play(new FileInfo(txt));
            this.myVlcPlayer.SetBinding(System.Windows.Controls.Image.SourceProperty,
               new Binding(nameof(VlcVideoSourceProvider.VideoSource)) { Source = sourceProvider });

            //this.BackgroundVideo.SetBinding(System.Windows.Controls.Image.SourceProperty,
            //    new Binding(nameof(VlcVideoSourceProvider.VideoSource)) { Source = sourceProvider });


            //控件做载体播放
            // this.myVlcPlayer.SourceProvider.MediaPlayer.Play(new FileInfo(txt));
        }
    }
}
