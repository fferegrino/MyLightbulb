using MyLightbulb.Controls;
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
        static Switch lightSwitch;
        public App()
        {
            lightSwitch = new Switch { };
            //var lightSwitch = new Button { Text = "On / Off" };
            var lightSwitchLabel = new Label
            {
                Text = "Bulb",
                HorizontalOptions = LayoutOptions.StartAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
            };

            LightbulbInterface li = new LightbulbInterface("192.168.0.75", 5436);

            var readButton = new Button { Image = "on.png", VerticalOptions = LayoutOptions.StartAndExpand, HeightRequest = 240 };
            toggleEnabled = true;
            lightSwitch.Toggled += (s, a) =>
            {
                if (toggleEnabled)
                {
                    li.SetLampStatus(lightSwitch.IsToggled);
                }
            };

            readButton.Clicked += async (s, a) =>
            {
                toggleEnabled = false;

                lightSwitch.IsToggled = await li.GetLampstatus();

                toggleEnabled = true;
            };

            var netduinoIp = new Entry { Placeholder = "127.0.0.1", Keyboard = Keyboard.Telephone };

            MainPage = new ContentPage
            {
                Content = new StackLayout
                {
                    Children = {
						new Label {
							Text = "IP del Netduino",
                            //FontSize = Font.SystemFontOfSize(NamedSize.Medium)
						},
                        netduinoIp,
                        new LightbulbToggleButton(){ ImageOn="on.png"}
					}
                }
            };
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
