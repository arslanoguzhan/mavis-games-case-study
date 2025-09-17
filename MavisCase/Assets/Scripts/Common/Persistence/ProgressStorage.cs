using System;
using System.Threading.Tasks;
using MavisCase.Common.Serialization;

namespace MavisCase.Common.Persistence
{
    public class ProgressStorage<TProgress> where TProgress : class, IProgress, new()
    {
        private string _progressFileName = string.Empty;
        public TProgress Progress { get; private set; }
        
        private readonly IGamePrefix _gamePrefix;
        private readonly FileManager _fileManager;
        private readonly JsonSerializer _jsonSerializer;

        public ProgressStorage(
            IGamePrefix gamePrefix,
            FileManager fileManager,
            JsonSerializer jsonSerializer
        ){
            _gamePrefix = gamePrefix;
            _fileManager = fileManager;
            _jsonSerializer = jsonSerializer;

            _progressFileName = $"progress.{_gamePrefix.Prefix}.json";
        }

        public async Task LoadProgressAsync(Func<TProgress> initialProgressFunction)
        {
            if (!await _fileManager.FileExistsAsync(_progressFileName))
            {
                var initialProgress = initialProgressFunction();
                await SaveProgressAsync(initialProgress);
            }
            else
            {
                var progressJson = await _fileManager.FileReadAsync(_progressFileName);
                Progress = _jsonSerializer.Deserialize<TProgress>(progressJson);
            }
        }

        public async Task SaveProgressAsync(TProgress progress)
        {
            Progress = progress;
            var progressJson = _jsonSerializer.Serialize(Progress);
            await _fileManager.FileWriteAsync(_progressFileName, progressJson);
        }
    }
}
