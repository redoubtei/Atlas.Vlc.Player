using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using DMSkin.WPF.API;

namespace Atlas.Vlc.Player
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        //初始化UI Dispatcher
        protected override void OnStartup(StartupEventArgs e)
        {
            //初始化UI Dispatcher
            Execute.InitializeWithDispatcher();

            ShutdownMode = ShutdownMode.OnLastWindowClose;

            //启动窗口
            AtlasPlayer st = new AtlasPlayer();
            st.Show();

            //ComplexWindow c = new ComplexWindow();
            //c.Show();

            //SimpleMainWindow s = new SimpleMainWindow();
            //s.Show();

            //DemoWindow d = new DemoWindow();
            //d.Show();
        }
    }
}
