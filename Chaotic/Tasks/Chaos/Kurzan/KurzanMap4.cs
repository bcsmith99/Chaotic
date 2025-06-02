using Chaotic.Resources;
using Chaotic.Tasks.Chaos.Class;
using Chaotic.User;
using Chaotic.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IP = Chaotic.Utilities.ImageProcessing;


namespace Chaotic.Tasks.Chaos.Kurzan
{
    internal class KurzanMap4 : KurzanBase
    {
        public KurzanMap4(UserSettings settings, MouseUtility mouse, KeyboardUtility kb, ApplicationResources rh, AppLogger logger) : base("KurzanMap4", settings, mouse, kb, rh, logger)
        {
            PreferredMovementArea = _r.ClickableRegion; // IP.ConvertStringCoordsToRect(_r["KurzanMap3_PreferredArea"]);
        }

        public override void StartMapMove(ChaosClass cc)
        {
            _logger.Log(LogDetailLevel.Debug, "Kurzan Map 4 Initial Move");
            var startPoints = _r.KurzanMap4Start;
            _mouse.ClickPosition(startPoints[0], 2000, MouseButtons.Right);
            
        }
    }
}
