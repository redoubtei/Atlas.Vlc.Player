﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using DMSkin;
using DMSkin.WPF;
using DMSkin.WPF.API;

namespace Atlas.Vlc.Player.ViewModels
{
    public class AtlasPlayerViewModel: ViewModelBase
    {
        public ICommand ComplexWindowCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    //UI线程执行
                    Execute.OnUIThread(() =>
                    {
                        //new ComplexWindow().Show();
                    });
                });
            }
        }

        public ICommand SimpleWindowCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    //new SimpleWindow().Show();
                });
            }
        }



        private Color _DMWindowShadowColor;

        /// <summary>
        /// 窗体阴影颜色
        /// </summary>
        public Color DMWindowShadowColor
        {
            get { return _DMWindowShadowColor; }
            set
            {
                _DMWindowShadowColor = value;
                OnPropertyChanged("DMWindowShadowColor");
            }
        }

        public ICommand ChangeWindowCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    DMWindowShadowColor = (Color)ColorConverter.ConvertFromString(obj.ToString());
                });
            }
        }



        public ICommand OpenLinkCommand
        {
            get
            {
                return new DelegateCommand(obj =>
                {
                    if (obj is string url)
                    {
                        Process.Start(url);
                    }
                });
            }
        }
    }
}
