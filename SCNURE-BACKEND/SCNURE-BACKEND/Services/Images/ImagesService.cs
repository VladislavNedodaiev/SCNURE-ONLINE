using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SCNURE_BACKEND.Services.Users;

namespace SCNURE_BACKEND.Services.Images
{
	public class ImagesService : IImagesService
	{
		private readonly IUserService userService;
		private readonly IStartupService startupService;

		public ImagesService(IUserService userService, IStartupService startupService)
		{
			this.userService = userService;
			this.startupService = startupService;
		}

		public async Task<string> UploadStartupPicture(IFormFile file, int startupId)
		{
			if (file == null || file.Length == 0)
				throw new ArgumentException("File wasn't found");

			var folderName = Path.Combine("Resources", "StartupsPics");
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

			if (!Directory.Exists(filePath))
			{
				Directory.CreateDirectory(filePath);
			}

			string uniqueFileName = $"startups_{startupId}_pic.png";
			string dbPath = Path.Combine(folderName, uniqueFileName);

			await startupService.UpdatePhotoPath(dbPath, startupId);

			using (var fileStream = new FileStream(Path.Combine(filePath, uniqueFileName), FileMode.Create))
			{
				await file.CopyToAsync(fileStream);
			}

			return dbPath;
		}

		public async Task<string> UploadUserPicture(IFormFile file, int userId)
		{
			if (file == null || file.Length == 0)
				throw new ArgumentException("File wasn't found");

			var folderName = Path.Combine("Resources", "UsersPics");
			var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

			if (!Directory.Exists(filePath))
			{
				Directory.CreateDirectory(filePath);
			}

			string uniqueFileName = $"user_{userId}_pic.png";
			string dbPath = Path.Combine(folderName, uniqueFileName);

			await userService.UpdatePhotoPath(dbPath, userId);

			using (var fileStream = new FileStream(Path.Combine(filePath, uniqueFileName), FileMode.Create))
			{
				await file.CopyToAsync(fileStream);
			}

			return dbPath;
		}
	}
}
