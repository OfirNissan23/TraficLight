﻿using System.Timers;
using TraficLight.Models;

namespace TraficLight.ModelsLogic
{
    internal class TraficLight : TraficLightModel
    {
        public override string SwitchChangeLightText => isAutoChange ? Strings.StopAutoChangeLightText : Strings.StartAutoChangeLightText;
        public override string LightImage => lightImage.GetLightImage(currentState);
        public TraficLight()
        {
            timer.Elapsed += OnTimerElapsed;
        }
        private void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            ChangeLight();
        }
        public override void ChangeLight()
        {
            if (currentState == TraficLightState.Red)
            {
                currentState = TraficLightState.RedYellow;
                lights[(int)TraficLight.Yellow].IsOn = true;
                LightChanged?.Invoke(this, new LightChengedEventArgs(TraficLight.Yellow));
            }
            else if (currentState == TraficLightState.RedYellow)
            {
                currentState = TraficLightState.Green;
                lights[(int)TraficLight.Red].IsOn = false;
                lights[(int)TraficLight.Yellow].IsOn = false;
                lights[(int)TraficLight.Green].IsOn = true;
                foreach (TraficLight tl in Enum.GetValues<TraficLight>())
                     LightChanged?.Invoke(this, new LightChengedEventArgs(tl));
            }
            else if (currentState == TraficLightState.Green)
            {
                currentState = TraficLightState.Yellow;
                lights[(int)TraficLight.Green].IsOn = false;
                lights[(int)TraficLight.Yellow].IsOn = true;
                foreach (TraficLight tl in Enum.GetValues<TraficLight>())
                    if (tl != TraficLight.Red)
                        LightChanged?.Invoke(this, new LightChengedEventArgs(tl));
            }
            else if (currentState == TraficLightState.Yellow)
            {
                currentState = TraficLightState.Red;
                lights[(int)TraficLight.Red].IsOn = true;
                lights[(int)TraficLight.Yellow].IsOn = false;
                foreach (TraficLight tl in Enum.GetValues<TraficLight>())
                    if(tl != TraficLight.Green)
                       LightChanged?.Invoke(this, new LightChengedEventArgs(tl));
            }
        }
        public void ChangeSeconds()
        {
            if(NumOfSeconds > 0)
               timer.Interval = NumOfSeconds * 1000;
        }
        public override void SwitchAutoChange()
        {
            isAutoChange = !isAutoChange;
            if (isAutoChange)
                timer.Start();
            else
                timer.Stop();
             
        }
    }
}
