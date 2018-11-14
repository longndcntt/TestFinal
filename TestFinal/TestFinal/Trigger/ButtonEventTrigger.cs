using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace TestFinal.Trigger
{
    public class ButtonEventTrigger : TriggerAction<Button>
    {
        protected async override void Invoke(Button sender)
        {
            if(sender != null)
            {
                await sender.ScaleTo(0.95, 50, Easing.CubicOut);
                await sender.ScaleTo(1, 50, Easing.CubicIn);
            }
        }
    }
}
