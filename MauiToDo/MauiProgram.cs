using MauiToDo.Data;
using MauiToDo.ViewModels;

namespace MauiToDo;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			})
			.RegisterServices()
			.RegisterViewModels()
			.RegisterViews();

        return builder.Build();
	}

	public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
	{
        mauiAppBuilder.Services.AddTransient<ToDoItemsViewModel>();
		return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddSingleton<IToDoService, AzureToDoService>();
        return mauiAppBuilder;
    }

    public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
    {
        mauiAppBuilder.Services.AddTransient<ToDoItemsPage>();
        return mauiAppBuilder;
    }
}
