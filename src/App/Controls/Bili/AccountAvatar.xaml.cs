﻿// Copyright (c) Richasy. All rights reserved.

using System.ComponentModel;
using Richasy.Bili.Controller.Uwp;
using Richasy.Bili.ViewModels.Uwp;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Richasy.Bili.App.Controls
{
    /// <summary>
    /// 账户管理中枢.
    /// </summary>
    public sealed partial class AccountAvatar : UserControl
    {
        /// <summary>
        /// <see cref="ViewModel"/>的依赖属性.
        /// </summary>
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(nameof(ViewModel), typeof(AccountViewModel), typeof(AccountAvatar), new PropertyMetadata(AccountViewModel.Instance));

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountAvatar"/> class.
        /// </summary>
        public AccountAvatar()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
            this.Unloaded += OnUnloaded;
        }

        /// <summary>
        /// 账户视图模型.
        /// </summary>
        public AccountViewModel ViewModel
        {
            get { return (AccountViewModel)GetValue(ViewModelProperty); }
            set { SetValue(ViewModelProperty, value); }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            CheckStatus();
            ViewModel.PropertyChanged += OnViewModelPropertyChanged;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            ViewModel.PropertyChanged -= OnViewModelPropertyChanged;
        }

        private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(ViewModel.Status))
            {
                CheckStatus();
            }
        }

        private void CheckStatus()
        {
            switch (ViewModel.Status)
            {
                case AccountViewModelStatus.Logout:
                case AccountViewModelStatus.Login:
                    VisualStateManager.GoToState(this, nameof(NormalState), false);
                    break;
                case AccountViewModelStatus.Logging:
                    VisualStateManager.GoToState(this, nameof(LoadingState), false);
                    break;
                default:
                    break;
            }
        }

        private async void OnUserAvatarTappedAsync(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            if (ViewModel.Status == AccountViewModelStatus.Logout)
            {
                await ViewModel.TrySignInAsync();
            }
        }
    }
}
