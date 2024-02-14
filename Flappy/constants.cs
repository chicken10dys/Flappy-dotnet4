namespace Flappy
{
    public static class C
    {
        //Screen width and height
        public const int SCREEN_HEIGHT = 1024;
        public const int SCREEN_WIDTH = 576;
        
        //gamestate meanings
        public const int STATS = 0;
        public const int MENU = 1;
        public const int READY = 2;
        public const int GAME = 3;
        public const int END = 4;

        //Speeds
        public const int MAX_SPEED = 15;
        public const float SPEED = 3.5f;
        public const float GRAVITY = 0.5f;
        public const float FLAPPOWER = -10f;
    }

}