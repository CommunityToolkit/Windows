// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


#if WINAPPSDK
using CommunityToolkit.WinUI.Deferred;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Input;
using VirtualKey = Windows.System.VirtualKey;
using DispatcherQueuePriority = Microsoft.UI.Dispatching.DispatcherQueuePriority;
using DispatcherQueue = Microsoft.UI.Dispatching.DispatcherQueue;
#else
using DispatcherQueuePriority = Windows.System.DispatcherQueuePriority;
using DispatcherQueue = Windows.System.DispatcherQueue;
using CommunityToolkit.WinUI.Deferred;
#endif
using Windows.System;
using Windows.UI.Core;
using Windows.Foundation.Metadata;
using CommunityToolkit.WinUI.Automation.Peers;
using CommunityToolkit.WinUI.Helpers;

namespace CommunityToolkit.WinUI.Controls;

/// <summary>
/// A text input control that auto-suggests and displays token items.
/// </summary>
[global::System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1124:Do not use regions", Justification = "Organization")]
[TemplatePart(Name = PART_NormalState, Type = typeof(VisualState))]
[TemplatePart(Name = PART_PointerOverState, Type = typeof(VisualState))]
[TemplatePart(Name = PART_FocusedState, Type = typeof(VisualState))]
[TemplatePart(Name = PART_UnfocusedState, Type = typeof(VisualState))]
[TemplatePart(Name = PART_MaxReachedState, Type = typeof(VisualState))]
public partial class TokenizingTextBox : ListViewBase
{
    internal const string PART_NormalState = "Normal";
    internal const string PART_PointerOverState = "PointerOver";
    internal const string PART_FocusedState = "Focused";
    internal const string PART_UnfocusedState = "Unfocused";
    internal const string PART_MaxReachedState = "MaxReachedState";

    /// <summary>
    /// Gets a value indicating whether the shift key is currently in a pressed state
    /// </summary>

#if WINAPPSDK
    internal static bool IsShiftPressed => InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Shift).HasFlag(CoreVirtualKeyStates.Down);
#else
    internal static bool IsShiftPressed => CoreWindow.GetForCurrentThread()!.GetKeyState(VirtualKey.Shift).HasFlag(CoreVirtualKeyStates.Down);
#endif
    /// <summary>
    /// Gets a value indicating whether the control key is currently in a pressed state
    /// </summary>

#if WINAPPSDK
    internal bool IsControlPressed => InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down);
#else
    internal bool IsControlPressed => CoreWindow.GetForCurrentThread()!.GetKeyState(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down);
#endif
    internal bool PauseTokenClearOnFocus { get; set; }

    internal bool IsClearingForClick { get; set; }

    private DispatcherQueue _dispatcherQueue;
    private InterspersedObservableCollection _innerItemsSource;
    private ITokenStringContainer _currentTextEdit; // Don't update this directly outside of initialization, use UpdateCurrentTextEdit Method - in future see https://github.com/dotnet/csharplang/issues/140#issuecomment-625012514
    private ITokenStringContainer _lastTextEdit;

    /// <summary>
    /// Initializes a new instance of the <see cref="TokenizingTextBox"/> class.
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public TokenizingTextBox()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        // Setup our base state of our collection
        _innerItemsSource = new InterspersedObservableCollection(new ObservableCollection<object>()); // TODO: Test this still will let us bind to ItemsSource in XAML?
        _currentTextEdit = _lastTextEdit = new PretokenStringContainer(true);
        _innerItemsSource.Insert(_innerItemsSource.Count, _currentTextEdit);
        ItemsSource = _innerItemsSource;
        //// TODO: Consolidate with callback below for ItemsSourceProperty changed?

        DefaultStyleKey = typeof(TokenizingTextBox);

        // TODO: Do we want to support ItemsSource better? Need to investigate how that works with adding...
        RegisterPropertyChangedCallback(ItemsSourceProperty, ItemsSource_PropertyChanged);
        PreviewKeyDown += TokenizingTextBox_PreviewKeyDown;
        PreviewKeyUp += TokenizingTextBox_PreviewKeyUp;
        CharacterReceived += TokenizingTextBox_CharacterReceived;
        ItemClick += TokenizingTextBox_ItemClick;

#if WINAPPSDK
        _dispatcherQueue = DispatcherQueue;
#else
        _dispatcherQueue = DispatcherQueue.GetForCurrentThread();
#endif
    }

    private void ItemsSource_PropertyChanged(DependencyObject sender, DependencyProperty dp)
    {
        // If we're given a different ItemsSource, we need to wrap that collection in our helper class.
        if (ItemsSource != null && ItemsSource.GetType() != typeof(InterspersedObservableCollection))
        {
            _innerItemsSource = new InterspersedObservableCollection(ItemsSource);

            if (ReadLocalValue(MaximumTokensProperty) != DependencyProperty.UnsetValue && _innerItemsSource.ItemsSource.Count >= MaximumTokens)
            {
                // Reduce down to below the max as necessary.
                var endCount = MaximumTokens > 0 ? MaximumTokens : 0;
                for (var i = _innerItemsSource.ItemsSource.Count - 1; i >= endCount; --i)
                {
                    _innerItemsSource.Remove(_innerItemsSource[i]);
                }
            }

            // Add our text box at the end of items and set its default value to our initial text, fix for #4749
            _currentTextEdit = _lastTextEdit = new PretokenStringContainer(true) { Text = Text };
            _innerItemsSource.Insert(_innerItemsSource.Count, _currentTextEdit);
            ItemsSource = _innerItemsSource;
        }
    }

    private void TokenizingTextBox_ItemClick(object sender, ItemClickEventArgs e)
    {
        // If the user taps an item in the list, make sure to clear any text selection as required
        // Note, token selection is cleared by the listview default behavior
        if (!IsControlPressed)
        {
            // Set class state flag to prevent click item being immediately deselected
            IsClearingForClick = true;
            ClearAllTextSelections(null);
        }
    }

    private void TokenizingTextBox_PreviewKeyUp(object sender, KeyRoutedEventArgs e)
    {
        TokenizingTextBox_PreviewKeyUp(e.Key);
    }

    internal void TokenizingTextBox_PreviewKeyUp(VirtualKey key)
    {
        switch (key)
        {
            case VirtualKey.Escape:
            {
                // Clear any selection and place the focus back into the text box
                DeselectAllTokensAndText();
                FocusPrimaryAutoSuggestBox();
                break;
            }
        }
    }

    /// <summary>
    /// Set the focus to the last item in the collection
    /// </summary>
    private void FocusPrimaryAutoSuggestBox()
    {
        if (Items?.Count > 0)
        {
            if (ContainerFromIndex(Items.Count - 1) is TokenizingTextBoxItem container)
            {
                container.Focus(FocusState.Programmatic);
            }
        }
    }

    private async void TokenizingTextBox_PreviewKeyDown(object sender, KeyRoutedEventArgs e)
    {
        e.Handled = await TokenizingTextBox_PreviewKeyDown(e.Key);
    }

    internal async Task<bool> TokenizingTextBox_PreviewKeyDown(VirtualKey key)
    {
        // Global handlers on control regardless if focused on item or in textbox.
        switch (key)
        {
            case VirtualKey.C:
                if (IsControlPressed)
                {
                    CopySelectedToClipboard();
                    return true;
                }

                break;

            case VirtualKey.X:
                if (IsControlPressed)
                {
                    CopySelectedToClipboard();

                    // now clear all selected tokens and text, or all if none are selected
                    await RemoveAllSelectedTokens();
                }

                break;

            // For moving between tokens
            case VirtualKey.Left:
                return MoveFocusAndSelection(MoveDirection.Previous);

            case VirtualKey.Right:
                return MoveFocusAndSelection(MoveDirection.Next);

            case VirtualKey.A:
                // modify the select-all behavior to ensure the text in the edit box gets selected.
                if (IsControlPressed)
                {
                    this.SelectAllTokensAndText();
                    return true;
                }

                break;
        }

        return false;
    }

    /// <inheritdoc/>
    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        var selectAllMenuItem = new MenuFlyoutItem
        {
            // TO DO: "WCT_TokenizingTextBox_MenuFlyout_SelectAll".GetLocalized("Microsoft.Toolkit.Uwp.UI.Controls.Input/Resources")
            Text = "Select all"
        };
        selectAllMenuItem.Click += (s, e) => this.SelectAllTokensAndText();
        var menuFlyout = new MenuFlyout();
        menuFlyout.Items.Add(selectAllMenuItem);

#if !HAS_UNO
        if (IsXamlRootAvailable && XamlRoot != null)
        {
            menuFlyout.XamlRoot = XamlRoot;
        }
#endif
        ContextFlyout = menuFlyout;
    }

    internal void RaiseQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        QuerySubmitted?.Invoke(sender, args);
    }

    internal void RaiseSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        SuggestionChosen?.Invoke(sender, args);
    }

    internal void RaiseTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        TextChanged?.Invoke(sender, args);
    }

    private async void TokenizingTextBox_CharacterReceived(UIElement sender, CharacterReceivedRoutedEventArgs args)
    {
        var container = ContainerFromItem(_currentTextEdit) as TokenizingTextBoxItem;

        if (container != null && !(GetFocusedElement().Equals(container._autoSuggestTextBox) || char.IsControl(args.Character)))
        {
            if (SelectedItems.Count > 0)
            {
                var index = _innerItemsSource.IndexOf(SelectedItems.First());

                await RemoveAllSelectedTokens();

                // Wait for removal of old items
#if WINAPPSDK
                _ = DispatcherQueue.EnqueueAsync(
#else
                var dispatcherQueue = DispatcherQueue.GetForCurrentThread();
                _ = dispatcherQueue.EnqueueAsync(
#endif
                    () =>
                    {
                        // If we're before the last textbox and it's empty, redirect focus to that one instead
                        if (index == _innerItemsSource.Count - 1 && string.IsNullOrWhiteSpace(_lastTextEdit.Text))
                        {
                            if (ContainerFromItem(_lastTextEdit) is TokenizingTextBoxItem lastContainer)
                            {
                                lastContainer.UseCharacterAsUser = true; // Make sure we trigger a refresh of suggested items.

                                _lastTextEdit.Text = string.Empty + args.Character;

                                UpdateCurrentTextEdit(_lastTextEdit);

                                lastContainer._autoSuggestTextBox.SelectionStart = 1; // Set position to after our new character inserted

                                lastContainer._autoSuggestTextBox.Focus(FocusState.Keyboard);
                            }
                        }
                        else
                        {
                            //// Otherwise, create a new textbox for this text.

                            UpdateCurrentTextEdit(new PretokenStringContainer((string.Empty + args.Character).Trim())); // Trim so that 'space' isn't inserted and can be used to insert a new box.

                            _innerItemsSource.Insert(index, _currentTextEdit);

                            // Need to wait for containerization
#if WINAPPSDK
                            _ = DispatcherQueue.EnqueueAsync(
#else
                            _ = dispatcherQueue.EnqueueAsync(
#endif
                                () =>
                                {
                                    if (ContainerFromIndex(index) is TokenizingTextBoxItem newContainer) // Should be our last text box
                                    {
                                        newContainer.UseCharacterAsUser = true; // Make sure we trigger a refresh of suggested items.

                                        void WaitForLoad(object s, RoutedEventArgs eargs)
                                        {
                                            if (newContainer._autoSuggestTextBox != null)
                                            {
                                                newContainer._autoSuggestTextBox.SelectionStart = 1; // Set position to after our new character inserted

                                                newContainer._autoSuggestTextBox.Focus(FocusState.Keyboard);
                                            }

                                            newContainer.Loaded -= WaitForLoad;
                                        }

                                        newContainer.AutoSuggestTextBoxLoaded += WaitForLoad;
                                    }
                                }, DispatcherQueuePriority.Normal);
                        }
                    }, DispatcherQueuePriority.Normal);
            }
            else
            {
                // If no items are selected, send input to the last active string container.
                // This code is only fires during an edgecase where an item is in the process of being deleted and the user inputs a character before the focus has been redirected to a string container.
                if (_innerItemsSource[_innerItemsSource.Count - 1] is ITokenStringContainer textToken)
                {
                    if (ContainerFromIndex(Items.Count - 1) is TokenizingTextBoxItem last) // Should be our last text box
                    {
                        var text = last._autoSuggestTextBox.Text;
                        var selectionStart = last._autoSuggestTextBox.SelectionStart;
                        var position = selectionStart > text.Length ? text.Length : selectionStart;
                        textToken.Text = text.Substring(0, position) + args.Character +
                                         text.Substring(position);

                        last._autoSuggestTextBox.SelectionStart = position + 1; // Set position to after our new character inserted

                        last._autoSuggestTextBox.Focus(FocusState.Keyboard);
                    }
                }
            }
        }
    }

    private object GetFocusedElement()
    {
        if (IsXamlRootAvailable && XamlRoot != null)
        {
            return FocusManager.GetFocusedElement(XamlRoot)!;
        }
        else
        {
            return FocusManager.GetFocusedElement()!;
        }
    }

    #region ItemsControl Container Methods

    /// <inheritdoc/>
    protected override DependencyObject GetContainerForItemOverride() => new TokenizingTextBoxItem();

    /// <inheritdoc/>
    protected override bool IsItemItsOwnContainerOverride(object item)
    {
        return item is TokenizingTextBoxItem;
    }

    /// <inheritdoc/>
    protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
    {
        base.PrepareContainerForItemOverride(element, item);

        if (element is TokenizingTextBoxItem tokenitem)
        {
            tokenitem.Owner = this;

            tokenitem.ContentTemplateSelector = TokenItemTemplateSelector;
            tokenitem.ContentTemplate = TokenItemTemplate;

            tokenitem.ClearClicked -= TokenizingTextBoxItem_ClearClicked;
            tokenitem.ClearClicked += TokenizingTextBoxItem_ClearClicked;

            tokenitem.ClearAllAction -= TokenizingTextBoxItem_ClearAllAction;
            tokenitem.ClearAllAction += TokenizingTextBoxItem_ClearAllAction;

            tokenitem.GotFocus -= TokenizingTextBoxItem_GotFocus;
            tokenitem.GotFocus += TokenizingTextBoxItem_GotFocus;

            tokenitem.LostFocus -= TokenizingTextBoxItem_LostFocus;
            tokenitem.LostFocus += TokenizingTextBoxItem_LostFocus;

            var menuFlyout = new MenuFlyout();

            var removeMenuItem = new MenuFlyoutItem
            {
                // TO DO: Localize - "WCT_TokenizingTextBoxItem_MenuFlyout_Remove".GetLocalized("Microsoft.Toolkit.Uwp.UI.Controls.Input/Resources")
                Text = "Remove"
            };
            removeMenuItem.Click += (s, e) => TokenizingTextBoxItem_ClearClicked(tokenitem, null);

            menuFlyout.Items.Add(removeMenuItem);

#if !HAS_UNO
            if (IsXamlRootAvailable && XamlRoot != null)
            {
                menuFlyout.XamlRoot = XamlRoot;
            }
#endif
            var selectAllMenuItem = new MenuFlyoutItem
            {
                // TO DO: Localize - "WCT_TokenizingTextBox_MenuFlyout_SelectAll".GetLocalized("Microsoft.Toolkit.Uwp.UI.Controls.Input/Resources")
                Text = "Select all"
            };
            selectAllMenuItem.Click += (s, e) => this.SelectAllTokensAndText();

            menuFlyout.Items.Add(selectAllMenuItem);

            tokenitem.ContextFlyout = menuFlyout;
        }
    }
    #endregion

    private void TokenizingTextBoxItem_GotFocus(object sender, RoutedEventArgs e)
    {
        // Keep track of our currently focused textbox
        if (sender is TokenizingTextBoxItem ttbi && ttbi.Content is ITokenStringContainer text)
        {
            UpdateCurrentTextEdit(text);
        }
    }

    private void TokenizingTextBoxItem_LostFocus(object sender, RoutedEventArgs e)
    {
        // Keep track of our currently focused textbox
        if (sender is TokenizingTextBoxItem ttbi && ttbi.Content is ITokenStringContainer text &&
            string.IsNullOrWhiteSpace(text.Text) && text != _lastTextEdit)
        {
            // We're leaving an inner textbox that's blank, so we'll remove it
            _innerItemsSource.Remove(text);

            UpdateCurrentTextEdit(_lastTextEdit);

            GuardAgainstPlaceholderTextLayoutIssue();
        }
    }

    /// <summary>
    /// Adds the specified data item as a new token to the collection, will raise the <see cref="TokenItemAdding"/> event asynchronously still for confirmation.
    /// </summary>
    /// <remarks>
    /// The <see cref="TokenizingTextBox"/> will automatically handle adding items for you, or you can add items to your backing <see cref="ItemsControl.ItemsSource"/> collection. This method is provide for other cases where you may need to drive inserting a new token item to where the user is currently inserting text between tokens.
    /// </remarks>
    /// <param name="data">Item to add as a token.</param>
    /// <param name="atEnd">Flag to indicate if the item should be inserted in the last used textbox (inserted) or placed at end of the token list.</param>
    public void AddTokenItem(object data, bool atEnd = false)
    {
        _ = AddTokenAsync(data, atEnd);
    }

    /// <summary>
    /// Clears the whole collection, will raise the <see cref="TokenItemRemoving"/> event asynchronously for each item.
    /// </summary>
    /// <returns>async task</returns>
    public async Task ClearAsync()
    {
        while (_innerItemsSource.Count > 1)
        {
            if (ContainerFromItem(_innerItemsSource[0]) is TokenizingTextBoxItem container)
            {
                if (!await RemoveTokenAsync(container, _innerItemsSource[0]))
                {
                    // if a removal operation fails then stop the clear process
                    break;
                }
            }
        }

        // Clear the active pretoken string.
        // Setting the text property directly avoids a delay when setting the text in the autosuggest box.
        Text = string.Empty;
    }

    internal async Task AddTokenAsync(object data, bool? atEnd = null)
    {
        if (ReadLocalValue(MaximumTokensProperty) != DependencyProperty.UnsetValue && (MaximumTokens <= 0 || MaximumTokens <= _innerItemsSource.ItemsSource.Count))
        {
            // No tokens for you
            return;
        }

        if (data is string str && TokenItemAdding != null)
        {
            var tiaea = new TokenItemAddingEventArgs(str);
            await TokenItemAdding.InvokeAsync(this, tiaea);

            if (tiaea.Cancel)
            {
                return;
            }

            if (tiaea.Item != null)
            {
                data = tiaea.Item; // Transformed by event implementor
            }
        }

        // If we've been typing in the last box, just add this to the end of our collection
        if (atEnd == true || _currentTextEdit == _lastTextEdit)
        {
            _innerItemsSource.InsertAt(_innerItemsSource.Count - 1, data);
        }
        else
        {
            // Otherwise, we'll insert before our current box
            var edit = _currentTextEdit;
            var index = _innerItemsSource.IndexOf(edit);

            // Insert our new data item at the location of our textbox
            _innerItemsSource.InsertAt(index, data);

            // Remove our textbox
            _innerItemsSource.Remove(edit);
        }

        // Focus back to our end box as Outlook does.
        var last = ContainerFromItem(_lastTextEdit) as TokenizingTextBoxItem;
        last?._autoSuggestTextBox.Focus(FocusState.Keyboard);

        TokenItemAdded?.Invoke(this, data);

        GuardAgainstPlaceholderTextLayoutIssue();
    }

    /// <summary>
    /// Helper to change out the currently focused text element in the control.
    /// </summary>
    /// <param name="edit"><see cref="ITokenStringContainer"/> element which is now the main edited text.</param>
    protected void UpdateCurrentTextEdit(ITokenStringContainer edit)
    {
        _currentTextEdit = edit;

        Text = edit.Text; // Update our text property.
    }

    /// <summary>
    /// Creates AutomationPeer (<see cref="UIElement.OnCreateAutomationPeer"/>)
    /// </summary>
    /// <returns>An automation peer for this <see cref="TokenizingTextBox"/>.</returns>
    protected override AutomationPeer OnCreateAutomationPeer()
    {
        return new TokenizingTextBoxAutomationPeer(this);
    }

    /// <summary>
    /// Remove the specified token from the list.
    /// </summary>
    /// <param name="item">Item in the list to delete</param>
    /// <param name="data">data </param>
    /// <remarks>
    /// the data parameter is passed in optionally to support UX UTs. When running in the UT the Container items are not manifest.
    /// </remarks>
    /// <returns><b>true</b> if the item was removed successfully, <b>false</b> otherwise</returns>
    private async Task<bool> RemoveTokenAsync(TokenizingTextBoxItem item, object? data = null)
    {
        if (data == null)
        {
            data = ItemFromContainer(item);
        }

        if (TokenItemRemoving != null)
        {
            var tirea = new TokenItemRemovingEventArgs(data, item);
            await TokenItemRemoving.InvokeAsync(this, tirea);

            if (tirea.Cancel)
            {
                return false;
            }
        }

        _innerItemsSource.Remove(data);

        TokenItemRemoved?.Invoke(this, data);

        GuardAgainstPlaceholderTextLayoutIssue();

        return true;
    }

    private void GuardAgainstPlaceholderTextLayoutIssue()
    {
        // If the *PlaceholderText is visible* on the last AutoSuggestBox, it can incorrectly layout itself
        // when the *ASB has focus*. We think this is an optimization in the platform, but haven't been able to
        // isolate a straight-reproduction of this issue outside of this control (though we have eliminated
        // most Toolkit influences like ASB/TextBox Style, the InterspersedObservableCollection, etc...).
        // The only Toolkit component involved here should be WrapPanel (which is a straight-forward Panel).
        // We also know the ASB itself is adjusting it's size correctly, it's the inner component.
        //
        // To combat this issue:
        //   We toggle the visibility of the Placeholder ContentControl in order to force it's layout to update properly
        var placeholder = ContainerFromItem(_lastTextEdit)?.FindDescendant("PlaceholderTextContentPresenter");

        if (placeholder?.Visibility == Visibility.Visible)
        {
            placeholder.Visibility = Visibility.Collapsed;

            // After we ensure we've hid the control, make it visible again (this is imperceptible to the user).
            _ = CompositionTargetHelper.ExecuteAfterCompositionRenderingAsync(() =>
            {
                placeholder.Visibility = Visibility.Visible;
            });
        }
    }

    /// <summary>
    /// Checks if the XamlRoot property is available via the current API. 
    /// </summary>
    public static bool IsXamlRootAvailable { get; } = ApiInformation.IsPropertyPresent("Windows.UI.Xaml.UIElement", "XamlRoot");
}
