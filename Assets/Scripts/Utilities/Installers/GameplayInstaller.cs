using Player.Controls;
using UnityEngine;
using Zenject;

namespace Utilities.Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField] private InputReader _inputReader;

        public override void InstallBindings()
        {
            Container.Bind<InputReader>().FromInstance(_inputReader);
        }
    }
}