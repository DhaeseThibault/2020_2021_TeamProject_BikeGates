﻿using BikeGates.Models;
using BikeGates.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BikeGates.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EndScreenParkour : ContentPage
    {
        public static List<int> listTimesHere { get; set; } = new List<int>();


        public EndScreenParkour()
        {
            InitializeComponent();

            string Players = ChoicePlayer.listAmount[0];
            int amount = listTimesHere.Count();

            if (Players == "1")
            {
                if (amount == 0)
                {
                    lblUser.Text = ChoicePlayer.listNames[0].ToString();
                    frmReady.IsVisible = false;
                    lblEndgame.Text = "Finish";
                }

            }
            else if (Players == "2")
            {
                if (amount == 0)
                {
                    lblUser.Text = ChoicePlayer.listNames[0].ToString();
                    lblNext.Text = ChoicePlayer.listNames[1].ToString();
                }
                else if (amount == 1)
                {
                    lblUser.Text = ChoicePlayer.listNames[1].ToString();
                    frmReady.IsVisible = false;
                    lblEndgame.Text = "Finish";
                }

            }
            else if (Players == "3")
            {
                if (amount == 0)
                {
                    lblUser.Text = ChoicePlayer.listNames[0].ToString();
                    lblNext.Text = ChoicePlayer.listNames[1].ToString();
                }
                else if (amount == 1)
                {
                    lblUser.Text = ChoicePlayer.listNames[1].ToString();
                    lblNext.Text = ChoicePlayer.listNames[2].ToString();
                }
                else if (amount == 2)
                {
                    lblUser.Text = ChoicePlayer.listNames[2].ToString();
                    frmReady.IsVisible = false;
                    lblEndgame.Text = "Finish";
                }

            }
            else if (Players == "4")
            {
                if (amount == 0)
                {
                    lblUser.Text = ChoicePlayer.listNames[0].ToString();
                    lblNext.Text = ChoicePlayer.listNames[1].ToString();
                }
                else if (amount == 1)
                {
                    lblUser.Text = ChoicePlayer.listNames[1].ToString();
                    lblNext.Text = ChoicePlayer.listNames[2].ToString();
                }
                else if (amount == 2)
                {
                    lblUser.Text = ChoicePlayer.listNames[2].ToString();
                    lblNext.Text = ChoicePlayer.listNames[3].ToString();
                }
                else if (amount == 3)
                {
                    lblUser.Text = ChoicePlayer.listNames[3].ToString();
                    frmReady.IsVisible = false;
                    lblEndgame.Text = "Finish";
                }

            }
            else if (Players == "5")
            {
                if (amount == 0)
                {
                    lblUser.Text = ChoicePlayer.listNames[0].ToString();
                    lblNext.Text = ChoicePlayer.listNames[1].ToString();
                }
                else if (amount == 1)
                {
                    lblUser.Text = ChoicePlayer.listNames[1].ToString();
                    lblNext.Text = ChoicePlayer.listNames[2].ToString();
                }
                else if (amount == 2)
                {
                    lblUser.Text = ChoicePlayer.listNames[2].ToString();
                    lblNext.Text = ChoicePlayer.listNames[3].ToString();
                }
                else if (amount == 3)
                {
                    lblUser.Text = ChoicePlayer.listNames[3].ToString();
                    lblNext.Text = ChoicePlayer.listNames[4].ToString();
                }
                else if (amount == 4)
                {
                    lblUser.Text = ChoicePlayer.listNames[4].ToString();
                    frmReady.IsVisible = false;
                    lblEndgame.Text = "Finish";
                }
            }
        }

        protected async override void OnAppearing()
        {
            List<Parkour> listParkour = await BikeGatesRepository.GetParkourByName(lblUser.Text);
            lblScore.Text = listParkour[0].Points.ToString();
        }

            private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            if (lblEndgame.Text == "Finish")
            {
                listTimesHere.Clear();
                Navigation.PushAsync(new EndLeaderboard());
            }
            else
            {
                listTimesHere.Add(1);
                Navigation.PushAsync(new CountdownScreen());
                BikeGatesRepository.StartParkour(lblNext.Text);
            }
        }
    }
}