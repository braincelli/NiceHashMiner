﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NiceHashMiner
{
    class ccminer_tpruvot_sm21 : ccminer
    {
        public ccminer_tpruvot_sm21() : base()
        {
            MinerDeviceName = "NVIDIA2.1";
            Path = "bin\\ccminer_tpruvot.exe";
            APIPort = 4021;

            // disable neoscrypt & whirlpoolx & ethereum
            var tmp = new List<Algorithm>(SupportedAlgorithms);
            tmp.RemoveAt(GetAlgoIndex("neoscrypt"));    // Remove NeoScrypt
            tmp.RemoveAt(GetAlgoIndex("whirlpoolx")-1); // Remove WhirlpoolX
            tmp.RemoveAt(GetAlgoIndex("ethereum")-2);   // Remove Ethereum
            SupportedAlgorithms = tmp.ToArray();

            if (!Config.ConfigData.DisableDetectionNVidia2X)
                QueryCDevs();
        }


        protected override void AddPotentialCDev(string text)
        {
            if (!text.Contains("GPU")) return;

            Helpers.ConsolePrint(MinerDeviceName, "Detected: " + text);

            string[] splt = text.Split(':');

            int id = int.Parse(splt[0].Split('#')[1]);
            string name = splt[1];

            // add only SM 2.1
            if (name.Contains("SM 2.1"))
            {
                name = name.Substring(8);
                CDevs.Add(new ComputeDevice(id, MinerDeviceName, name));
                Helpers.ConsolePrint(MinerDeviceName, "Added: " + name);
            }
        }
    }
}
