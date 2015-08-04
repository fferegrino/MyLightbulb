using MyLightbulb.Controls;
using MyLightbulb.WinPhone.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Xamarin.Forms;
using Xamarin.Forms.Platform.WinPhone;


[assembly: ExportRenderer(typeof(LightbulbToggleButton), typeof(LightbulbToggleButtonRenderer))]
namespace MyLightbulb.WinPhone.Controls
{

    public class LightbulbToggleButtonRenderer : ViewRenderer<LightbulbToggleButton, System.Windows.Controls.Button>
    {

        protected override void OnElementChanged(ElementChangedEventArgs<LightbulbToggleButton> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement != null || this.Element == null)
                return;
            var progress = new System.Windows.Controls.Button()
            {
            };
            var file = Element.ImageOn;

            progress.Background  = new System.Windows.Media.ImageBrush
            {
                ImageSource = new BitmapImage(new Uri(file.File, UriKind.RelativeOrAbsolute))
            }; 
            progress.Content = new System.Windows.Controls.TextBlock () 
            {
                Text="Hola miundo"
            }; 
            SetNativeControl(progress);
        }


    }   

}
