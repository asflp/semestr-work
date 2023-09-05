using Algorithm;
using Microsoft.Maui.Graphics.Text;
using System.Reflection;

namespace DesignSort;

public partial class StartPage : ContentPage
{
   

    private Picker _languagePicker = new Picker 
    { 
        Title = "Тип сортируемых элементов",
        HorizontalOptions = LayoutOptions.Center,
        VerticalOptions = LayoutOptions.Center,
        TitleColor = Colors.MidnightBlue,
        BackgroundColor = Colors.MidnightBlue
    };

    private Label _header;
    private FileResult _fileResult;
    internal static Type multiplierType;
    public static string SelectedElement;
    private Button _startButton;
    private Button _selectDllButton;
    internal MethodInfo methodInfo;
    private Label _textSwitch;
    Switch _switcher;

    public StartPage()
    {
        _header = new Label
        {
            Text = "Выберите тип элементов",
            FontSize = 25,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            TextColor = Colors.MidnightBlue,
            Margin = 15
        };

    UseReflection();
        BackgroundColor = Colors.White;
        _languagePicker.ItemsSource = new string[]{ "числа", "слова"};
        _languagePicker.SelectedIndexChanged += PickerSelectedIndexChanged;

        _switcher = new Switch  
        {
            IsToggled = true,
            OnColor = Colors.HotPink,
            Margin = 5
        };
        _switcher.Toggled += switcher_Toggled;

        _textSwitch = new Label
        {
            Text = "сменить тему",
            TextColor = Colors.MidnightBlue
        };

        HorizontalStackLayout swithText = new HorizontalStackLayout
        {
            Margin = 25,
            Children = { _textSwitch, _switcher },
            HorizontalOptions = LayoutOptions.Start
        };

        _startButton = new Button
        {
            Text = "Start",
            FontSize = 22,
            BackgroundColor = Colors.White,
            TextColor = Colors.MidnightBlue,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Margin = 20,
            BorderColor = Colors.MidnightBlue,
            BorderWidth = 2
        };
        _startButton.Clicked += ToFirstPage;


        _selectDllButton = new Button
        {
            Text = "Выбрать DLL",
            TextColor = Colors.MidnightBlue
        };

        _selectDllButton.Clicked += OnSelectDllButtonClicked;

        Content = new StackLayout { Children = { swithText, _header, _languagePicker, _startButton, _selectDllButton }, Padding = 8 };
    }

    void PickerSelectedIndexChanged(object sender, EventArgs e)
    {
        _header.Text = $"Вы выбрали: {_languagePicker.SelectedItem}";
        _header.TextColor = Colors.MidnightBlue;
        SelectedElement = _languagePicker.SelectedItem.ToString();
    }

    private async void ToFirstPage(object sender, EventArgs e)
    {
        if (_languagePicker.SelectedItem == null)
        {
            _header.Text = "Выберите тип элемента!";
            _header.TextColor = Colors.Red;
            _languagePicker.BackgroundColor = Colors.Red;
            return;
        }
        else
        {
            await Navigation.PushModalAsync(new FirstPage());
        }
    }

    private async void OnSelectDllButtonClicked(object sender, EventArgs e)
    {
        _fileResult = await FilePicker.PickAsync();
    }

    private async void UseReflection()
    {
        if (_fileResult != null && SelectedElement != null)
        {
            var filePath = _fileResult.FullPath;
            if (filePath.EndsWith(".dll"))
            {
                var assembly = Assembly.LoadFrom(filePath);
                var multiplierType = assembly.GetTypes().FirstOrDefault(t =>
                {
                    var interfaces = t.GetInterfaces();
                    foreach (var iface in interfaces)
                    {
                        if (iface.IsGenericType && iface.GetGenericTypeDefinition() == typeof(IQuickSort<>))
                        {
                            var genericArguments = iface.GetGenericArguments();
                            return genericArguments.Length == 1 && genericArguments[0] == typeof(double);
                        }
                    }
                    return false;
                }); 

                if(multiplierType == null)
                {
                    await DisplayAlert("Type error", "No type implementing IQuickSort found.", "ok");
                }
            }
            else if(SelectedElement == null && _fileResult != null)
            {
                await DisplayAlert("", "Select type of elements", "ok");
            }
            else
            {
                await DisplayAlert("", "Invalid file", "ok");
            }
        }
        else
        {
            await DisplayAlert("File error", "DLL file not found", "ok");
        }
    }

    private async void switcher_Toggled(object sender, ToggledEventArgs e)
    {
        if(!_switcher.IsToggled)
        {
            this.BackgroundColor = Colors.Pink;
            _header.TextColor = Colors.DeepPink;
            _languagePicker.TitleColor = Colors.DeepPink;
            _languagePicker.BackgroundColor = Colors.DeepPink;
            _startButton.BackgroundColor = Colors.DeepPink;
            _startButton.TextColor = Colors.Pink;
            _startButton.BorderColor = Colors.DeepPink;
            _selectDllButton.TextColor = Colors.DeepPink;
            _selectDllButton.BackgroundColor = Colors.Pink;
            _textSwitch.TextColor = Colors.DeepPink;
            _switcher.OnColor = Colors.Pink;

        }
        else
        {
            this.BackgroundColor = Colors.White;
            _header.TextColor = Colors.MidnightBlue;
            _languagePicker.TitleColor = Colors.MidnightBlue;
            _languagePicker.BackgroundColor = Colors.MidnightBlue;
            _startButton.BackgroundColor = Colors.White;
            _startButton.TextColor = Colors.MidnightBlue;
            _startButton.BorderColor = Colors.MidnightBlue;
            _selectDllButton.TextColor = Colors.MidnightBlue;
            _selectDllButton.BackgroundColor = Colors.White;
            _switcher.OnColor = Colors.White;
            _textSwitch.TextColor = Colors.MidnightBlue;
        }
    }
}