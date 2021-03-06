﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OpenweatherApps
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static HttpClient client;
        static ForecastKota forecast;
        string kota;

        public MainWindow()
        {
            forecast = null;
            client = new HttpClient();
            InitializeComponent();
            getForecast();
         
        }

        private async void btnSubmitKota_Click(object sender, RoutedEventArgs e)
        {
            
            //String isi = await getTextKota();
            if (textBox.Text!= null)
            {
                getForecast();
                //setKota();
                //String isi = await getTextKota();
                string isi = "id " + forecast.id.ToString() + "\n name : " + forecast.name.ToString()
       + "\n cod : " + forecast.cod.ToString() + "\ntemp: " + kelvinToCelcius(forecast.main.temp)+" Celcius";

                textBlock.Text = isi;

                imageWeather.Source = getImage(forecast.weather[0].icon);
            }
            else
            {
                textBlock.Text = "Masukkan dahulu nama kota Anda";
            }

        }
        private async Task<string> getTextKota()
        {
            await getForecast();
            return "id " + forecast.id.ToString() + "\n name : " + forecast.name.ToString()
       + "\n cod : " + forecast.cod.ToString() + "temp: " + kelvinToCelcius(forecast.main.temp);

        }

        BitmapImage getImage(string iconPath)
        {
            var image = new Image();
            var imagePath = "http://openweathermap.org/img/w/" + iconPath + ".png";

            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            bitmap.UriSource = new Uri(imagePath, UriKind.Absolute);
            bitmap.EndInit();

            return bitmap;
        }

        string kelvinToCelcius(String kelvin)
        {

            Double kelvinInt = Convert.ToDouble(kelvin);

            Double celcius = kelvinInt - 273.15;

            return Convert.ToString(celcius);
        }

         async Task getForecast()
        {
            client.BaseAddress = new Uri("http://api.openweathermap.org/data/2.5/weather");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //string kota = "Malang";
            setKota();
            /* string kota = await getKota();*/
            string kota = "Malang";
            string key = "{YOUR API KEY}";
            string path = "?q=" + kota + "&APPID=" + key;

            try
            {
                //GET
                forecast = await getForecasteKotaAsync(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        async void setKota() { kota = textBox.Text; }
        async Task<string> getKota() { return textBox.Text; }

        static async Task<ForecastKota> getForecasteKotaAsync(String path)
        {   
            ForecastKota forecastKota = null;
            HttpResponseMessage respon = await client.GetAsync(path);
            if (respon.IsSuccessStatusCode)
            {
                forecastKota = await respon.Content.ReadAsAsync<ForecastKota>();
            }
            return forecastKota;
        }


    }
}
