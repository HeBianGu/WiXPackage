﻿using Microsoft.Tools.WindowsInstallerXml.Bootstrapper;
using System;
using System.Windows;
using System.Windows.Interop;

namespace CustomBA.Models
{
    public class BootstrapperApplicationModel
    {
        private IntPtr hwnd;
        private static BootstrapperApplicationModel bootstrapperAppModel;
        public static BootstrapperApplicationModel GetBootstrapperAppModel(BootstrapperApplication bootstrapperApplication)
        {
            if (bootstrapperAppModel == null)
                bootstrapperAppModel = new BootstrapperApplicationModel(bootstrapperApplication);
            return bootstrapperAppModel;
        }
        public static BootstrapperApplicationModel GetBootstrapperAppModel()
        {
            if (bootstrapperAppModel != null)
                return bootstrapperAppModel;
            else return null;
        }
        private BootstrapperApplicationModel(BootstrapperApplication bootstrapperApplication)
        {
            this.BootstrapperApplication =
              bootstrapperApplication;
            this.hwnd = IntPtr.Zero;
            string[] strs = GetCommandLine();
        }

        public BootstrapperApplication BootstrapperApplication { get; private set; }

        public int FinalResult { get; set; }

        public void SetWindowHandle(Window view)
        {
            this.hwnd = new WindowInteropHelper(view).Handle;
        }

        public void PlanAction(LaunchAction action)
        {
            this.BootstrapperApplication.Engine.Plan(action);
        }

        public void ApplyAction()
        {
            this.BootstrapperApplication.Engine.Apply(this.hwnd);
        }

        public void LogMessage(string message)
        {
            this.BootstrapperApplication.Engine.Log(
              LogLevel.Standard,
              message);
        }
        public void SetBurnVariable(string variableName, string value)
        {
            this.BootstrapperApplication.Engine
               .StringVariables[variableName] = value;
        }
        public string[] GetCommandLine()
        {
            return this.BootstrapperApplication.Command
               .GetCommandLineArgs();
        }
        public bool HelpRequested()
        {
            return this.BootstrapperApplication.Command.Action ==
               LaunchAction.Help;
        }
    }
    public enum InstallState
    {
        Initializing,
        Present,
        NotPresent,
        Applying,
        Cancelled,
        Applied,
        Failed,
    }
}