using System.Globalization;
using MauiAppEventos.Models;

namespace MauiAppEventos.Views;

public partial class CadastroEvento : ContentPage
{
    private int numeroParticipantes = 1;
    private bool navegando;

    public CadastroEvento()
    {
        InitializeComponent();
        dtp_inicio.MaximumDate = new DateTime(2100, 12, 30);
        dtp_termino.MaximumDate = new DateTime(2100, 12, 31);
        dtp_inicio.Date = DateTime.Today;
        AtualizarTermino();
        AtualizarParticipantes();
    }

    private void AumentarParticipantes(object sender, EventArgs e)
    {
        if (numeroParticipantes < int.MaxValue) numeroParticipantes++;
        AtualizarParticipantes();
    }

    private void DiminuirParticipantes(object sender, EventArgs e)
    {
        if (numeroParticipantes > 1) numeroParticipantes--;
        AtualizarParticipantes();
    }

    private void AtualizarParticipantes()
    {
        lbl_participantes.Text = numeroParticipantes.ToString(CultureInfo.CurrentCulture);
        btn_diminuir.IsEnabled = numeroParticipantes > 1;
        btn_aumentar.IsEnabled = numeroParticipantes < int.MaxValue;
    }

    private void InicioSelecionado(object sender, DateChangedEventArgs e) => AtualizarTermino();

    private void AtualizarTermino()
    {
        // DateSelected pode ocorrer durante a inicialização do XAML.
        if (dtp_termino is null || dtp_inicio.Date is not DateTime inicio) return;
        var minimo = inicio.Date.AddDays(1);
        dtp_termino.MinimumDate = minimo;
        if (dtp_termino.Date is not DateTime termino || termino.Date < minimo)
            dtp_termino.Date = minimo;
    }

    private async void VerResumo(object sender, EventArgs e)
    {
        if (navegando) return;
        var nome = ent_nome.Text?.Trim();
        var local = ent_local.Text?.Trim();
        string? erro = null;
        var estilo = NumberStyles.AllowLeadingSign | NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingWhite | NumberStyles.AllowTrailingWhite;
        bool custoValido = double.TryParse(ent_custo.Text, estilo, CultureInfo.GetCultureInfo("pt-BR"), out double custo);

        if (string.IsNullOrWhiteSpace(nome)) erro = "Informe o nome do evento.";
        else if (string.IsNullOrWhiteSpace(local)) erro = "Informe o local do evento.";
        else if (numeroParticipantes < 1) erro = "Informe pelo menos 1 participante.";
        else if (!custoValido || !double.IsFinite(custo) || custo < 0)
            erro = "Informe um custo válido, maior ou igual a zero. Use vírgula para os centavos.";
        else if (dtp_inicio.Date is null || dtp_termino.Date is null || dtp_termino.Date.Value.Date <= dtp_inicio.Date.Value.Date)
            erro = "Selecione as datas. O término deve ser posterior ao início.";

        if (erro is not null)
        {
            await DisplayAlertAsync("Confira os dados", erro, "OK");
            return;
        }

        var evento = new Evento
        {
            Nome = nome!, Local = local!, NumeroParticipantes = numeroParticipantes,
            DataInicio = dtp_inicio.Date!.Value.Date,
            DataTermino = dtp_termino.Date!.Value.Date,
            CustoPorParticipante = custo
        };
        if (!double.IsFinite(evento.CustoTotal))
        {
            await DisplayAlertAsync("Confira os dados", "O custo total excede o limite numérico. Reduza o custo ou a quantidade.", "OK");
            return;
        }
        navegando = true;
        btn_resumo.IsEnabled = false;
        try
        {
            await Navigation.PushAsync(new ResumoEvento { BindingContext = evento });
        }
        finally
        {
            navegando = false;
            btn_resumo.IsEnabled = true;
        }
    }
}
