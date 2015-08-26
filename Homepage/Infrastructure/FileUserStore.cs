using System;
using System.IO;
using System.Threading.Tasks;
using Homepage.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace Homepage.Infrastructure
{
    public class FileUserStore : IUserStore<ApplicationUser>, IUserPasswordStore<ApplicationUser>, IUserEmailStore<ApplicationUser>, IUserLockoutStore<ApplicationUser, string>, IUserTwoFactorStore<ApplicationUser, string>
    {
        private string usercachePath = @"C:\Users\Joen\Documents\Projects\Homepage\Homepage\usercache";

        public Task CreateAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                string filename = GetUserFilename(user.Id);
                string serializedUser = JsonConvert.SerializeObject(user, Formatting.Indented);
                File.WriteAllText(filename, serializedUser);
            });
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                string filename = GetUserFilename(user.Id);
                File.Delete(filename);
            });
        }

        public void Dispose()
        {
        }

        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return Task<ApplicationUser>.Run(() =>
            {
                string filename = GetUserFilename(userId);
                if (!File.Exists(filename))
                {
                    return null;
                }
                else
                {
                    ApplicationUser user = JsonConvert.DeserializeObject<ApplicationUser>(File.ReadAllText(filename));
                    return user;
                }
            });
        }

        public Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return Task.Run(() =>
            {
                var files = new DirectoryInfo(usercachePath).GetFiles();
                foreach (var file in files)
                {
                    ApplicationUser user = JsonConvert.DeserializeObject<ApplicationUser>(File.ReadAllText(file.FullName));
                    if (user.UserName == userName)
                    {
                        return user;
                    }
                }

                return null;
            });
        }

        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return Task.Run(() =>
            {
                var files = new DirectoryInfo(usercachePath).GetFiles();
                foreach (var file in files)
                {
                    ApplicationUser user = JsonConvert.DeserializeObject<ApplicationUser>(File.ReadAllText(file.FullName));
                    if (user.Email == email)
                    {
                        return user;
                    }
                }

                return null;
            });
        }

        public Task<string> GetEmailAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                return user.Email;
            });
        }

        public Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                return user.EmailConfirmed;
            });
        }

        public Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                return user.PasswordHash;
            });
        }

        public Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                return !string.IsNullOrWhiteSpace(user.PasswordHash);
            });
        }

        public Task SetEmailAsync(ApplicationUser user, string email)
        {
            return Task.Run(() =>
            {
                var _user = FindByIdAsync(user.Id).Result;
                _user.Email = email;
                UpdateAsync(_user).Wait();
            });
        }

        public Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            return Task.Run(() =>
            {
                var _user = FindByIdAsync(user.Id).Result;
                _user.EmailConfirmed = confirmed;
                UpdateAsync(_user).Wait();
            });
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            return Task.Run(() =>
            {
                user.PasswordHash = passwordHash;
                //var _user = FindByIdAsync(user.Id).Result;
                //_user.PasswordHash = passwordHash;
                //UpdateAsync(_user).Wait();
            });
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            return Task.Run(() =>
            {
                string filename = GetUserFilename(user.Id);
                string serializedUser = JsonConvert.SerializeObject(user);
                File.WriteAllText(filename, serializedUser);
            });
        }

        private string GetUserFilename(string userId)
        {
            return Path.Combine(usercachePath, userId + ".json");
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            return Task.Run(() => { return user.LockoutEndDateUtc.HasValue ? user.LockoutEndDateUtc.Value : DateTimeOffset.MinValue; });
        }

        public Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset lockoutEnd)
        {
            return Task.Run(() => { return user.LockoutEndDateUtc = lockoutEnd.UtcDateTime; });
        }

        public Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            return Task.Run(() => { user.AccessFailedCount++; return user.AccessFailedCount; });
        }

        public Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            return Task.Run(() => { user.AccessFailedCount = 0; });
        }

        public Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            return Task.Run(() => { return user.AccessFailedCount; });
        }

        public Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            return Task.Run(() => { return user.LockoutEnabled; });
        }

        public Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
        {
            return Task.Run(() => { user.LockoutEnabled = enabled; });
        }

        public Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            return Task.Run(() => { user.TwoFactorEnabled = enabled; });
        }

        public Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            return Task.Run(() => { return user.TwoFactorEnabled; });
        }
    }
}