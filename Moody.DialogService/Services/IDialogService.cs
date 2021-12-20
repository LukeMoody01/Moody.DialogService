using Moody.Core.Models;
using Moody.Core.Settings;
using System;

namespace Moody.Core.Services
{
    /// <summary>
    /// Represents Dialog Service.
    /// </summary>
    /// <remarks>
    /// A ViewModel that injects this interface can open, close, 
    /// and return parameters during the showing and closing of Dialogs.
    /// </remarks>
    public interface IDialogService
    {
        /// <summary>
        /// Shows the dialog associated with the passed ViewModel.
        /// </summary>
        /// <typeparam name="TViewModel">The associated ViewModel to the requested View</typeparam>
        void ShowDialog<TViewModel>();

        /// <summary>
        /// Shows the dialog associated with the passed ViewModel.
        /// </summary>
        /// <typeparam name="TViewModel">The associated ViewModel to the requested View</typeparam>
        /// <param name="dialogParameters">KeyValuePair of parameters used in IDialogAware ViewModels</param>
        void ShowDialog<TViewModel>(DialogParameters dialogParameters);

        /// <summary>
        ///  Shows the dialog associated with the passed ViewModel.
        ///  Once the dialog has been closed, the callback will be called
        /// </summary>
        /// <typeparam name="TViewModel">The associated ViewModel to the requested View</typeparam>
        /// <param name="closeCallback">The callback called once the dialog has been requested to close</param>
        void ShowDialog<TViewModel>(Action closeCallback);

        /// <summary>
        ///  Shows the dialog associated with the passed ViewModel.
        ///  Once the dialog has been closed, the callback will be called        
        /// </summary>
        /// <typeparam name="TViewModel">The associated ViewModel to the requested View</typeparam>
        /// <param name="closeCallback">The callback called once the dialog has been requested to close</param>
        /// <param name="dialogParameters">KeyValuePair of parameters used in IDialogAware ViewModels</param>
        void ShowDialog<TViewModel>(DialogParameters dialogParameters, Action closeCallback);


        /// <summary>
        ///  Shows the dialog associated with the passed ViewModel.
        /// </summary>
        /// <typeparam name="TViewModel">The associated ViewModel to the requested View</typeparam>
        /// <typeparam name="TReturn">The expected return type</typeparam>
        /// <returns>Returns the 'ReturnParameters' set in the dialog ViewModel</returns>
        TReturn ShowDialog<TViewModel, TReturn>();

        /// <summary>
        ///  Shows the dialog associated with the passed ViewModel.
        /// </summary>
        /// <typeparam name="TViewModel">The associated ViewModel to the requested View</typeparam>
        /// <typeparam name="TReturn">The expected return type</typeparam>
        /// <param name="dialogParameters">KeyValuePair of parameters used in IDialogAware ViewModels</param>
        /// <returns></returns>
        TReturn ShowDialog<TViewModel, TReturn>(DialogParameters dialogParameters);

        /// <summary>
        ///  Shows the dialog associated with the passed ViewModel.
        /// </summary>
        /// <typeparam name="TViewModel">The associated ViewModel to the requested View</typeparam>
        /// <typeparam name="TReturn">The expected return type</typeparam>
        /// <param name="closeCallback">The callback called once the dialog has been requested to close</param>
        /// <returns>Returns the 'ReturnParameters' set in the dialog ViewModel</returns>
        TReturn ShowDialog<TViewModel, TReturn>(Action closeCallback);

        /// <summary>
        ///  Shows the dialog associated with the passed ViewModel.
        /// </summary>
        /// <typeparam name="TViewModel">The associated ViewModel to the requested View</typeparam>
        /// <typeparam name="TReturn">The expected return type</typeparam>
        /// <param name="closeCallback">The callback called once the dialog has been requested to close</param>
        /// <param name="dialogParameters">KeyValuePair of parameters used in IDialogAware ViewModels</param>
        /// <returns>Returns the 'ReturnParameters' set in the dialog ViewModel</returns>
        TReturn ShowDialog<TViewModel, TReturn>(DialogParameters dialogParameters, Action closeCallback);

        /// <summary>
        /// Closes the dialog associated with the passed ViewModel
        /// </summary>
        /// <typeparam name="TViewModel">The associated ViewModel to the requested View</typeparam>
        void CloseDialog<TViewModel>();

        /// <summary>
        /// Get's the current ReturnParameters
        /// </summary>
        /// <typeparam name="TReturn">The expected return type</typeparam>
        /// <returns>Returns the ReturnParameters as the requested type</returns>
        TReturn? GetReturnParameters<TReturn>();

        /// <summary>
        /// Set's the ReturnParameters
        /// </summary>
        /// <param name="returnParameters">The value of the expected return parameters</param>
        void SetReturnParameters(object? returnParameters);

        /// <summary>
        /// Set's the DefaultDialogSettings
        /// </summary>
        /// <param name="dialogSettings">The value of the expected default settings</param>
        void SetDefaultDialogSettings(DefaultDialogSettings dialogSettings);

        /// <summary>
        /// Get's the current Default Dialog Settings
        /// </summary>
        /// <returns>Returns the Default Dialog Settings</returns>
        DefaultDialogSettings GetDefaultDialogSettings();
    }
}