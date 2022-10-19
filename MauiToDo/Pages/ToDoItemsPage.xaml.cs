using MauiToDo.ViewModels;

namespace MauiToDo;

public partial class ToDoItemsPage : ContentPage
{
	public ToDoItemsPage(ToDoItemsViewModel viewModel)
	{
		BindingContext = viewModel;
        InitializeComponent();
    }

	private async void AddItemButton_Clicked(object sender, EventArgs e)
	{
		//TODO: See if this should be moved to the view model triggered using a command or is it better being part of the view
		string result = await DisplayPromptAsync("New To-Do Item", "Enter details", cancel: "CANCEL", keyboard: Keyboard.Text);
		await ((ToDoItemsViewModel)BindingContext).AddItem(result);
	}
}