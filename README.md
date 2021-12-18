<h1 align="center"> Moody.DialogService </h1>

<p align="center">
  <img src="https://github.com/LukeMoody01/Moody.DialogService/blob/master/moodyIcon.jpg">
</p>

<p align="center">
    Moody DialogService is a service designed to handle dialogs in an MVVM pattern (No more code behind!).
</p>

# How to use it?

### Setup
Before using the DialogService, we need to make sure the view is registered against the desired ViewModel (For later use).

Also, you need to make sure your IServiceCollection has all the services registered, otherwise the service will not work. Why?
This is because when we activate a view, we need to resolve any services you may be passing in the code behind.. (Although you shouldn't be).
So if its not registered, we will have problems..

Anyway, You have 2 options on registering Dialogs
#### Manual Register
```c#
DialogService.RegisterDialog<DialogOne, DialogOneViewModel>();
```
#### Automatic Register
```c#
DialogService.AutoRegisterDialogs<App>();
```
The reason we pass through App, is so that the AutoRegister knows what assembly to look into.

NOTE: Your "Dialog" CANNOT be a window. It will have to be a type of UserControl.
This is because the service uses a "DialogShell" and injects the UserControl into the shell content.

Also, for AutoRegister to work, you will need to add the 'DialogModule' attribute in the code behind of the desired Dialogs you would like to register.
```c#
[DialogModule]
public partial class DialogOne : UserControl
{
    public DialogOne()
    {
        InitializeComponent();
    }
}
```
### Consuming the Service
To consume the service, you will need to register it to your IoC Container. 
```c#
private void ConfigureServices(IServiceCollection services)
{
    services.AddSingleton<IDialogService, DialogService>();
}

```
After you have added it to your IoC Conatiner, you can now start using it in your ViewModels!
```c#
//Constructor
public MainWindowViewModel(IDialogService dialogService)
{
    _dialogService = dialogService;
}

public void ShowSomeDialog()
{
    _dialogService.ShowDialog<SomeDialogViewModel>();
}

public void CloseSomeDialog()
{
    _dialogService.CloseDialog<SomeDialogViewModel>();
}
```

#### Setting the Dialog Settings
When you add these properties in the control, the service will pick up these settings and apply it to this dialog.
(All dialogs can be unique)
```xaml
<UserControl 
    xmlns:moody="clr-namespace:Moody.DialogService.Settings;assembly=Moody.DialogService"
    moody:DialogSettings.DialogName="Dialog One"
    moody:DialogSettings.WindowStyle="ToolWindow">
<!--Some UI Code-->
</UserControl>
```

#### Setting default Dialog Settings
Incase you want to override the default settings, you can use the SetDefaultDialogSettings method to apply your own defaults!
```c#
_dialogService.SetDefaultDialogSettings(new DefaultDialogSettings()
{
    DialogWindowDefaultStyle = WindowStyle.None,
    DialogWindowDefaultTitle = "Window Title",
});
```

#### Returning values
Lets say your Dialog needs to pass back an object to your ViewModel.. How would we do that?

We would use the DialogServices 'ReturnParameters'

```c#
//MainViewModel
public void ShowSomeDialogThatReturns()
{
    var expectedObject = _dialogService.ShowDialog<SomeDialogViewModel, bool>();
}

//DialogViewModel
public void SomeMethodInDialog()
{
    _dialogService.ReturnParameters = true;
}
```
Once the above dialog closes, the expectedObject will be true (bool).

Now how you handle closing the dialog is up to you, But ensure you set the ReturnParameters before closing!

If you have any questions or comments, please send me a message on one of the platforms below!

[![LinkedIn][linkedin-shield]][linkedin-url]
[![Discord][discord-shield]][discord-url]

[discord-shield]: https://img.shields.io/badge/Discord-Moody-orange
[discord-url]: https://discord.com/users/269162855255769089
[linkedin-shield]: https://img.shields.io/badge/-LinkedIn-black.svg?style=for-the-badge&logo=linkedin&colorB=555
[linkedin-url]: https://www.linkedin.com/in/luke-moody-0482651a6/
