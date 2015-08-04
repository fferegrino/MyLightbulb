using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MyLightbulb.Controls
{
    public class LightbulbToggleButton : Button
    {
        public bool IsOn { get; set; }

        //Bindable property for the progress color
        public static readonly BindableProperty ProgressColorProperty =
          BindableProperty.Create<LightbulbToggleButton, FileImageSource>(p => p.ImageOn, null);
        //Gets or sets the color of the progress bar
        public FileImageSource ImageOn
        {
            get { return (FileImageSource)GetValue(ProgressColorProperty); }
            set { SetValue(ProgressColorProperty, value); }
        }



    }
}
