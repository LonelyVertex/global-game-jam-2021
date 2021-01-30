using UnityEngine;

namespace Player.ScriptableObjects
{
    [CreateAssetMenu(menuName = "ScriptableObjects/PlayerInitialConfiguration", order = 1)]
    public class PlayerInitialConfiguration : ScriptableObject
    {
        [SerializeField]
        int playerInitialEnergy;

        [SerializeField]
        int initialTickRate;

        public int PlayerInitialEnergy => playerInitialEnergy;
        public int InitialTickRate => initialTickRate;
    }
}
