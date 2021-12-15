﻿using System;

namespace Moody.DialogService
{
    public interface IDialogService
    {
        object? ReturnParameters { get; set; }

        //
        // Summary:
        //     Shows the dialog associated with the pass ViewModel
        void ShowDialog<TViewModel>();
        //
        // Summary:
        //     Shows the dialog associated with the pass ViewModel.
        //     Once the dialog has been closed, the callback will be called
        void ShowDialog<TViewModel>(Action<string> closeCallback);

        //
        // Summary:
        //     Shows the dialog associated with the pass ViewModel.
        //     Returns the 'ReturnParameters' set in the dialog ViewModel
        TReturn ShowDialog<TViewModel, TReturn>();
        //
        // Summary:
        //     Shows the dialog associated with the pass ViewModel.
        //     Once the dialog has been closed, the callback will be called
        //     Returns the 'ReturnParameters' set in the dialog ViewModel.
        TReturn ShowDialog<TViewModel, TReturn>(Action<string> closeCallback);

        //
        // Summary:
        //     Get's the current ReturnParameters
        object? GetReturnParameters<TReturn>();
        //
        // Summary:
        //     Set's the ReturnParameters
        void SetReturnParameters<TReturn>(object? returnParameters);

        //
        // Summary:
        //     Set's the DialogServiceSettings
        void SetDialogSettings(DialogServiceSettings dialogSettings);

        //
        // Summary:
        //     Get's the DialogServiceSettings
        DialogServiceSettings GetDialogSettings(DialogServiceSettings dialogSettings);
    }
}