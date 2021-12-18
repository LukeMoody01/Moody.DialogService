using System.Windows;

namespace Moody.Core.Settings
{
    /// <summary>
    /// Represents the Settings defined in the xaml
    /// </summary>
    public static class DialogSettings 
    {
        /// <summary>
        /// The Dialog Window Name
        /// </summary>
        public static readonly DependencyProperty DialogNameProperty = 
            DependencyProperty.RegisterAttached("DialogName", typeof(string), typeof(DialogSettings), new PropertyMetadata(defaultValue: null, propertyChangedCallback: DialogNameChanged));

        /// <summary>
        /// The Dialog Window Style
        /// </summary>
        public static readonly DependencyProperty WindowStyleProperty =
            DependencyProperty.RegisterAttached("WindowStyle", typeof(WindowStyle), typeof(DialogSettings), new PropertyMetadata(defaultValue: WindowStyle.SingleBorderWindow, propertyChangedCallback: WindowStyleChanged));

        /// <summary>
        /// Gets the value for the <see cref="DialogNameProperty"/> attached property.
        /// </summary>
        /// <param name="obj">The target element.</param>
        /// <returns>The <see cref="DialogNameProperty"/> attached to the <paramref name="obj"/> element.</returns>
        public static string? GetDialogName(DependencyObject obj)
        {
            return (string?)obj.GetValue(DialogNameProperty);
        }

        /// <summary>
        /// Gets the value for the <see cref="WindowStyleProperty"/> attached property.
        /// </summary>
        /// <param name="obj">The target element.</param>
        /// <returns>The <see cref="WindowStyleProperty"/> attached to the <paramref name="obj"/> element.</returns>
        public static WindowStyle GetWindowStyle(DependencyObject obj)
        {
            return (WindowStyle)obj.GetValue(WindowStyleProperty);
        }

        /// <summary>
        /// Sets the <see cref="DialogNameProperty"/> attached property.
        /// </summary>
        /// <param name="obj">The target element.</param>
        /// <param name="value">The value to attach.</param>
        public static void SetDialogName(DependencyObject obj, string? value)
        {
            obj.SetValue(DialogNameProperty, value);
        }

        /// <summary>
        /// Sets the <see cref="WindowStyleProperty"/> attached property.
        /// </summary>
        /// <param name="obj">The target element.</param>
        /// <param name="value">The value to attach.</param>
        public static void SetWindowStyle(DependencyObject obj, WindowStyle value)
        {
            obj.SetValue(WindowStyleProperty, value);
        }

        private static void DialogNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = (string?)e.NewValue;
            SetDialogName(d, value);
        }

        private static void WindowStyleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = (WindowStyle)e.NewValue;
            SetWindowStyle(d, value);
        }
    }
}
