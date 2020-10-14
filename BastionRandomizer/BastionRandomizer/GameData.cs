using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BastionRandomiztion
{
    public class GameData
    {
        public List<LevelInfo> levelInfos = new List<LevelInfo>()
        {
            new LevelInfo("ProtoIntro01b", "GUI\\Shell\\level_icons\\map_protointro01b", "Menu_21", "ProtoIntro01b_Completed", 0),
            new LevelInfo("ProtoTown03", "GUI\\Shell\\level_icons\\map_prototown01", "ProtoTown03_154", "", 0),
            new LevelInfo("Crossroads01", "GUI\\Shell\\level_icons\\map_crossroads01", "Menu_33", "Crossroads01_Completed", 99),
            new LevelInfo("Challenge01", "GUI\\Shell\\level_icons\\map_challenge01", "Menu_26", "Challenge01_Completed", 99),
            new LevelInfo("Holdout01", "GUI\\Shell\\level_icons\\map_holdout01", "Menu_31", "Holdout01_Completed", 99),
            new LevelInfo("Falling01", "GUI\\Shell\\level_icons\\map_falling01", "Menu_22", "Falling01_Completed", 99),
            new LevelInfo("Challenge03", "GUI\\Shell\\level_icons\\map_challenge03", "Challenge03_24", "Challenge03_Completed", 99),
            new LevelInfo("Challenge06", "GUI\\Shell\\level_icons\\map_challenge06", "Challenge06_18", "Challenge06_Completed", 99),
            new LevelInfo("Survivor01", "GUI\\Shell\\level_icons\\map_survivor01", "Menu_25", "Survivor01_Completed", 99),
            new LevelInfo("Siege01", "GUI\\Shell\\level_icons\\map_seige01", "Siege01_65", "Siege01_Completed", 99),
            new LevelInfo("Shrine01", "GUI\\Shell\\level_icons\\map_shrine01", "Menu_57", "Shrine01_Completed", 99),
            new LevelInfo("Challenge04", "GUI\\Shell\\level_icons\\map_challenge04", "Challenge04_19", "Challenge04_Completed", 99),
            new LevelInfo("Challenge02", "GUI\\Shell\\level_icons\\map_challenge02", "Challenge02_24", "Challenge02_Completed", 99),
            new LevelInfo("Challenge07", "GUI\\Shell\\level_icons\\map_challenge07", "Challenge07_20", "Challenge07_Completed", 99),
            new LevelInfo("Moving01", "GUI\\Shell\\level_icons\\map_moving01", "Moving01_71", "Moving01_Completed", 99),
            new LevelInfo("Survivor02", "GUI\\Shell\\level_icons\\map_survivor02", "ProtoTown03_144", "Survivor02_Completed", 99),
            new LevelInfo("Crossroads02", "GUI\\Shell\\level_icons\\map_crossroads02", "Menu_58", "Crossroads02_Completed", 99),
            new LevelInfo("Scenes02", "GUI\\Shell\\level_icons\\map_scenes01", "Menu_61", "Scenes02_Completed", 99),
            new LevelInfo("Hunt01", "GUI\\Shell\\level_icons\\map_hunt01", "Menu_60", "Hunt01_Completed", 99),
            new LevelInfo("Challenge10", "GUI\\Shell\\level_icons\\map_challenge10", "ProtoTown03_116", "Challenge10_Completed", 99),
            new LevelInfo("Challenge09", "GUI\\Shell\\level_icons\\map_challenge09", "ProtoTown03_118", "Challenge09_Completed", 99),
            new LevelInfo("Platforms01", "GUI\\Shell\\level_icons\\map_platforms01", "Menu_62", "Platforms01_Completed", 99),
            new LevelInfo("Challenge05", "GUI\\Shell\\level_icons\\map_challenge05", "ProtoTown03_108", "Challenge05_Completed", 99),
            new LevelInfo("Scorched01", "GUI\\Shell\\level_icons\\map_scorched01", "ProtoTown03_73", "Scorched01_Completed", 99),
            new LevelInfo("Challenge08", "GUI\\Shell\\level_icons\\map_challenge08", "ProtoTown03_120", "Challenge08_Completed", 99),
            new LevelInfo("Fortress01", "GUI\\Shell\\level_icons\\map_fortress01", "ProtoTown03_67", "Fortress01_Completed", 99),
            new LevelInfo("Challenge11", "GUI\\Shell\\level_icons\\map_challenge11", "ProtoTown03_117", "Challenge11_Completed", 99),
            new LevelInfo("Gauntlet01", "GUI\\Shell\\level_icons\\map_gauntlet01", "ProtoTown03_74", "Gauntlet01_Completed", 99),
            new LevelInfo("Voyage01", "GUI\\Shell\\level_icons\\map_voyage01", "ProtoTown03_236", "Voyage01_Completed", 99),
            new LevelInfo("Rescue01", "GUI\\Shell\\level_icons\\map_rescue01", "ProtoTown03_159", "Rescue01_Completed", 99),
            new LevelInfo("Challenge12", "GUI\\Shell\\level_icons\\map_challenge12", "ProtoTown03_119", "Challenge12_Completed", 99),
            new LevelInfo("FinalArena01", "GUI\\Shell\\level_icons\\map_finalArena01", "ProtoTown03_101", "", 99)
        };
        public List<Loot> loot = new List<Loot>()
        {
            // ProtoIntro01
            new Loot("Hammer_Kit", 24818, 10, LootType.Weapon, 0),
            new Loot("City_Plant01", 25571, 12, LootType.Generic, 0),
            new Loot("Repeater_Kit", 152629, 12, LootType.Weapon, 0),

            // ProtoIntro01a
            new Loot("Shield_Kit", 10980, 10, LootType.Weapon, 1),

            // ProtoIntro01b
            new Loot("MonumentPiece", 871, 13, LootType.Core, 2),
            new Loot("Longbow_Kit", 2530, 11, LootType.Weapon, 2),
            new Loot("Longbow_Plant", 38098, 13, LootType.Upgrade, 2),
            new Loot("Hammer_Whirlwind_Kit", 75363, 20, LootType.Ability, 2),
            new Loot("City_CarbineUnlockPlant01", 115157, 25, LootType.Generic, 2),
            new Loot("Hammer_Plant", 115317, 12, LootType.Upgrade, 2),

            // ProtoTown03
            new Loot("Crossroads02_Item", 25229, 17, LootType.Generic, 3),

            // Crossroads01
            new Loot("Squirt_Lure_Kit", 6059, 15, LootType.Ability, 4),
            new Loot("Machete_Kit", 6498, 11, LootType.Weapon, 4),
            new Loot("City_Plant02", 6932, 12, LootType.Generic, 4),
            new Loot("Machete_Plant", 84720, 13, LootType.Upgrade, 4),
            new Loot("MonumentPiece", 102983, 13, LootType.Core, 4),

            // Falling01
            new Loot("Grenade_Kit", 18569, 11, LootType.Ability, 5),
            new Loot("MonumentPiece", 144010, 13, LootType.Core, 5),
            new Loot("City_Plant03", 154724, 12, LootType.Generic, 5),
            new Loot("Mortar_Plant", 288939, 12, LootType.Upgrade, 5),

            // Holdout01
            new Loot("MonumentPiece", 2691, 13, LootType.Core, 6),
            new Loot("Mine_Kit", 3149, 8, LootType.Ability, 6),
            new Loot("City_Plant04", 19674, 12, LootType.Generic, 6),
            new Loot("Repeater_Plant", 21654, 14, LootType.Upgrade, 6),

            // Survivor01
            new Loot("Survivor01_Item", 75857, 15, LootType.Generic, 7),
            new Loot("MonumentPiece", 116040, 13, LootType.Core, 7),

            // Siege01
            new Loot("MonumentPiece", 15782, 13, LootType.Core, 8),
            new Loot("Shotgun_Kit", 220247, 11, LootType.Weapon, 8),
            new Loot("Siege01_Item", 384702, 12, LootType.Generic, 8),

            // Shrine01
            new Loot("Shrine01_Item", 24643, 13, LootType.Generic, 9),
            new Loot("Shotgun_Plant", 29130, 13, LootType.Upgrade, 9),

            // Moving01
            new Loot("MonumentPiece", 89146, 13, LootType.Core, 10),

            // Survivor02
            new Loot("Survivor02_Item", 125811, 15, LootType.Generic, 11),

            // Crossroads02
            new Loot("MonumentPiece", 2131, 13, LootType.Core, 12),
            new Loot("Revolvers_Plant", 12703, 15, LootType.Upgrade, 12),
            new Loot("Flamethrower_Plant", 22204, 18, LootType.Upgrade, 12),
            new Loot("Revolvers_Kit", 29498, 13, LootType.Weapon, 12),

            // Scenes02
            new Loot("MonumentPiece_Upgrade", 82156, 21, LootType.Core, 13),

            // Scenes01

            // Hunt01
            new Loot("Spear_Kit", 1262, 9, LootType.Weapon, 15),
            new Loot("Hunt01_Item", 55247, 11, LootType.Generic, 15),
            new Loot("Spear_Plant", 57486, 11, LootType.Upgrade, 15),
            new Loot("MonumentPiece_Upgrade", 155552, 21, LootType.Core, 15),
            new Loot("PlayerDopplewalk_Kit", 325239, 20, LootType.Ability, 15),
            new Loot("Repeater_Plant", 325920, 14, LootType.Upgrade, 15),

            // Platforms01
            new Loot("Platforms01_Item", 7305, 16, LootType.Generic, 16),
            new Loot("Rifle_Kit", 140350, 9, LootType.Weapon, 16),
            new Loot("Machete_Plant", 204238, 13, LootType.Upgrade, 16),
            new Loot("Rifle_Plant", 204376, 11, LootType.Upgrade, 16),
            new Loot("MonumentPiece_Upgrade", 204512, 21, LootType.Core, 16),

            // Scorched01
            new Loot("Flamethrower_Kit", 374, 16, LootType.Weapon, 17),
            new Loot("Hammer_Plant", 101707, 12, LootType.Upgrade, 17),
            new Loot("MonumentPiece_Upgrade", 101844, 21, LootType.Core, 17),
            new Loot("Spear_Plant", 101990, 11, LootType.Upgrade, 17),
            new Loot("Fortress01_Item", 102684, 15, LootType.Generic, 17),

            // Fortress01
            new Loot("Mortar_Kit", 4804, 10, LootType.Weapon, 18),
            new Loot("MusicBox_Item", 5553, 13, LootType.Generic, 18),
            new Loot("MonumentPiece_Upgrade", 40865, 21, LootType.Core, 18),
            new Loot("Revolvers_Plant", 381742, 15, LootType.Upgrade, 18),

            // Gauntlet01
            new Loot("MonumentPiece_Upgrade", 66566, 21, LootType.Core, 19),
            new Loot("Shotgun_Plant", 66712, 13, LootType.Upgrade, 19),
            new Loot("Longbow_Plant", 66850, 13, LootType.Upgrade, 19),

            // Attack01
            new Loot("Attack01_Item", 3939, 13, LootType.Generic, 20),
            new Loot("Crossroads02_Item", 35357, 17, LootType.Generic, 20),

            // Voyage01
            new Loot("MonumentPiece_Upgrade", 23530, 21, LootType.Core, 21),
            new Loot("Cannon_Plant", 247699, 12, LootType.Upgrade, 21),
            new Loot("Rifle_Plant", 247836, 11, LootType.Upgrade, 21),

            // Rescue01
            new Loot("Cannon_Kit", 148783, 10, LootType.Weapon, 22),
            new Loot("Cannon_Plant", 171181, 12, LootType.Upgrade, 22),
            new Loot("Flamethrower_Plant", 171318, 18, LootType.Upgrade, 22),
            new Loot("Mortar_Plant", 172406, 12, LootType.Upgrade, 22),
            new Loot("Rescue01_Item", 172543, 13, LootType.Generic, 22),

            // FinalArena01

            // FinalChase01
            new Loot("Jump_Kit", 275990, 8, LootType.Ability, 24),

            // FinalRam01
            new Loot("Ram_Kit", 123675, 7, LootType.Weapon, 25),

            // FinalZulf01
            // new Loot("Ram_Kit", 664, 671, LootType.Weapon, 26),
            new Loot("MonumentPiece_Upgrade", 1560, 21, LootType.Core, 26),
        };
        public List<Enemy> enemies = new List<Enemy>();
        public List<MapData> Maps = new List<MapData>()
        {
            new MapData("ProtoIntro01", 162680, 184473), // done
            new MapData("ProtoIntro01a", 5403, 24944), // done
            new MapData("ProtoIntro01b", 347870, 398029), // done 
            new MapData("ProtoTown03", 78205, 336088), // done
            new MapData("Crossroads01", 137016, 160204), // done
            new MapData("Falling01", 266414, 294645), // done
            new MapData("Holdout01", 50113, 81129), // done
            new MapData("Survivor01", 246383, 270542), // done
            new MapData("Siege01", 494398, 529044), // done
            new MapData("Shrine01", 62091, 79561), // done
            new MapData("Moving01", 185518, 239299), // done
            new MapData("Survivor02", 192028, 209719), // done
            new MapData("Crossroads02", 119717, 151533), // done
            new MapData("Scenes02", 105982, 124570), // done
            new MapData("Scenes01", 215662, 241492), // done
            new MapData("Hunt01", 136779, 175315), // done
            new MapData("Platforms01", 138589, 166472), // done
            new MapData("Scorched01", 228476, 248326), // done
            new MapData("Fortress01", 125951, 143919), // done
            new MapData("Gauntlet01", 528136, 555335), // done
            new MapData("Attack01", 59356, 83547), // done
            new MapData("Voyage01", 281032, 310044), // done
            new MapData("Rescue01", 302309, 334535), // done
            new MapData("FinalArena01", 222273, 246873), // done
            new MapData("FinalChase01", 292107, 309619), // done
            new MapData("FinalRam01", 101060, 116054), // done
            new MapData("FinalZulf01", 235804, 277706) // done
        };

        public List<Loot> randomizedLoot = new List<Loot>();

        public GameData()
        {
            // need to determine level id somehow
            string[] files = Directory.GetFiles("Bastion Enemy Locations");
            for (int i = 0; i < files.Length; ++i)
            {
                StreamReader reader = new StreamReader(files[i]);
                int levelIndex = int.Parse(reader.ReadLine());

                string enemy;
                while ((enemy = reader.ReadLine()) != null)
                {
                    string[] loot = enemy.Split(' ');
                    enemies.Add(new Enemy(loot[1], int.Parse(loot[0]), loot[1].Length, levelIndex));
                }
            }
        }
    }

    public struct MapData
    {
        public MapData(string levelname, int scriptstart, int scriptend)
        {
            levelName = levelname;
            scriptStart = scriptstart;
            scriptEnd = scriptend;
        }

        public string levelName { get; }
        public int scriptStart { get; }
        public int scriptEnd { get; }
    }

    public struct LevelInfo
    {
        public LevelInfo(string Name, string Graphic, string Audio, string Completedtext, int Cores)
        {
            name = Name;
            graphic = Graphic;
            audio = Audio;
            completedtext = Completedtext;
            cores = Cores;
        }

        public string name;
        public string graphic;
        public string audio;
        public string completedtext;
        public int cores;
    }
    
    public class Loot
    {
        public Loot(string Name, int Start, int Length, LootType Type, int LevelIndex)
        {
            name = Name;
            start = Start;
            length = Length;
            type = Type;
            levelIndex = LevelIndex;
        }

        public Loot(Loot loot)
        {
            name = loot.name;
            start = loot.start;
            length = loot.length;
            type = loot.type;
            levelIndex = loot.levelIndex;
        }

        public string name;
        public int start;
        public int length;
        public LootType type;
        public int levelIndex;
    }

    public class Enemy
    {
        public Enemy(string Name, int Start, int Length, int LevelIndex)
        {
            name = Name;
            start = Start;
            length = Length;
            levelIndex = LevelIndex;
        }

        public Enemy(Enemy enemy)
        {
            name = enemy.name;
            start = enemy.start;
            length = enemy.length;
            levelIndex = enemy.levelIndex;
        }

        public string name;
        public int start;
        public int length;
        public int levelIndex;
    }

    public enum LootType
    {
        Weapon,
        Ability,
        Core,
        Generic,
        Upgrade
    }
}
