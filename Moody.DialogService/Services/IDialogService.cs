using Moody.Core.Settings;
using System;

namespace Moody.Core.Services
{
    public interface IDialogService
    {
        //
        // Summary:
        //     Shows the dialog associated with the passed ViewModel
        void ShowDialog<TViewModel>();

        //
        // Summary:
        //     Shows the dialog associated with the passed ViewModel.
        //     Once the dialog has been closed, the callback will be called
        void ShowDialog<TViewModel>(Action<string> closeCallback);

        //
        // Summary:
        //     Shows the dialog associated with the passed ViewModel.
        //     Returns the 'ReturnParameters' set in the dialog ViewModel
        TReturn ShowDialog<TViewModel, TReturn>();

        //
        // Summary:
        //     Shows the dialog associated with the passed ViewModel.
        //     Once the dialog has been closed, the callback will be called
        //     Returns the 'ReturnParameters' set in the dialog ViewModel.
        TReturn ShowDialog<TViewModel, TReturn>(Action<string> closeCallback);

        //
        // Summary:
        //     Closes the dialog associated with the passed ViewModel
        void CloseDialog<TViewModel>();

        //
        // Summary:
        //     Get's the current ReturnParameters
        object? GetReturnParameters<TReturn>();

        //
        // Summary:
        //     Set's the ReturnParameters
        void SetReturnParameters(object? returnParameters);

        //
        // Summary:
        //     Set's the DefaultDialogSettings
        void SetDefaultDialogSettings(DefaultDialogSettings dialogSettings);

        //
        // Summary:
        //     Get's the DefaultDialogSettings
        DefaultDialogSettings GetDefaultDialogSettings();
    }
}