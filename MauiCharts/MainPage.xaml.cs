using MauiCharts.Charts;

namespace MauiCharts;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();

        BindingContext = new Dictionary<string, float>()
            {
                {"Apples",25},
                {"Bananas",13},
                {"Strawberries",25},
                {"Blueberries", 53},
                {"Oranges", 14},
                {"Grapes", 52},
                {"Watermelons", 15},
                {"Pears",34 },
                {"Cantalopes", 67},
                {"Citrus",53 },
                {"Starfruit", 43},
                {"Papaya", 22},
                 {"Papassya", 22},
            };
    }
}