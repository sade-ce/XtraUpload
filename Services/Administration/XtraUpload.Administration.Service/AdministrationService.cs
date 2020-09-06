﻿using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using XtraUpload.Administration.Service.Common;
using XtraUpload.Authentication.Service.Common;
using XtraUpload.Database.Data.Common;
using XtraUpload.Domain;
using XtraUpload.Domain.Infra;
using XtraUpload.FileManager.Service.Common;
using XtraUpload.WebApp.Common;

namespace XtraUpload.Administration.Service
{
    public class AdministrationService : IAdministrationService
    {
        readonly IUnitOfWork _unitOfWork;

        public AdministrationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Update a page
        /// </summary>
        public async Task<PageResult> UpdatePage(Page p)
        {
            PageResult result = new PageResult();
            Page page = await _unitOfWork.Pages.FirstOrDefaultAsync(s => s.Id == p.Id);
            // Check page exist
            if (page == null)
            {
                result.ErrorContent = new ErrorContent($"The page {p.Name} does not exist", ErrorOrigin.Client);
                return result;
            }
            // Check page name is unique
            Page pageNameUnique = await _unitOfWork.Pages.FirstOrDefaultAsync(s => s.Name == p.Name && s.Id != p.Id);
            if (pageNameUnique != null)
            {
                result.ErrorContent = new ErrorContent($"A page with the same name already exists", ErrorOrigin.Client);
                return result;
            }
            // Update
            page.UpdatedAt = DateTime.Now;
            page.Content = p.Content;
            page.Name = p.Name;
            page.Url = Regex.Replace(p.Name.ToLower(), @"\s+", "_");

            // Save to db
            result = await _unitOfWork.CompleteAsync(result);
            if (result.State == OperationState.Success)
            {
                result.Page = page;
            }
            return result;
        }

        /// <summary>
        /// Delete a Page
        /// </summary>
        public async Task<OperationResult> DeletePage(string Id)
        {
            OperationResult result = new OperationResult();
            // Get page name
            Page page = await _unitOfWork.Pages.FirstOrDefaultAsync(s => s.Id == Id);
            if (page == null)
            {
                result.ErrorContent = new ErrorContent($"The requested page was not found", ErrorOrigin.Client);
                return result;
            }

            _unitOfWork.Pages.Remove(page);

            // Save to db
            return await _unitOfWork.CompleteAsync(result);
        }

        
    }
}
