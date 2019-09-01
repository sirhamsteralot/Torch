﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;
using NLog;
using Torch.API;

namespace Torch.Server
{
    class TorchService : ServiceBase
    {
        public const string Name = "Torch (SEDS)";
        private TorchServer _server;
        private Initializer _initializer;
        private string[] _args;

        public TorchService(string[] args)
        {
            _args = args;
            var workingDir = new FileInfo(typeof(TorchService).Assembly.Location).Directory.ToString();
            Directory.SetCurrentDirectory(workingDir);
            _initializer = new Initializer(workingDir);

            ServiceName = Name;
            CanHandleSessionChangeEvent = false;
            CanPauseAndContinue = false;
            CanStop = true;
        }

        /// <inheritdoc />
        protected override void OnStart(string[] _)
        {
            base.OnStart(_args);

            _initializer.Initialize(_args);
            _initializer.Run();
        }

        /// <inheritdoc />
        protected override void OnStop()
        {
            _server.Stop();
            base.OnStop();
        }
    }
}
