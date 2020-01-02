using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Services.Images
{
	public interface IImagesService
	{
		Task<string> UploadUserPicture(IFormFile file, int userId);
		Task<string> UploadStartupPicture(IFormFile file, int startupId);
	}
}
