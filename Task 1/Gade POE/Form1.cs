using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gade_POE
{
    public partial class Form1 : Form
    {
        GameEnigine gameEngine = new GameEnigine();
        Map map = new Map(10);

        public Form1()
        {
            InitializeComponent();
            
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            map.BattlefieldCreator();
            gameEngine.map = map;
            lblMap.Text = map.PopulateMap(map.units);

            gameEngine.GameLogic(map.units);

            rtbInfo.Text = gameEngine.info;  
        }

        public class Unit
        {
            //CLASS VARIABLES 
            public int xPos, yPos, Health, maxHealth, speed, attack, attackRange;
            public int team;
            public char symbol;
            public bool combatCheck;
            public string info;
            public int count;
            public string name;

            //CLASS CONSTRUCTOR
            public Unit(int _xPos, int _yPos, int _health, int _speed, int _attack, int _attackRange, int _team, char _symbol, bool _combatCheck, string _name)
            {
                xPos = _xPos;
                yPos = _yPos;
                Health = _health;
                speed = _speed;
                attack = _attack;
                attackRange = _attackRange;
                team = _team;
                symbol = _symbol;
                combatCheck = _combatCheck;
                name = _name;
                maxHealth = _health;
            }

            //CLASS METHODS
            public void MoveUnit(Unit u, Unit closestUnit)
            {
                if (((u.Health * 100) / u.maxHealth) < 25)
                {
                    u.combatCheck = false;
                    Random rnd = new Random();

                    int temp = rnd.Next(0, 4);
                    if (temp == 0)
                    {
                        if (u.xPos - u.speed < 0)
                        {

                        }
                        else
                            u.xPos = u.xPos - u.speed;
                    }
                    else if (temp == 1)
                    {
                        if (u.yPos - u.speed < 0)
                        {

                        }
                        else
                            u.yPos = u.yPos - u.speed;
                    }
                    else if (temp == 2)
                    {
                        if (u.xPos + u.speed > 19)
                        {

                        }
                        else
                            u.xPos = u.xPos + u.speed;
                    }
                    else if (temp == 3)
                    {
                        if (u.yPos + u.speed > 19)
                        {

                        }
                        else
                            u.yPos = u.yPos + u.speed;
                    }
                }
                else
                    for (int k = 0; k < u.speed; k++)
                    {
                        if (u.xPos > closestUnit.xPos)
                        {
                            u.xPos = u.xPos - 1;
                        }
                        else
                        if (u.xPos < closestUnit.xPos)
                        {
                            u.xPos = u.xPos + 1;
                        }
                        else
                        if (u.yPos > closestUnit.yPos)
                        {
                            u.yPos = u.yPos - 1;
                        }
                        else
                        if (u.yPos < closestUnit.yPos)
                        {
                            u.yPos = u.yPos + 1;
                        }
                    }
            }

            public void CombatHandler(Unit closestUnit, Unit u)
            {
                closestUnit.Health -= u.attack;
            }

            public bool RangeCheck(Unit closestUnit, Unit u )
            {
                if (u.attackRange >= Math.Sqrt(Math.Pow((closestUnit.xPos - u.xPos), 2) + Math.Pow((closestUnit.yPos - u.yPos), 2)))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public Unit ClosestUnit(Unit[] units, int numUnits, Unit u)
            {

                double distance = 0;
                int counter = 0;
                double smallestDist;
                Unit closestUnit = u;

                smallestDist = 15;
                for (int j = 0; j < units.Length; j++)
                {
                    if (units[counter].team != u.team && units[counter].Health > 0)
                    {
                        distance = Math.Sqrt(Math.Pow((units[counter].xPos - u.xPos), 2) + Math.Pow((units[counter].yPos - u.yPos), 2));
                        if (distance < smallestDist)
                        {
                            smallestDist = distance;
                            closestUnit = units[j];
                        }
                        counter += 1;
                    }
                    else
                    {
                        counter += 1;
                    }
                }
                return closestUnit;
            }

            public  void Death(Unit[] units ,int i)
            {
                //for (int k = i; k < units.Length - 1; k++)
                //{
                    //units[k] = units[k + 1];
                //}
            }

            public  string ToString(Unit u, Unit[] units, int i)
            {
                info = "";

                if (u.Health <= 0)
                {

                }
                else
                {
                    info += u.name + "\n" + "____________" + "\n" + "Hp : " + u.Health + "\n" + "Damage : " + u.attack + "\n" + "Team : " + (u.team + 1) + "\n" + "In Combat : " + u.combatCheck + "\n" + "Symbol: " + u.symbol; ;
                    info = info + "\n" + "\n";
                }

                return info;
            }

        }

        public class Map
        { 
            //CLASS VARIABLES
            public char[,] map = new char[20, 20];
            public int unitAmount;
            public Unit[] units;

            //CLASS CONSTRUCTOR
            public Map(int _unitAmount)
            {
                unitAmount = _unitAmount;
            }

            //CLASS METHODS
            public void BattlefieldCreator()
            {
                for (int i = 0; i < 20; i++)
                {
                    for (int k = 0; k < 20; k++)
                    {
                        map[i, k] = Convert.ToChar(".");  
                    }
                }

                Random rnd = new Random();
                units = new Unit[unitAmount];

                for (int i = 0; i < unitAmount; i++)
                {
                   
                    int x = rnd.Next(0, 20);
                    int y = rnd.Next(0, 20);
                    int team = rnd.Next(0, 2);
                    int unit = rnd.Next(0, 2);
                    


                    if (unit == 1 & team == 0)
                    {
                        Unit RangedUnit = new Unit(x, y, 10, 1, 1, 5, team , Convert.ToChar("R"), false, "RangedUnit");
                        map[x, y] = RangedUnit.symbol;
                        units[i] = RangedUnit;
                    }

                    if (unit == 1 & team == 1)
                    {
                        Unit RangedUnit = new Unit(x, y, 10, 1, 1, 5, team, Convert.ToChar("r"), false, "RangedUnit");
                        map[x, y] = RangedUnit.symbol;
                        units[i] = RangedUnit;
                    }

                    if (unit == 0 & team == 0)
                    {
                        Unit MeleeUnit = new Unit(x, y, 20, 2, 2, 1, team, Convert.ToChar("M"), false, "MeleeUnit");
                        map[x, y] = MeleeUnit.symbol;
                        units[i] = MeleeUnit;
                    }

                    if (unit == 0 & team == 1)
                    {
                        Unit MeleeUnit = new Unit(x, y, 20, 2, 2, 1, team, Convert.ToChar("m"), false, "MeleeUnit");
                        map[x, y] = MeleeUnit.symbol;
                        units[i] = MeleeUnit;
                        
                    }
                }
            }

            public string PopulateMap(Unit[] units)
            {
                string mapLayout = "";

                for (int k = 0; k < units.Length; k++)
                {

                    if (units[k].Health > 0)
                    {
                        map[units[k].xPos, units[k].yPos] = units[k].symbol;
                    }
                    else
                    {
                        map[units[k].xPos, units[k].yPos] = '.';
                    }
                }

                for (int j = 0; j < 20; j++)
                {
                    for (int l = 0; l < 20; l++)
                    {
                        mapLayout += map[j, l];
                    }
                    mapLayout = mapLayout + "\n";
                }

                return mapLayout;
            }

            public void UpdatePosition(int i, int oldX, int oldY)
            {

                map[units[i].xPos, units[i].yPos] = units[i].symbol;
                map[oldX, oldY] = '.';
            }

        }
        class GameEnigine
        {
            public int roundCheck;
            public string info;
            public string unitType;
            Unit closestUnit;
            public int x, y, i;
            public Map map;
            public int temp;


            public void GameLogic(Unit[] units)
                {

                info = "";

                for (i = 0; i < units.Length; i++)
                {
                    Unit u = (Unit)units[i];

                    x = u.xPos;
                    y = u.yPos;

                    if (u.Health > 0)
                    {
                        closestUnit = u.ClosestUnit(units, units.Length, u);

                        if (closestUnit == u)
                        {
                            u.combatCheck = false; 
                        }else
                        if (u.RangeCheck(closestUnit, u) == true)
                        {
                            u.combatCheck = true;
                            u.CombatHandler(closestUnit, u);
                        }
                        else
                        {
                            u.combatCheck = false;
                            u.MoveUnit(u, closestUnit);
                            map.UpdatePosition(i, x, y);

                        }

                        info += u.ToString(u, units, i);
                    }
                    else
                    {     

                    }
                }
            }
                
        }

            
            

            private void btnStart_Click(object sender, EventArgs e)
            {
                Timer.Enabled = true;
                Timer.Start();
            }

            private void Timer_Tick(object sender, EventArgs e)
            {

                gameEngine.roundCheck += 1;
                gameEngine.temp += 1;
                rtbInfo.Clear();
                gameEngine.GameLogic(map.units);
                rtbInfo.Text = gameEngine.info;
                lblMap.Text = map.PopulateMap(map.units);
                lblScore.Text = "Round : " + gameEngine.roundCheck;

        }

            private void btnPause_Click(object sender, EventArgs e)
            {
                Timer.Enabled = false;
                Timer.Stop();
            }
    }
}
