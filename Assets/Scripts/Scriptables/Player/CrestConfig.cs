using UnityEngine;

namespace Scriptables.Player
{
    [CreateAssetMenu(fileName = "New Crest Config", menuName = "Player/Crest Config", order = 0)]
    public class CrestConfig : ScriptableObject
    {
        [SerializeField] private int _maxEnergyAmount = 8;
        [SerializeField] private int _energyToHeal = 8;
        [SerializeField] private int _healAmount = 3;
        
        public int MaxEnergyAmount => _maxEnergyAmount;
        public int EnergyToHeal => _energyToHeal;
        public int HealAmount => _healAmount;
    }
}