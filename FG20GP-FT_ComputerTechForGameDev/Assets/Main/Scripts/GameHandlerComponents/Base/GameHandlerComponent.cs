namespace Main.Scripts
{
    public interface GameHandlerComponent
    {
        /// <summary>
        /// The same as Start.
        /// </summary>
        /* Called Init instead of Start for clarity and readability. */
        void Init();
        
        /// <summary>
        /// The same as a Update.
        /// </summary>
        /* Called Tick instead of Update for clarity and readability. */
        void Tick(); 
    }
}