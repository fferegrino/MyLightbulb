using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace MyLightbulb
{
    public class App : Application
    {
        static bool toggleEnabled;
        static Entry netduinoIpEntry;
        static Entry netduinoPortEntry;
        static Switch lightSwitch;
        public App()
        {

            toggleEnabled = true;

            lightSwitch = new Switch
            {
                //RotationX = 90,
                Scale = 2,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            lightSwitch.Toggled += (s, a) =>
            {
                if (toggleEnabled)
                {
                    LightbulbInterface li = new LightbulbInterface(netduinoIpEntry.Text, Int32.Parse(netduinoPortEntry.Text));
                    li.SetLightSwitchStatus(lightSwitch.IsToggled);
                }
            };


            netduinoIpEntry = new Entry
            {
#if DEBUG
                // Hardcoded for testing purposes
                Text = "192.168.0.75",
#endif
                Placeholder = "127.0.0.1",
                Keyboard = Keyboard.Telephone
            };
            netduinoPortEntry = new Entry
            {
#if DEBUG
                // Hardcoded for testing purposes
                Text = "5436",
#endif
                Placeholder = "1234",
                Keyboard = Keyboard.Numeric
            };

            var refreshToobarButton = new ToolbarItem { Text = "Refresh" };
            refreshToobarButton.Clicked += async (s, a) =>
            {
                toggleEnabled = false;
                LightbulbInterface li = new LightbulbInterface(netduinoIpEntry.Text, Int32.Parse(netduinoPortEntry.Text));
                lightSwitch.IsToggled = await li.GetLightSwitchStatus();
                toggleEnabled = true;
            };


            MainPage = new NavigationPage(new ContentPage()
            {
                ToolbarItems =
                {
                    refreshToobarButton
                },
                Content = new StackLayout
                {
                    Children = {
						new Label {
							Text = "Netduino's IP"
						},
                        netduinoIpEntry,
						new Label {
							Text = "Netduino's port"
						},
                        netduinoPortEntry,
                        lightSwitch
					}
                }
            });
        }


        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
