﻿using OpenCvSharp;
using OpenCvSharp.Flann;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chaotic.Resources
{
    internal class AppResources3440x1440 : ApplicationResources
    {

        protected string _fileName = "resources_3440x1440.json";
        public AppResources3440x1440()
        {
            FileName = _fileName;

            AdventureMenu = new Point(3120, 1385);
            Bifrost1 = new Point(2400, 520);
            Bifrost2 = new Point(2400, 630);
            Bifrost3 = new Point(2400, 740);
            Bifrost4 = new Point(2400, 910);
            Bifrost5 = new Point(2400, 1020);
            Bifrost6 = new Point(2400, 1120);
            BifrostMenu = new Point(3025, 1030);
            BifrostOk = new Rect(1550, 775, 300, 125);
            BossMobHealth = new Rect(1000, 30, 600, 300);
            BottomRightExit = new Rect(3160, 1340, 200, 60);
            CenterScreen = new Point(1720, 710);
            ChaosDungeon_1415 = new Point(1140, 480);
            ChaosDungeon_1445 = new Point(1140, 570);
            ChaosDungeon_1475 = new Point(1140, 670);
            ChaosDungeon_1490 = new Point(1140, 760);
            ChaosDungeon_1520 = new Point(1140, 850);
            ChaosDungeon_1540 = new Point(1140, 950);
            ChaosDungeon_1560 = new Point(1140, 1040);
            ChaosDungeon_1580 = new Point(1140, 480);
            ChaosDungeon_1600 = new Point(1140, 570);
            ChaosDungeon_1610 = new Point(1140, 480);

            ChaosDungeon_Elgacia = new Point(2320, 310);
            ChaosDungeon_RightArrow = new Point(2810, 315);
            ChaosDungeon_Shortcut = new Point(1600, 380);
            ChaosDungeon_Vern = new Point(2060, 310);
            ChaosDungeon_Voldis = new Point(2580, 310);

            ChaosLeave = new Rect(110, 350, 150, 50);
            ChaosOk = new Rect(1600, 1250, 300, 50);
            CharacterIcon = new Rect(1620, 1250, 200, 175);
            CharacterMenu = new Point(3030, 1390);
            CharacterProfileMenu = new Point(2950, 1275);
            CharSelectCol0 = 1380;
            CharSelectCol1 = 1740;
            CharSelectCol2 = 2100;
            CharSelectRow0 = 550;
            CharSelectRow1 = 710;
            CharSelectRow2 = 860;

            Chevron = new Rect(3350, 0, 50, 50);
            ClaimAll = new Rect(600, 1120, 300, 70);
            ClickableOffset = new Point(200, 150);
            ClickableRegion = new Rect(600, 175, 2150, 1000);
            ClickableArea = new Point(1100, 450);
            CommunityMenu = new Point(3290, 1385);
            Complete = new Rect();
            ConnectButton = new Point(1850, 1000);
            ElgaciaShardClickpoints = new List<Point>()
            {
                new Point(2000,1200),
                new Point(1450,150),
                new Point(1700,350)
            };

            GameMenu = new Point(3300, 1275);
            GoldDonate = new Rect(1600, 680, 250, 100);
            GuideMenu = new Point(3200, 1385);
            GuidePetMenu = new Point(3100, 1100);
            GuildMenu = new Point(3200, 1275);
            GuildNormalSupport = new Point(1525, 700);
            GuildSupport = new Rect(2080, 575, 200, 50);
            GuildSupportCancel = new Rect(1720, 990, 120, 50);
            GuildSupportOkButton = new Point(1650, 1010);
            HealthBar = new Rect(1260, 1270, 330, 30);
            Inventory = new Rect(1950, 300, 700, 800);
            Kurzan_1640 = new Point(1490, 1040);
            Kurzan_1660 = new Point(1725, 888);
            KurzanMap1PreferredArea = new Rect(1550, 100, 1200, 600);
            KurzanMap1Start = new List<Point>
            {
                new Point(800,275),
                new Point(1700,250)
            };
            KurzanMap2PreferredArea = new Rect(720, 420, 600, 500);
            KurzanMap2Start = new List<Point>
            {
                new Point(1400,50),
                new Point(950,300)
            };
            KurzanMap3PreferredArea = new Rect(720, 270, 700, 650);
            KurzanMap3Start = new List<Point>
            {
                new Point(1000,250)
            };
            KurzanMap3StickingPoint = new Rect(1400, 0, 1200, 280);


            MinimapCenter = new Point(3190, 2220);
            Minimap = new Rect(2990, 50, 400, 350);
            OngoingQuests = new Rect(2980, 500, 200, 50);
            PlecciaShardClickpoints = new List<Point>()
            {
                new Point(2250,400)
            };
            ScreenX = 3440;
            ScreenY = 1440;
            ServicesMenu = new Point(3380, 1385);
            SilverDonate = new Rect(1250, 680, 250, 100);
            Skill_A = new Rect(1380, 1370, 55, 55);
            Skill_D = new Rect(1505, 1370, 55, 55);
            Skill_E = new Rect(1475, 1308, 55, 55);
            Skill_F = new Rect(1570, 1370, 55, 55);
            Skill_HyperV = new Rect(1180, 1335, 65, 65);
            Skill_Q = new Rect(1350, 1308, 55, 55);
            Skill_R = new Rect(1540, 1308, 55, 55);
            Skill_S = new Rect(1445, 1370, 55, 55);
            Skill_T = new Rect(1275, 1335, 65, 65);
            Skill_V = new Rect(1260, 1335, 65, 65);
            Skill_W = new Rect(1415, 1308, 55, 55);

            SouthKurzanLeapClickpoints = new List<Point>()
            {
                new Point(1500,65),
                new Point(880,490)
            };

            SwitchCharacterButton = new Point(950, 1000);
            Timeout = new Rect(350, 40, 1000, 150);
            UnaDaily = new Point(1000, 210);
            UnaDailyDropdown = new Point(1000, 320);
            UnaDailyDropdownFavorite = new Point(1000, 480);
            UnaDailyRegion = new Rect(2050, 425, 200, 600);
            UnaLopangRoute = new List<Point>()
            {
                new Point(565,650   ),
                new Point(650,1125  ),
                new Point(1100,100  ),
                new Point(2230,70   ),
                new Point(2600,300  ),
                new Point(2400,300  ),
                new Point(600,1000  ),
                new Point(800,1150  ),
                new Point(800,1250  ),
                new Point(700,250   ),
                new Point(1000,60   )
            };

            UnaTask = new Point(3000, 1275);
            UnaWeekly = new Point(1200, 210);
            UnaWeeklyDropdown = new Point(1000, 265);
            UnaWeeklyDropdownFavorite = new Point(1000, 350);
            UnaWeeklyRegion = new Rect(2080, 390, 150, 500);
            VoldisLeapClose = new Rect(1700, 20, 1000, 500);
            VoldisLeapNpc = new Rect(2300, 400, 500, 250);
            VoldisLeapX = 800;
            YDistance = 400;

            CheckFileExists();
        }
    }
}