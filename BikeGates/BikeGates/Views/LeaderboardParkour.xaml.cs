using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BikeGates.Models;
using BikeGates.Repositories;

namespace BikeGates.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeaderboardParkour : ContentPage
    {
        public LeaderboardParkour()
        {
            InitializeComponent();


        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }

        //PARKOUR
        private ObservableCollection<ItemsList> thpendingordersP;
        private ObservableCollection<ItemsList> pendingordersP;

        //TIMERACE
        private ObservableCollection<ItemsListTimeRace> thpendingordersT;
        private ObservableCollection<ItemsListTimeRace> pendingordersT;

        protected async override void OnAppearing()
        {
            //GEDEELTE VOOR LEADERBOARD PARKOUR
            List<Parkour> listParkour = await BikeGatesRepository.GetParkour();
            List<Parkour> sortedlistParkour = listParkour.OrderBy(o => o.Points).ToList();
            sortedlistParkour.Reverse();

            thpendingordersP = new ObservableCollection<ItemsList>();
            thpendingordersP.Add(new ItemsList() { Rank = "1.", Name = sortedlistParkour[0].Name, Score = sortedlistParkour[0].Points });
            thpendingordersP.Add(new ItemsList() { Rank = "2.", Name = sortedlistParkour[1].Name, Score = sortedlistParkour[1].Points });
            thpendingordersP.Add(new ItemsList() { Rank = "3.", Name = sortedlistParkour[2].Name, Score = sortedlistParkour[2].Points });
            thpendingordersP.Add(new ItemsList() { Rank = "4.", Name = sortedlistParkour[3].Name, Score = sortedlistParkour[3].Points });
            thpendingordersP.Add(new ItemsList() { Rank = "5.", Name = sortedlistParkour[4].Name, Score = sortedlistParkour[4].Points });
            thpendingordersP.Add(new ItemsList() { Rank = "6.", Name = sortedlistParkour[5].Name, Score = sortedlistParkour[5].Points });
            thpendingordersP.Add(new ItemsList() { Rank = "7.", Name = sortedlistParkour[6].Name, Score = sortedlistParkour[6].Points });
            thpendingordersP.Add(new ItemsList() { Rank = "8.", Name = sortedlistParkour[7].Name, Score = sortedlistParkour[7].Points });
            thpendingordersP.Add(new ItemsList() { Rank = "9.", Name = sortedlistParkour[8].Name, Score = sortedlistParkour[8].Points });
            thpendingordersP.Add(new ItemsList() { Rank = "10.", Name = sortedlistParkour[9].Name, Score = sortedlistParkour[9].Points });
            thpendingordersP.Add(new ItemsList() { Rank = "11.", Name = sortedlistParkour[10].Name, Score = sortedlistParkour[10].Points });
            pendingordersP = new ObservableCollection<ItemsList>();

            int x = 0;
            foreach (var i in thpendingordersP)
            {
                string name = i.Name;
                string rank = i.Rank;
                int points = i.Score;
                if (x < 10)
                {
                    pendingordersP.Add(new ItemsList { Rank = rank, Name = name, Score = points });
                }
                x++;
            }
            lvwLeaderboard.ItemsSource = pendingordersP;
            base.OnAppearing();


            //GEDEELTE VOOR LEADERBOARD TIMERACE

            List<TimeRace> listTimeRace = await BikeGatesRepository.GetTimeRace();
            List<TimeRace> sortedlistTimeRace = listTimeRace.OrderBy(z => z.TotalTime).ToList();

            thpendingordersT = new ObservableCollection<ItemsListTimeRace>();
            thpendingordersT.Add(new ItemsListTimeRace() { Rank = "1.", Name = sortedlistTimeRace[0].Name, Time = sortedlistTimeRace[0].TotalTime.ToString() });
            thpendingordersT.Add(new ItemsListTimeRace() { Rank = "2.", Name = sortedlistTimeRace[1].Name, Time = sortedlistTimeRace[1].TotalTime.ToString() });
            thpendingordersT.Add(new ItemsListTimeRace() { Rank = "3.", Name = sortedlistTimeRace[2].Name, Time = sortedlistTimeRace[2].TotalTime.ToString() });
            thpendingordersT.Add(new ItemsListTimeRace() { Rank = "4.", Name = sortedlistTimeRace[3].Name, Time = sortedlistTimeRace[3].TotalTime.ToString() });
            thpendingordersT.Add(new ItemsListTimeRace() { Rank = "5.", Name = sortedlistTimeRace[4].Name, Time = sortedlistTimeRace[4].TotalTime.ToString() });
            thpendingordersT.Add(new ItemsListTimeRace() { Rank = "6.", Name = sortedlistTimeRace[5].Name, Time = sortedlistTimeRace[5].TotalTime.ToString() });
            thpendingordersT.Add(new ItemsListTimeRace() { Rank = "7.", Name = sortedlistTimeRace[6].Name, Time = sortedlistTimeRace[6].TotalTime.ToString() });
            thpendingordersT.Add(new ItemsListTimeRace() { Rank = "8.", Name = sortedlistTimeRace[7].Name, Time = sortedlistTimeRace[7].TotalTime.ToString() });
            thpendingordersT.Add(new ItemsListTimeRace() { Rank = "9.", Name = sortedlistTimeRace[8].Name, Time = sortedlistTimeRace[8].TotalTime.ToString() });
            thpendingordersT.Add(new ItemsListTimeRace() { Rank = "10.", Name = sortedlistTimeRace[9].Name, Time = sortedlistTimeRace[9].TotalTime.ToString() });
            thpendingordersT.Add(new ItemsListTimeRace() { Rank = "11.", Name = sortedlistTimeRace[10].Name, Time = sortedlistTimeRace[10].TotalTime.ToString() });
            pendingordersT = new ObservableCollection<ItemsListTimeRace>();

            int y = 0;
            foreach (var i in thpendingordersT)
            {
                string name = i.Name;
                string rank = i.Rank;
                string points = i.Time;


                TimeSpan time = TimeSpan.FromSeconds(Convert.ToDouble(points));
                string seconds = time.ToString(@"mm\:ss");

                if (y < 10)
                {
                    pendingordersT.Add(new ItemsListTimeRace { Rank = rank, Name = name, Time = seconds });
                }
                y++;
            }
            lvwLeaderboardTimeRace.ItemsSource = pendingordersT;
            base.OnAppearing();

        }

        private void pckChoosen_Gamemode(object sender, EventArgs e)
        {
            if (pckGamemode.SelectedIndex == 0)
            {
                lvwLeaderboard.IsVisible = true;
                lvwLeaderboardTimeRace.IsVisible = false;
                lblPoints.Text = "Points";
                
            }
            else if (pckGamemode.SelectedIndex == 1)
            {
                lvwLeaderboard.IsVisible = false;
                lvwLeaderboardTimeRace.IsVisible = true;
                lblPoints.Text = "Time";
            }
        }
    }
}