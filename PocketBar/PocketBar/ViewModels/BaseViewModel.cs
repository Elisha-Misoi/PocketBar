﻿using PocketBar.Services;
using Prism.Navigation;
using Prism.Services;
using Refit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PocketBar.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        INavigationService NavigationService { get; set; }
        IPageDialogService PageDialogService { get; set; }
        public bool IsLoading { get; set; }

        public Task<bool> HasInternetConnection()
        {
            if(Connectivity.NetworkAccess == NetworkAccess.Internet)
            {
                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }
        }

        public Task ShowMessage(string title, string message, string cancel, string accept = null)
        {
           return PageDialogService.DisplayAlertAsync(title, message, accept, cancel);
        }
    }
}
