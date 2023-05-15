// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Controls;
using System.Windows.Input;

namespace MetadataControlExperiment.Samples;

/// <summary>
/// An example sample page of a custom control inheriting from Panel.
/// </summary>
[ToolkitSampleTextOption("Separator", " • ", Title = "Separator")]
[ToolkitSampleTextOption("AccessibleSeparator", ", ", Title = "AccessibleSeparator")]

[ToolkitSample(id: nameof(MetadataControlSample), "MetadataControl", description: $"A sample for showing how to create and use a {nameof(MetadataControl)} control.")]
public sealed partial class MetadataControlSample : Page
{
    private static readonly string[] Labels = "Lorem ipsum dolor sit amet consectetur adipiscing elit".Split(' ');

    private readonly Random _random;
    private readonly ObservableCollection<MetadataItem> _units;
    private readonly DelegateCommand<object> _command;

    public MetadataControlSample()
    {
        this.InitializeComponent();
        _random = new Random();
        _units = new ObservableCollection<MetadataItem>();
        _command = new DelegateCommand<object>(OnExecuteCommand);
        metadataControl.Items = _units;
    }

    private string GetRandomLabel() => Labels[_random!.Next(Labels.Length)];

    private void OnExecuteCommand(object obj)
    {
        OutputTxt.Text = $"Command invoked - parameter: {obj}";
    }

    private void AddLabel_Click(object sender, RoutedEventArgs e)
    {
        if (_units != null)
        {
            _units.Add(new MetadataItem { Label = GetRandomLabel() });
        }
    }

    private void AddCommand_Click(object sender, RoutedEventArgs e)
    {
        if (_units != null)
        {
            var label = GetRandomLabel();
            _units.Add(new MetadataItem
            {
                Label = label,
                Command = _command!,
                CommandParameter = label,
            });
        }
    }

    private void Clear_Click(object sender, RoutedEventArgs e)
    {
        if (_units != null)
        {
            OutputTxt.Text = "";
            _units.Clear();
        }
    }
}

public class DelegateCommand : ICommand
{
    private readonly Action commandExecuteAction;

    private readonly Func<bool> commandCanExecute;

    /// <summary>
    /// Initializes a new instance of the <see cref="DelegateCommand"/> class.
    /// </summary>
    /// <param name="execute">
    /// The action to execute when called.
    /// </param>
    /// <param name="canExecute">
    /// The function to call to determine if the command can execute the action.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if the execute action is null.
    /// </exception>
    public DelegateCommand(Action execute, Func<bool>? canExecute = null)
    {
        if (execute == null)
        {
            throw new ArgumentNullException(nameof(execute));
        }

        commandExecuteAction = execute;
        commandCanExecute = canExecute ?? (() => true);
    }

    /// <summary>
    /// Occurs when changes occur that affect whether or not the command should execute.
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    /// Defines the method that determines whether the command can execute in its current state.
    /// </summary>
    /// <param name="parameter">
    /// The parameter used by the command.
    /// </param>
    /// <returns>
    /// Returns a value indicating whether this command can be executed.
    /// </returns>
    public bool CanExecute(object? parameter = null)
    {
        try
        {
            return commandCanExecute();
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Defines the method to be called when the command is invoked.
    /// </summary>
    /// <param name="parameter">
    /// The parameter used by the command.
    /// </param>
    public void Execute(object? parameter)
    {
        if (!CanExecute(parameter))
        {
            return;
        }

        try
        {
            commandExecuteAction();
        }
        catch
        {
            Debugger.Break();
        }
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}


public class DelegateCommand<T> : ICommand
{
    private readonly Action<T> commandExecuteAction;

    private readonly Func<T, bool> commandCanExecute;

    public DelegateCommand(Action<T> executeAction, Func<T, bool>? canExecute = null)
    {
        if (executeAction == null)
        {
            throw new ArgumentNullException(nameof(executeAction));
        }

        commandExecuteAction = executeAction;
        commandCanExecute = canExecute ?? (e => true);
    }

    /// <summary>
    /// Occurs when changes occur that affect whether or not the command should execute.
    /// </summary>
    public event EventHandler? CanExecuteChanged;

    /// <summary>
    /// Defines the method that determines whether the command can execute in its current state.
    /// </summary>
    /// <param name="parameter">
    /// The parameter used by the command.
    /// </param>
    /// <returns>
    /// Returns a value indicating whether this command can be executed.
    /// </returns>
    public bool CanExecute(object? parameter)
    {
        try
        {
            return commandCanExecute(ConvertParameterValue(parameter!));
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Defines the method to be called when the command is invoked.
    /// </summary>
    /// <param name="parameter">
    /// The parameter used by the command.
    /// </param>
    public void Execute(object? parameter)
    {
        if (!CanExecute(parameter))
        {
            return;
        }

        try
        {
            commandExecuteAction(ConvertParameterValue(parameter!));
        }
        catch
        {
            Debugger.Break();
        }
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    private static T ConvertParameterValue(object parameter)
    {
        parameter = parameter is T ? parameter : Convert.ChangeType(parameter, typeof(T));
        return (T)parameter;
    }
}
