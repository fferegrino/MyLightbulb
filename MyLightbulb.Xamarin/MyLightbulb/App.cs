using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace MyLightbulb
{
    public class App : Application
    {
        public App()
        {

            //var lightSwitch = new Button { Text = "On / Off" };
            var lightSwitch = new Switch { };
            var lightSwitchLabel = new Label
            {
                Text = "Bulb",
                HorizontalOptions = LayoutOptions.StartAndExpand,
                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label))
            };
            var readButton = new Button { Text = "Read", HorizontalOptions = LayoutOptions.FillAndExpand     };
            var writeButton = new Button { Text = "Write", HorizontalOptions = LayoutOptions.FillAndExpand };

            writeButton.Clicked += writeButton_Clicked;

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
                        new StackLayout{
                            Orientation = StackOrientation.Horizontal,
                            Children ={ readButton, writeButton }
                        },
                        new StackLayout{
                            Orientation = StackOrientation.Horizontal,
                            Children = {
                                lightSwitchLabel,
                                lightSwitch
                            }
                        }
					}
                }
            };
        }

        async void writeButton_Clicked(object sender, EventArgs e)
        {
            var s = new Sockets.Plugin.TcpSocketClient();
            await s.ConnectAsync("192.168.0.75", 3212);
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
