using System;

namespace TanksLib
{
    public class Tank
    {
        public int HP { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Speed { get; set; }
        public int Damage { get; set; }
        public float Rotation { get; set; }
        public Tank(int hp, int speed, int dmg)
        {
            Rotation = 0;
            this.HP = hp;
            this.Speed = speed;
            this.Damage = dmg;
        }
    }
}
