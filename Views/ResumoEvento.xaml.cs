namespace MauiAppEventos.Views;

public partial class ResumoEvento : ContentPage
{
    public ResumoEvento() => InitializeComponent();

    private async void Voltar(object sender, EventArgs e)
    {
        if (sender is not Button button || !button.IsEnabled) return;
        button.IsEnabled = false;
        try { await Navigation.PopAsync(); }
        finally { button.IsEnabled = true; }
    }
}
