﻿using SCNURE_BACKEND.Data.Dtos.Users;
using SCNURE_BACKEND.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCNURE_BACKEND.Data.Dtos.Mappers
{
	public static class UserMapper
	{
		public static UserProfileResponse ToUserProfileResponse(this User user)
		{
			return new UserProfileResponse
			{
				Ban = user.Ban,
				Birthday = user.ShowBirthday ? user.Birthday : null,
				Description = user.Description,
				Email = user.ShowEmail ? user.Email : null,
				FirstName = user.FirstName,
				SecondName = user.SecondName,
				Membership = user.Membership,
				Phone = user.ShowPhone ? user.Phone : null,
				Photo = user.Photo,
				RegisterDate = user.RegisterDate
			};
		}

		public static AccountDataResponse ToAccountDataResponse(this User user)
		{
			return new AccountDataResponse
			{
				Ban = user.Ban,
				Birthday = user.Birthday,
				Description = user.Description,
				Email = user.Email,
				FirstName = user.FirstName,
				SecondName = user.SecondName,
				Membership = user.Membership,
				Phone = user.Phone,
				Photo = user.Photo,
				RegisterDate = user.RegisterDate,
				Admin = user.Admin,
				IsEmailConfirmed = user.Verification == null ? true : false,
				Login = user.Login,
				ShowBirthday = user.ShowBirthday,
				ShowEmail = user.ShowEmail,
				ShowPhone = user.ShowPhone
			};
		}
	}
}
