using Unity.Entities;

namespace Main.Scripts
{
    public class PlayerHandler : GameHandlerComponent
    {
        private readonly GameHandler _gameHandler = null;
        
        public PlayerHandler(in GameHandler gameHandler)
        {
            _gameHandler = gameHandler;
        }
        
        public void Init()
        {
            EntityManager entManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            
            Entity player = entManager.Instantiate(PrefabEntities.Entities[(int)PrefabEntity.Player]);

            _gameHandler.SetPlayerRef(player);
        }

        public void Tick() { }
    }
}