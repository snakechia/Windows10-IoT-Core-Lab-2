using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.Gpio;
using Windows.UI.Popups;
using Windows.UI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Lab2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        GpioController gpio;
        GpioPin inputpin;
        GpioPin ledpin;
        GpioPinValue ledpinvalue;

        public MainPage()
        {
            this.InitializeComponent();

            initializeGPIO();
            inputpin.ValueChanged += Inputpin_ValueChanged;
        }


        private void Inputpin_ValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            if (args.Edge == GpioPinEdge.FallingEdge)
            {
                ledpinvalue = (ledpinvalue == GpioPinValue.Low) ?
                    GpioPinValue.High : GpioPinValue.Low;
                ledpin.Write(ledpinvalue);
            }
        }
        
        private async void initializeGPIO()
        {
            gpio = GpioController.GetDefault();

            if (gpio == null)
            {
                MessageDialog dialog = new MessageDialog("This device doesn't have any GPIO Controller.");
                await dialog.ShowAsync();
                return;
            }
            
            // define and enable led pin
            ledpin = gpio.OpenPin(18);
            ledpin.SetDriveMode(GpioPinDriveMode.Output);

            // define and enable push button pin
            inputpin = gpio.OpenPin(23);
            
            if (inputpin.IsDriveModeSupported(GpioPinDriveMode.InputPullUp))
                inputpin.SetDriveMode(GpioPinDriveMode.InputPullUp);
            else
                inputpin.SetDriveMode(GpioPinDriveMode.Input);

            inputpin.DebounceTimeout = TimeSpan.FromMilliseconds(50);
            
        }
    }
}
