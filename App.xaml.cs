namespace MauiAppEventos;

public partial class App : Application
{
    public App() => InitializeComponent();

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var navigation = new NavigationPage(new Views.CadastroEvento())
        {
            BarBackgroundColor = Color.FromArgb("#171530"),
            BarTextColor = Colors.White
        };
        return new Window(navigation)
        {
            Title = "MauiAppEventos", Width = 600, Height = 850,
            MinimumWidth = 340, MinimumHeight = 480
        };
    }
}
