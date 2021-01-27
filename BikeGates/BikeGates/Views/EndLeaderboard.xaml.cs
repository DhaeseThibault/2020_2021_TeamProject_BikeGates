using BikeGates.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BikeGates.Repositories;

namespace BikeGates.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EndLeaderboard : ContentPage
    {
        private ObservableCollection<ItemsList> thpendingordersP;
        private ObservableCollection<ItemsList> pendingordersP;

        private ObservableCollection<ItemsListTimeRace> thpendingordersT;
        private ObservableCollection<ItemsListTimeRace> pendingordersT;
        int x = 0;
        public EndLeaderboard()
        {
            InitializeComponent();

            
        }

        protected async override void OnAppearing()
        {
            //   await _connection.CreateTableAsync<TH_PENDING_ORDERS>();
            //   var porders = await _connection.Table<TH_PENDING_ORDERS>().ToListAsync();
            //thpendingorders = new ObservableCollection<ItemsList>();
            //thpendingorders.Add(new ItemsList() { Name = "Thibault", Score = 1 });
            //thpendingorders.Add(new ItemsList() { Name = "Henri", Score = 1 });
            //thpendingorders.Add(new ItemsList() { Name = "Niels", Score = 2 });
            //thpendingorders.Add(new ItemsList() { Name = "Robin", Score = 3 });
            //thpendingorders.Add(new ItemsList() { Name = "Laurens", Score = 4 });

            //pendingorders = new ObservableCollection<ItemsList>();
            //lvwLeaderboard.ItemsSource = pendingorders;
            //foreach (var i in thpendingorders)
            //{
            //    int z = 0;
            //    string dno = i.Name;
            //    int tno = i.Score;

            //    if (tno > 0)
            //    {
            //        //await Task.Delay(1000);
            //        pendingorders.Add(new ItemsList { Name = dno, Score = tno });
            //    }
            //}
            //base.OnAppearing();





            if (ChoiceGamemode.listMode[0] == "Parkour")
            {
                List<Parkour> listParkour = await BikeGatesRepository.GetParkour();
                List<Parkour> sortedListParkour = listParkour.OrderBy(p => p.timestamp).ToList();
                sortedListParkour.Reverse();

                // aantal spelers die meespelen
                int amount = Convert.ToInt32(ChoicePlayer.listAmount[0]);

                //in de sorted list de eerste 5 pakken
                List<Parkour> listAmountOfPlayers = sortedListParkour.GetRange(0, amount);

                List<Parkour> sortedListByPoints = listAmountOfPlayers.OrderBy(l => l.Points).ToList();
                sortedListByPoints.Reverse();

                pendingordersP = new ObservableCollection<ItemsList>();

                int position = 1;


                for (int i = 0; i < amount; i++)
                {
                    thpendingordersP = new ObservableCollection<ItemsList>();
                    thpendingordersP.Add(new ItemsList() { Rank = Convert.ToString(position), Name = sortedListByPoints[i].Name, Score = sortedListByPoints[i].Points });

                    foreach (var u in thpendingordersP)
                    {
                        string name = u.Name;
                        int score = u.Score;
                        string rank = u.Rank;

                        pendingordersP.Add(new ItemsList { Rank = rank, Name = name, Score = score });
                        position++;

                    }
                }
                lvwLeaderboard.ItemsSource = pendingordersP;

                base.OnAppearing();
                lvwLeaderboard.IsVisible = true;
                lvwLeaderboardT.IsVisible = false;
                lblScore.Text = "Points";
            }
            else if (ChoiceGamemode.listMode[0] == "Time Race")
            {
                List<TimeRace> listTimeRace = await BikeGatesRepository.GetTimeRace();

                List<TimeRace> sortedListTimeRace = listTimeRace.OrderBy(p => p.timestamp).ToList();
                sortedListTimeRace.Reverse();

                // aantal spelers die meespelen
                int amount = Convert.ToInt32(ChoicePlayer.listAmount[0]);

                //in de sorted list de eerste 5 pakken
                List<TimeRace> listAmountOfPlayers = sortedListTimeRace.GetRange(0, amount);

                List<TimeRace> sortedListByPoints = listAmountOfPlayers.OrderBy(l => l.TotalTime).ToList();

                pendingordersT = new ObservableCollection<ItemsListTimeRace>();

                int position = 1;

                for (int i = 0; i < amount; i++)
                {
                    thpendingordersT = new ObservableCollection<ItemsListTimeRace>();
                    thpendingordersT.Add(new ItemsListTimeRace() { Rank = Convert.ToString(position), Name = sortedListByPoints[i].Name, Time = sortedListByPoints[i].TotalTime.ToString() });

                    foreach (var u in thpendingordersT)
                    {
                        string name = u.Name;
                        string time = u.Time;
                        string rank = u.Rank;

                        TimeSpan convertedTime = TimeSpan.FromSeconds(Convert.ToDouble(time));
                        string seconds = convertedTime.ToString(@"mm\:ss");

                        pendingordersT.Add(new ItemsListTimeRace { Rank = position.ToString(), Name = name, Time = seconds });
                        position++;

                    }
                }
                lvwLeaderboardT.ItemsSource = pendingordersT;

                base.OnAppearing();
                lvwLeaderboard.IsVisible = false;
                lvwLeaderboardT.IsVisible = true;
                lblScore.Text = "Time";
            }
        }

        private void OpenMainPage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }
    }
}