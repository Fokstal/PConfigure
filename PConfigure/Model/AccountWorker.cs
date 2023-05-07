using PConfigure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PConfigure.Model
{
	class AccountWorker
	{
		private static bool CheckIsNull(params string?[] listArg)
		{
			foreach (var arg in listArg) if (arg is null) return true;

			return false;
		}

		private static string Hash(string password)
		{
			byte[] data = Encoding.Default.GetBytes(password);
			MD5 md5 = new MD5CryptoServiceProvider();
			byte[] result = md5.ComputeHash(data);
			password = Convert.ToBase64String(result);
			return password;
		}

		#region Account

		static readonly string codeCreator = "CREATOR";

		public static bool SignInAccout(out string resultStr, string? login, string? password)
		{
			resultStr = $"This {login} in creator is not exists!";

			if (CheckIsNull(login, password))
			{
				resultStr = $"Your data has a NULL values!";

				return false;
			}

			string passwordHash = "";
			if (password is not null) passwordHash = Hash(password);

			using (PConfigureContext db = new())
			{
				bool checkIsExist = db.Creators.Any(o => o.Login == login);

				if (checkIsExist)
				{
					resultStr = "SignIn is NOT success (Password is FALSE)!";

					bool checkIsMathedPassword = db.Creators.Any(o => o.PasswordHash == passwordHash);

					if (checkIsMathedPassword)
					{
						resultStr = "SignIn is SUCCESS!";

						return true;
					}

				}
			}

			return false;
		}
		public static bool AddNewValue(out string resultStr, string? login, string? password)
		{
			resultStr = $"Add new {codeCreator} is NOT success";

			if (CheckIsNull(login, password))
			{
				resultStr = $"Your data has a NULL values!";

				return false;
			}

			string passwordHash = "";
			if (password is not null) passwordHash = Hash(password);

			using (PConfigureContext db = new())
			{
				bool checkIsExist = db.Creators.Any(o => o.Login == login);

				if (!checkIsExist)
				{
					resultStr = $"Add new {codeCreator} is SUCCESS";

					db.Creators.Add(new Creator() { Login = login, PasswordHash = passwordHash });
					db.SaveChanges();

					return true;
				}

				return false;
			}
		}
		public static bool DeleteValue(Creator value, out string resultStr)
		{
			resultStr = $"This {value.Login} in {codeCreator} is not exists";

			using (PConfigureContext db = new())
			{
				bool checkIsExist = db.Creators.Contains(value); ;

				if (checkIsExist)
				{
					db.Creators.Remove(value);
					db.SaveChanges();

					resultStr = $"This {value.Login} in {codeCreator} hase been delete";
					return true;
				}
			}

			return false;
		}
		public static bool EditValue(out string resultStr, Creator oldValue, string? login, string? password)
		{
			resultStr = $"This {oldValue.Login} in {codeCreator} is NOT exists";

			if (CheckIsNull(login, password))
			{
				resultStr = $"Your data has a NULL values!";

				return false;
			}

			using (PConfigureContext db = new())
			{
				Creator? value = db.Creators.FirstOrDefault(o => o.ID == oldValue.ID);

				if (value is not null)
				{
					resultStr = $"This {oldValue.Login} in {codeCreator} has been CHANGED!";

					value.Login = login;
					if (password is not null) value.PasswordHash = Hash(password);

					db.SaveChanges();

					return true;
				}

				return false;
			}
		}
		public static List<Creator> GetAllCreator()
		{
			using (PConfigureContext db = new())
			{
				return db.Creators.ToList();
			}
		}

		#endregion
	}
}
