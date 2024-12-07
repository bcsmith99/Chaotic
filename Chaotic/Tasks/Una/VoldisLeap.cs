﻿using Chaotic.Tasks.Una;
using Chaotic.Tasks;
using Chaotic.User;
using Chaotic.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using IP = Chaotic.Utilities.ImageProcessing;
using System.Diagnostics;

namespace Chaotic.Tasks.Una
{
    [UnaTask(UnaTaskNames.VoldisLeap)]
    public class VoldisLeap : UnaTask
    {
        public VoldisLeap(UITasks uiTask, MouseUtility mouse, KeyboardUtility kb, ResourceHelper r, UserSettings settings, AppLogger logger)
                    : base(uiTask, mouse, kb, r, settings, logger)
        {

        }

        protected override void ExecuteTask()
        {
            var retryCount = 0;
            var maxTries = 60;

            ScreenSearchResult npc = new ScreenSearchResult();

            while (retryCount < maxTries)
            {
                _kb.Press(Key.G, 500);

                var closeButton = IP.LocateCenterOnScreen(Utility.ImageResourceLocation("x.png", _settings.Resolution), IP.ConvertStringCoordsToRect(_r["VoldisLeap_Close"]), .65);
                if (closeButton.Found)
                    _mouse.ClickPosition(closeButton.CenterX, closeButton.CenterY, 200);

                if (retryCount > 4)
                    npc = IP.LocateCenterOnScreen(Utility.ImageResourceLocation("voldis_leap_npc.png", _settings.Resolution), IP.ConvertStringCoordsToRect(_r["VoldisLeap_Npc"]), .7);

                if (npc.Found)
                    break;

                retryCount++;
                Sleep.SleepMs(500, 750);
            }

            if (npc.Found)
            {
                _logger.Log(LogDetailLevel.Debug, $"Voldis NPC Confidence: {npc.MaxConfidence}");
                _mouse.ClickPosition(npc.CenterX, npc.CenterY + 300, 2000, MouseButtons.Right);
            }
            else
            {
                _logger.Log(LogDetailLevel.Debug, $"Voldis NPC Not Found, Reverting to backup and walk yo ass over there.");
                _mouse.ClickPosition(CenterScreen.X + Int32.Parse(_r["VoldisLeap_X"]), CenterScreen.Y + 100, 2000, MouseButtons.Right);
            }

            _kb.Press(Key.G, 1200);
            _kb.ShiftPress(Key.G, 400);

            var completeButton = IP.LocateCenterOnScreen(Utility.ImageResourceLocation("complete_button.png", _settings.Resolution), 400, 950, 200, 100, .95);
            if (completeButton.Found)
            {
                Sleep.SleepMs(100, 200);
                _mouse.ClickPosition(completeButton.CenterX, completeButton.CenterY, 3000);
            }
        }
    }
}
