using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics.Text;
using System.Collections.ObjectModel;
using System.Xml;
using Algorithm;
using Microsoft.Maui;

namespace DesignSort;

public partial class FirstPage : ContentPage
{
    private List<string> _items = new List<string>();
    private Entry _entry;
    private Button _addButton;
    private Button _sortButton;
    private Button _deleteButton;
    private readonly string _methodName = "QuickSort";
    private Label _warringLabel  = new Label 
    { 
        Text = "",
        TextColor = Colors.Red,
        Margin = new Thickness(10, 0, 0, 0)
    };
    private ListView _listView;
    Label selectedItemHeader = new Label { FontSize = 18 };
    public FirstPage()
    {
        BackgroundColor = Colors.White;

        _entry = new Entry
        {
            TextColor = Colors.Black,
            Placeholder = "Enter your element here...",
            HeightRequest = 30
        };
        _entry.Completed += OnAddButtonClicked;

        _addButton = new Button
        {
            Text = "Add",
            BackgroundColor = Colors.White,
            TextColor = Colors.DarkBlue,
            WidthRequest = 100,
            HeightRequest = 40,
            HorizontalOptions = LayoutOptions.Start,
            BorderColor = Colors.DarkBlue,
            BorderWidth = 3,
            Margin = 5,
        };
        _addButton.Clicked += OnAddButtonClicked;


        var horisontalButtonsStack = new HorizontalStackLayout { Spacing = 370};
        var backButton = new Button
        {
            Text = "Go back",
            BackgroundColor = Colors.White,
            TextColor = Colors.Grey,
            HorizontalOptions = LayoutOptions.Start,
            Margin = 5
        };
        backButton.Clicked += ToStartPage;
        horisontalButtonsStack.Children.Add(backButton);


        var changeButton = new Button
        {
            Text = "Change input elements",
            BackgroundColor = Colors.White,
            TextColor = Colors.Grey,
            HorizontalOptions = LayoutOptions.Center,
            Margin = 5
        };
        changeButton.Clicked += OnChangeButtonClicked;
        horisontalButtonsStack.Children.Add(changeButton);


        _sortButton = new Button
        {
            Text = "Sort",
            BackgroundColor = Colors.MidnightBlue,
            TextColor = Colors.White,
            HorizontalOptions = LayoutOptions.End,
            Margin = 10,
            FontAttributes = FontAttributes.Bold,
            WidthRequest = 100,
            HeightRequest = 50,
        };
        _sortButton.Clicked += OnSortButtonClicked;
        horisontalButtonsStack.Children.Add(_sortButton);


        _listView = new ListView
        {
            ItemsSource = _items,
            RowHeight = 50,
            VerticalScrollBarVisibility = ScrollBarVisibility.Always,
            ItemTemplate = new DataTemplate(() =>
            {
                var nameLabel = new Label() { TextColor = Colors.DarkBlue };
                nameLabel.SetBinding(Label.TextProperty, ".");
                nameLabel.TextColor = Colors.Black;

                _deleteButton = new Button
                {
                    ImageSource = ImageSource.FromFile("krest.png")
                };
                _deleteButton.Clicked += OnDeleteButtonClicked;

                var layout = new StackLayout
                {
                    Padding = new Thickness(10, 5),
                    Orientation = StackOrientation.Horizontal,
                    Children = { nameLabel, _deleteButton }
                };

                return new ViewCell { View = layout };
            })
        };

        _listView.ItemSelected += ListView_ItemSelected;

        Content = new StackLayout { Children = { _entry, _warringLabel, _addButton, _listView, horisontalButtonsStack }, Padding = 7 };
    }

    private void OnAddButtonClicked(object sender, EventArgs e)
    {
        _warringLabel.Text = "";
        string text = _entry.Text;
        if (StartPage.SelectedElement == "числа" && !double.TryParse(text, out double value))
        {
            _warringLabel.Text = $"{text} не является числом! (дробные числа вводите через запятую)";
            return;
        }
        else if(StartPage.SelectedElement == "слова" && !text.All(Char.IsLetter))
        {
            _warringLabel.Text = $"{text} не является словом!";
            return;
        }
        else if (!string.IsNullOrEmpty(text))
        {
            _items.Add(text);
            _listView.ItemsSource = null;
            _listView.ItemsSource = _items;
            _entry.Text = string.Empty;
        }
    }

    private void OnChangeButtonClicked(object sender, EventArgs e)
    {
        _listView.ItemsSource = _items;
        _addButton.Clicked += OnAddButtonClicked;
        _deleteButton.Clicked += OnDeleteButtonClicked;
    }

    private void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var item = (sender as Button).BindingContext as string;
        _items.Remove(item);

        _listView.ItemsSource = null;
        _listView.ItemsSource = _items;
    }

    private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        selectedItemHeader.TextColor = Colors.Chartreuse;
    }

    private void OnSortButtonClicked(object sender, System.EventArgs e)
    {
        _warringLabel.Text = "";
        _addButton.Clicked -= OnAddButtonClicked;
        _deleteButton.Clicked -= OnDeleteButtonClicked;

        var instance = Activator.CreateInstance(StartPage.multiplierType);
        var methodInfo = StartPage.multiplierType.GetMethod(_methodName);

        if (StartPage.SelectedElement == "слова")
        { 
            var result = methodInfo?.Invoke(instance,
                new object[] { _items.ToArray() });
            _listView.ItemsSource = null;
            _listView.ItemsSource = (List<string>)result;
        }
        else
        {
            var result = methodInfo?.Invoke(instance,
                new object[] { _items.Select(double.Parse).ToArray() });
            _listView.ItemsSource = null;
            _listView.ItemsSource = (List<string>)result;
        }
    }


    private async void ToStartPage(object sender, EventArgs e)
    {

        await Navigation.PushModalAsync(new StartPage());
    }
}