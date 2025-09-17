using System;
using System.Threading.Tasks;
using MavisCase.Common.Serialization;

namespace MavisCase.Common.Persistence
{
    public class UserStorage
    {
        private const string UserFileName = "user.json";
        private User _user;
        
        private readonly FileManager _fileManager;
        private readonly JsonSerializer _jsonSerializer;

        public UserStorage(
            FileManager fileManager,
            JsonSerializer jsonSerializer
        ){
            _fileManager = fileManager;
            _jsonSerializer = jsonSerializer;
        }

        public string GetUserGuid()
        {
            return _user.UserGuid;
        }

        public string GetUserName()
        {
            return _user.UserName;
        }

        public async Task LoadUserAsync()
        {
            if (!await _fileManager.FileExistsAsync(UserFileName))
            {
                var newUser = new User()
                {
                    UserGuid = Guid.NewGuid().ToString(),
                    UserName = "UnknownPlayer",
                };

                await SaveUserAsync(newUser);
            }
            else
            {
                var userJson = await _fileManager.FileReadAsync(UserFileName);
                _user = _jsonSerializer.Deserialize<User>(userJson);
            }
        }

        public async Task SaveUserAsync(User user)
        {
            await Task.Yield();

            _user = user;
            var userJson = _jsonSerializer.Serialize(_user);
            await _fileManager.FileWriteAsync(UserFileName, userJson);
        }
    }
}
