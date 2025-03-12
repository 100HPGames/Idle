using System.Collections.Generic;
using CodeBase.Tools;
using CodeBase.Tools.StaticDataLoader;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class StaticData : IService
    {
        private GameDTO _gameDto;
        public GameDTO GameDTO => _gameDto;

        public async UniTask Initialize()
        {
            var text = await Resources.LoadAsync(GameConst.GameDataFilePath).ToUniTask();
            _gameDto = JsonConvert.DeserializeObject<GameDTO>(text.ToString());
        }
    }
}