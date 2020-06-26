﻿using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XtraUpload.WebApp.Common;
using System.IO;
using System.Security.Permissions;
using System.Security;

namespace XtraUpload.WebApp
{
    public class FileStoreHealthCheck : IHealthCheck
    {
        readonly UploadOptions _uploadOpts;

        public FileStoreHealthCheck(IOptionsMonitor<UploadOptions> uploadOpts)
        {
            _uploadOpts = uploadOpts.CurrentValue;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (!Directory.Exists(_uploadOpts.UploadPath))
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("file store directory not found."));
            }
            FileIOPermissionAccess readWrite = FileIOPermissionAccess.Read | FileIOPermissionAccess.Write;
            FileIOPermission file = new FileIOPermission(readWrite, _uploadOpts.UploadPath);
            try
            {
                file.Demand();
                return Task.FromResult(HealthCheckResult.Healthy());
            }
            catch (SecurityException)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy("Please check that the file store have the Read and Write permission."));
            }
        }
    }
}