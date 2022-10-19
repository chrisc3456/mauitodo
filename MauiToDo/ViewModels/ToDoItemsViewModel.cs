using MauiToDo.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace MauiToDo.ViewModels
{
    public class ToDoItemsViewModel : INotifyPropertyChanged
    {
        private IToDoService _toDoService;
        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<ToDoItem> _toDoItems;
        public ObservableCollection<ToDoItem> ToDoItems
        {
            get => _toDoItems;
            set => _toDoItems = value;
        }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                if (_isRefreshing == value)
                    return;

                _isRefreshing = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsRefreshing)));
            }
        }

        public ICommand LoadDataCommand { get; private set; }
        public ICommand ItemTappedCommand { get; private set; }
        public ICommand ToggleItemCompletedCommand { get; private set; }
        public ICommand DeleteItemCommand { get; private set; }

        public ToDoItemsViewModel(IToDoService toDoService)
        {
            _toDoItems = new();
            _toDoService = toDoService;

            LoadDataCommand = new Command(async () => await LoadData());
            ItemTappedCommand = new Command<ToDoItem>(async (item) => await ItemTapped(item));
            ToggleItemCompletedCommand = new Command<ToDoItem>(async (item) => await ToggleItemCompleted(item));
            DeleteItemCommand = new Command<ToDoItem>(async (item) => await DeleteItem(item));

            Task.Run(LoadData);
        }

        public async Task LoadData()
        {
            try
            {
                IsRefreshing = true;

                var result = await _toDoService.GetToDoList();
                List<ToDoItem> items = result.OrderBy(item => item.CreatedTime).ToList();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ToDoItems.Clear();
                    foreach (ToDoItem item in items)
                    {
                        ToDoItems.Add(item);
                    }
                }
                );
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        public async Task AddItem(string description)
        {
            try
            {
                if (description == null)
                    return;

                ToDoCreateItem newItem = new ToDoCreateItem(description);
                ToDoItem createdItem = await _toDoService.CreateToDoItem(newItem);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ToDoItems.Add(createdItem);
                });
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        public async Task ItemTapped(ToDoItem toDoItem)
        {
            //TODO: Investigate if this is the best way to achieve this, feels like this should be part of UI implementation as it refers to a page (albeit the root page rather than a specific one) - any way of notifying the page code-behind that it should display the prompt?
            //TODO: Check default 'cancel' approach on iOS. Upper case on Android generally but defaults to first character upper only which looks odd against the uppercase default OK option
            string newDescription = await App.Current.MainPage.DisplayPromptAsync("Existing To-Do Item", "Enter new details", cancel: "CANCEL", initialValue: toDoItem.TaskDescription, keyboard: Keyboard.Text);

            // Result will be null if the user has cancelled the prompt
            if (newDescription == null)
                return;

            try
            {
                ToDoUpdateItem updateItem = new ToDoUpdateItem()
                {
                    TaskDescription = newDescription,
                    IsCompleted = toDoItem.IsCompleted
                };

                ToDoItem updatedItem = await _toDoService.UpdateToDoItem(toDoItem.Id, updateItem);

                int index = ToDoItems.IndexOf(toDoItem);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ToDoItems[index] = updatedItem;
                }
                );
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        public async Task ToggleItemCompleted(ToDoItem toDoItem)
        {
            try
            {
                ToDoUpdateItem updateItem = new ToDoUpdateItem()
                {
                    TaskDescription = toDoItem.TaskDescription,
                    IsCompleted = !toDoItem.IsCompleted
                };

                ToDoItem updatedItem = await _toDoService.UpdateToDoItem(toDoItem.Id, updateItem);

                int index = ToDoItems.IndexOf(toDoItem);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ToDoItems[index] = updatedItem;
                }
                );
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        public async Task DeleteItem(ToDoItem toDoItem)
        {
            try
            {
                var result = await _toDoService.DeleteToDoItem(toDoItem.Id);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    ToDoItems.Remove(toDoItem);
                }
                );
            }
            catch (Exception exception)
            {
                HandleException(exception);
            }
        }

        private void HandleException(Exception exception)
        {
            App.Current.MainPage.DisplayAlert("Error", exception.Message, "OK");
        }
    }
}
