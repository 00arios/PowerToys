﻿// Copyright (c) Microsoft Corporation
// The Microsoft Corporation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading;
using System.Windows;
using FancyZonesEditor.Models;

namespace FancyZonesEditor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    { 
        public Settings[] ZoneSettings { get; set; }

        public EditorOverlay Overlay { get; set; }

        public LayoutModel[] FoundModel { get; set; }

        public static int NumMonitors { get; private set; }

        public App()
        {
            MonitorVM.CurrentMonitor = 0;
            NumMonitors = Environment.GetCommandLineArgs().Length / 6;
            ZoneSettings = new Settings[NumMonitors];
            for (int monitor_shift = 0; monitor_shift < NumMonitors; monitor_shift++)
            {
                ZoneSettings[monitor_shift] = new Settings(monitor_shift);
            }
        }

        public void OnStartup(object sender, StartupEventArgs e)
        {
            FoundModel = new LayoutModel[NumMonitors];

            for (int setting = 0; setting < NumMonitors; setting++)
            {
                foreach (LayoutModel model in ZoneSettings[setting].DefaultModels)
                {
                    if (model.Type == ZoneSettings[setting].ActiveZoneSetLayoutType)
                    {
                        // found match
                        FoundModel[setting] = model;
                        break;
                    }
                }

                if (FoundModel == null)
                {
                    foreach (LayoutModel model in Settings.CustomModels)
                    {
                        if ("{" + model.Guid.ToString().ToUpper() + "}" == Settings.ActiveZoneSetUUid.ToUpper())
                        {
                            // found match
                            FoundModel[setting] = model;
                            break;
                        }
                    }
                }

                if (FoundModel == null)
                {
                    FoundModel[setting] = ZoneSettings[setting].DefaultModels[0];
                }

                FoundModel[setting].IsSelected = true;
            }
            Overlay = new EditorOverlay();
            Overlay.Show();
            Overlay.DataContext = FoundModel[MonitorVM.CurrentMonitor];
        }
    }
}
