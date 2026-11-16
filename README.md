# MauiAppEventos — Agenda 15

Aplicativo de cadastro de eventos para a Agenda 15 de Desenvolvimento de Sistemas II, desenvolvido por Pedro Akio Sakuma com C#, XAML, .NET MAUI e .NET 10 para Windows.

## Funcionamento

Preencha nome, local, participantes, início, término e custo individual; clique em **Ver resumo**. A segunda página apresenta todos os dados e destaca o custo total. **Voltar** permite revisar os dados preservados no formulário. Os dados ficam somente em memória; não há banco de dados nem gravação permanente.

O contador usa botões próprios de incremento e decremento, com mínimo de 1 e estados Normal, PointerOver, Pressed e Disabled. Campos obrigatórios, datas e custo são validados com alertas simples. O custo usa `double.TryParse`, aceita zero e vírgula decimal (ex.: `25,50`), sem separador de milhar. Valores inválidos ou não finitos são rejeitados.

## Model e conceitos

`Models/Evento.cs` possui `Nome`, `Local`, `DataInicio`, `DataTermino`, `NumeroParticipantes` e `CustoPorParticipante`.

- `DateTime` representa as datas, sem horário. Os valores anuláveis do `DatePicker` são verificados antes de acessar `.Value`.
- `TimeSpan Duracao` calcula `DataTermino - DataInicio`; `DuracaoDias` retorna `Duracao.Days`.
- `CustoTotal` calcula `NumeroParticipantes * CustoPorParticipante` na model. A duração não multiplica o custo.
- Ao alterar o início, `dtp_termino.MinimumDate` passa a ser o dia seguinte e a seleção final é corrigida quando necessário. O início é limitado a 30/12/2100 e o término a 31/12/2100.
- `NavigationPage` é criada em `App.CreateWindow`. O cadastro instancia `Evento` e chama `Navigation.PushAsync(new ResumoEvento { BindingContext = evento })`.
- A página de resumo recebe o objeto por `BindingContext`, com bindings tipados por `x:DataType` e `StringFormat` para datas (`dd/MM/yyyy`), duração e moeda (`C`). A cultura é `pt-BR` para valores em reais.

## Interface

Degradê azul escuro/roxo, destaque violeta, cards claros arredondados, Open Sans, estilos globais no `App.xaml`, `ScrollView` e largura máxima de 600 para adaptação à janela.

## Estrutura

```text
MauiAppEventos/
|-- Models/Evento.cs
|-- Views/
|   |-- CadastroEvento.xaml / .xaml.cs
|   `-- ResumoEvento.xaml / .xaml.cs
|-- Platforms/Windows/
|-- Resources/ (AppIcon, Fonts, Splash)
|-- App.xaml / App.xaml.cs
|-- MauiProgram.cs
|-- MauiAppEventos.csproj
`-- MauiAppEventos.sln
```

## Execução no Windows com .NET 10

Instale o SDK .NET 10, o Windows SDK e a carga de trabalho MAUI para Windows (pelo instalador do Visual Studio ou `dotnet workload install maui-windows`). No diretório do projeto:

```powershell
dotnet restore MauiAppEventos.sln
dotnet build MauiAppEventos.sln -c Debug
dotnet run --project MauiAppEventos.csproj -f net10.0-windows10.0.19041.0
```

Também é possível abrir `MauiAppEventos.sln` no Visual Studio compatível com .NET 10 e executar com Windows Machine. O projeto usa `WindowsPackageType=None`, permitindo execução local sem instalação de pacote MSIX.

## Capturas de tela

As imagens do funcionamento serão adicionadas após os testes finais.

## Referência da base

A estrutura visual e os recursos compartilhados foram adaptados do [projeto anterior da Agenda 14](https://github.com/akioShaolin/MauiAppHotel), de Pedro Akio Sakuma, baseado no material de Tiago Antonio da Silva. O cadastro, a model e o resumo desta atividade são dedicados a eventos.
