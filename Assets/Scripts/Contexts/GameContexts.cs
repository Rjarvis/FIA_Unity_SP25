namespace Contexts
{
    public static class GameContexts
    {
        public static Context[] AllContexts { get; set; }
        public static Context Gameplay { get; set; }
        public static Context Create { get; set; }
        public static Context UI { get; set; }
        public static Context Physics { get; set; }
        public static Context Input { get; set; }
        public static Context Level { get; set; }
        public static Context Player { get; set; }
        public static Context Alien { get; set; }
        public static Context Sound { get; set; }
    }
}