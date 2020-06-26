﻿using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using XtraUpload.Administration.Service.Common;
using XtraUpload.Domain;
using XtraUpload.WebApp.Common;
using XtraUpload.Setting.Service.Common;

namespace XtraUpload.WebApp.Controllers
{
    [Authorize(Policy = "User")]
    public class SettingController : BaseController
    {
        readonly IMapper _mapper;
        readonly WebAppSettings _webappSettings;
        readonly SocialAuthSettings _socialSettings;
        readonly ISettingService _settingService;

        public SettingController(ISettingService settingService, IOptionsMonitor<SocialAuthSettings> socialSettings,
            IOptionsMonitor<WebAppSettings> webappSettings, IMapper mapper)
        {
            _mapper = mapper;
            _settingService = settingService;
            _webappSettings = webappSettings.CurrentValue;
            _socialSettings = socialSettings.CurrentValue;
        }

        [HttpGet("uploadsetting")]
        public async Task<IActionResult> UploadSetting()
        {
            UploadSettingResult Result = await _settingService.UploadSetting();

            return HandleResult(Result);
        }

        [HttpGet("accountoverview")]
        public async Task<IActionResult> AccountOverview()
        {
            AccountOverviewResult Result = await _settingService.AccountOverview();

            return HandleResult(Result, _mapper.Map<AccountOverviewDto>(Result));
        }

        [HttpPatch("password")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordViewModel model)
        {
            UpdatePasswordResult result = await _settingService.UpdatePassword(model);

            return HandleResult(result);
        }

        [HttpPatch("theme")]
        public async Task<IActionResult> UpdateTheme(ThemeModel model)
        {
            await _settingService.UpdateTheme(model.Theme);
            return Ok();
        }

        [HttpGet("confirmemail")]
        public async Task<IActionResult> ConfirmEmail()
        {
            OperationResult result = await _settingService.RequestConfirmationEmail(Request.Host.Host);

            return HandleResult(result);
        }

        [HttpPut("confirmemail")]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailViewModel model)
        {
            OperationResult result = await _settingService.ConfirmEmail(model.EmailToken);

            return HandleResult(result);
        }

        [AllowAnonymous]
        [HttpGet("page/{name:regex(^[[a-zA-Z0-9_]]*$)}")]
        public async Task<IActionResult> GetPage(string name)
        {
            PageResult result = await _settingService.GetPage(name);

            return HandleResult(result, result.Page);
        }

        [AllowAnonymous]
        [HttpGet("webappconfig")]
        public IActionResult GetWebAppConfig()
        {
            return Ok(_webappSettings);
        }

        [AllowAnonymous]
        [HttpGet("socialauthconfig")]
        public IActionResult GetSocialAuthConfig()
        {
            return Ok(_socialSettings);
        }
    }
}