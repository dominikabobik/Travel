﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Travel
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {

        public static DataBaseService Service;
        public static ObservableCollection<City> Cities { get; set; }
        
        public MainPage()
        {
            InitializeComponent();
            Service = new DataBaseService();
            Cities = new ObservableCollection<City>();
            listView.ItemsSource = Cities;
        }
        protected override async void OnAppearing()
        {
            await Service.Init();
            List<City> list = await Service.connection.Table<City>().ToListAsync();
            Cities = new ObservableCollection<City>(list);
            listView.ItemsSource = Cities;
            base.OnAppearing();
        }

        public void LoadData()
        {
            listView.ItemsSource = Cities;
        }

        private ObservableCollection<City> SearchData(string searchText = null)
        {
            if (String.IsNullOrWhiteSpace(searchText))
            {
                return Cities;
            }

            IEnumerable<City> temp = Cities.Where(c => c.Name.StartsWith(searchText));
            ObservableCollection<City> cities = new ObservableCollection<City>(temp);

            return cities;
        }

        private void Add_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddPage());
        }

        private void Delete_Clicked(object sender, EventArgs e)
        {
            City city = (sender as MenuItem).CommandParameter as City;
            Service.DeleteEntry(city.Id);

        }

        private void listView_Refreshing(object sender, EventArgs e)
        {
            //Refresh by calling method (To Do)
            listView.EndRefresh();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tempCities = SearchData(e.NewTextValue);
            listView.ItemsSource = tempCities;
        }

        private void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {

        }

        private void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}