using Terraria.Achievements;
using Terraria.Audio;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.UI.Chat;
using Terraria.ID;
using Terraria.Localization;

namespace Terraria.Initializers;

public class AchievementInitializer
{
	public static void Load()
	{
		if (Main.netMode != 2) {
			Achievement achievement = new Achievement("TIMBER");
			achievement.AddCondition(ItemPickupCondition.Create(9, 619, 2504, 620, 2503, 2260, 621, 911, 1729, 5215));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("BENCHED");
			achievement.AddCondition(ItemCraftCondition.Create(ItemID.Sets.Workbenches));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("NO_HOBO");
			achievement.AddCondition(ProgressionEventCondition.Create(8));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("OBTAIN_HAMMER");
			achievement.AddCondition(ItemPickupCondition.Create(2775, 2746, 5283, 3505, 654, 3517, 7, 3493, 2780, 1513, 2516, 660, 3481, 657, 922, 3511, 2785, 3499, 3487, 196, 367, 104, 797, 2320, 787, 1234, 1262, 3465, 204, 217, 1507, 3524, 3522, 3525, 3523, 4317, 1305));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("OOO_SHINY");
			achievement.AddCondition(TileDestroyedCondition.Create(7, 6, 9, 8, 166, 167, 168, 169, 22, 204, 58, 107, 108, 111, 221, 222, 223, 211));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("HEART_BREAKER");
			achievement.AddCondition(TileDestroyedCondition.Create(12));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("HEAVY_METAL");
			achievement.AddCondition(ItemPickupCondition.Create(35, 716));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("I_AM_LOOT");
			achievement.AddCondition(CustomFlagCondition.Create("Peek"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("STAR_POWER");
			achievement.AddCondition(CustomFlagCondition.Create("Use"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("HOLD_ON_TIGHT");
			achievement.AddCondition(CustomFlagCondition.Create("Equip"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("EYE_ON_YOU");
			achievement.AddCondition(NPCKilledCondition.Create(4));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("SMASHING_POPPET");
			achievement.AddCondition(ProgressionEventCondition.Create(7));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("WORM_FODDER");
			achievement.AddCondition(NPCKilledCondition.Create(13, 14, 15));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("MASTERMIND");
			achievement.AddCondition(NPCKilledCondition.Create(266));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("WHERES_MY_HONEY");
			achievement.AddCondition(CustomFlagCondition.Create("Reach"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("STING_OPERATION");
			achievement.AddCondition(NPCKilledCondition.Create(222));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("BONED");
			achievement.AddCondition(NPCKilledCondition.Create(35));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("DUNGEON_HEIST");
			achievement.AddCondition(ItemPickupCondition.Create(327));
			achievement.AddCondition(ProgressionEventCondition.Create(19));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("ITS_GETTING_HOT_IN_HERE");
			achievement.AddCondition(CustomFlagCondition.Create("Reach"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("MINER_FOR_FIRE");
			achievement.AddCondition(ItemCraftCondition.Create(122));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("STILL_HUNGRY");
			achievement.AddCondition(NPCKilledCondition.Create(113, 114));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("ITS_HARD");
			achievement.AddCondition(ProgressionEventCondition.Create(9));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("BEGONE_EVIL");
			achievement.AddCondition(ProgressionEventCondition.Create(6));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("EXTRA_SHINY");
			achievement.AddCondition(TileDestroyedCondition.Create(107, 108, 111, 221, 222, 223));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("HEAD_IN_THE_CLOUDS");
			achievement.AddCondition(CustomFlagCondition.Create("Equip"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("LIKE_A_BOSS");
			achievement.AddCondition(ItemPickupCondition.Create(1133, 1331, 1307, 267, 1293, 5334, 557, 544, 556, 560, 43, 70, 3601, 5120, 4961, 4988, 2673));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("BUCKETS_OF_BOLTS");
			achievement.AddCondition(NPCKilledCondition.Create(125, 126));
			achievement.AddConditions(NPCKilledCondition.CreateMany(127, 134));
			achievement.UseConditionsCompletedTracker();
			Main.Achievements.Register(achievement);
			achievement = new Achievement("DRAX_ATTAX");
			achievement.AddCondition(ItemCraftCondition.Create(579, 990));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("PHOTOSYNTHESIS");
			achievement.AddCondition(TileDestroyedCondition.Create(211));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("GET_A_LIFE");
			achievement.AddCondition(CustomFlagCondition.Create("Use"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("THE_GREAT_SOUTHERN_PLANTKILL");
			achievement.AddCondition(NPCKilledCondition.Create(262));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("TEMPLE_RAIDER");
			achievement.AddCondition(ProgressionEventCondition.Create(22));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("LIHZAHRDIAN_IDOL");
			achievement.AddCondition(NPCKilledCondition.Create(245));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("ROBBING_THE_GRAVE");
			achievement.AddCondition(ItemPickupCondition.Create(1513, 938, 963, 977, 1300, 1254, 1514, 679, 759, 1446, 1445, 1444, 1183, 1266, 671, 3291, 4679));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("BIG_BOOTY");
			achievement.AddCondition(ProgressionEventCondition.Create(20));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("FISH_OUT_OF_WATER");
			achievement.AddCondition(NPCKilledCondition.Create(370));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("OBSESSIVE_DEVOTION");
			achievement.AddCondition(NPCKilledCondition.Create(439));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("STAR_DESTROYER");
			achievement.AddConditions(NPCKilledCondition.CreateMany(517, 422, 507, 493));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("CHAMPION_OF_TERRARIA");
			achievement.AddCondition(NPCKilledCondition.Create(398));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("BLOODBATH");
			achievement.AddCondition(ProgressionEventCondition.Create(5));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("SLIPPERY_SHINOBI");
			achievement.AddCondition(NPCKilledCondition.Create(50));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("GOBLIN_PUNTER");
			achievement.AddCondition(ProgressionEventCondition.Create(10));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("WALK_THE_PLANK");
			achievement.AddCondition(ProgressionEventCondition.Create(11));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("KILL_THE_SUN");
			achievement.AddCondition(ProgressionEventCondition.Create(3));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("DO_YOU_WANT_TO_SLAY_A_SNOWMAN");
			achievement.AddCondition(ProgressionEventCondition.Create(12));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("TIN_FOIL_HATTER");
			achievement.AddCondition(ProgressionEventCondition.Create(13));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("BALEFUL_HARVEST");
			achievement.AddCondition(ProgressionEventCondition.Create(15));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("ICE_SCREAM");
			achievement.AddCondition(ProgressionEventCondition.Create(14));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("STICKY_SITUATION");
			achievement.AddCondition(ProgressionEventCondition.Create(16));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("REAL_ESTATE_AGENT");
			achievement.AddCondition(ProgressionEventCondition.Create(17));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("NOT_THE_BEES");
			achievement.AddCondition(CustomFlagCondition.Create("Use"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("JEEPERS_CREEPERS");
			achievement.AddCondition(CustomFlagCondition.Create("Reach"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("FUNKYTOWN");
			achievement.AddCondition(CustomFlagCondition.Create("Reach"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("INTO_ORBIT");
			achievement.AddCondition(CustomFlagCondition.Create("Reach"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("ROCK_BOTTOM");
			achievement.AddCondition(CustomFlagCondition.Create("Reach"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("MECHA_MAYHEM");
			achievement.AddCondition(ProgressionEventCondition.Create(21));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("GELATIN_WORLD_TOUR");
			achievement.AddConditions(NPCKilledCondition.CreateMany(-5, -6, 1, 81, 71, -3, 147, 138, -10, 50, 59, 16, -7, 244, -8, -1, -2, 184, 204, 225, -9, 141, 183, -4));
			achievement.UseConditionsCompletedTracker();
			Main.Achievements.Register(achievement);
			achievement = new Achievement("FASHION_STATEMENT");
			achievement.AddCondition(CustomFlagCondition.Create("Equip"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("VEHICULAR_MANSLAUGHTER");
			achievement.AddCondition(CustomFlagCondition.Create("Hit"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("BULLDOZER");
			achievement.AddCondition(CustomIntCondition.Create("Pick", 10000));
			achievement.UseTrackerFromCondition("Pick");
			Main.Achievements.Register(achievement);
			achievement = new Achievement("THERE_ARE_SOME_WHO_CALL_HIM");
			achievement.AddCondition(NPCKilledCondition.Create(45));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("DECEIVER_OF_FOOLS");
			achievement.AddCondition(NPCKilledCondition.Create(196));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("SWORD_OF_THE_HERO");
			achievement.AddCondition(ItemPickupCondition.Create(757));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("LUCKY_BREAK");
			achievement.AddCondition(CustomFlagCondition.Create("Hit"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("THROWING_LINES");
			achievement.AddCondition(CustomFlagCondition.Create("Use"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("DYE_HARD");
			achievement.AddCondition(CustomFlagCondition.Create("Equip"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("SICK_THROW");
			achievement.AddCondition(ItemPickupCondition.Create(3389));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("FREQUENT_FLYER");
			achievement.AddCondition(CustomFloatCondition.Create("Pay", 10000f));
			achievement.UseTrackerFromCondition("Pay");
			Main.Achievements.Register(achievement);
			achievement = new Achievement("THE_CAVALRY");
			achievement.AddCondition(CustomFlagCondition.Create("Equip"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("COMPLETELY_AWESOME");
			achievement.AddCondition(ItemPickupCondition.Create(98));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("TIL_DEATH");
			achievement.AddCondition(NPCKilledCondition.Create(53));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("ARCHAEOLOGIST");
			achievement.AddCondition(NPCKilledCondition.Create(52));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("PRETTY_IN_PINK");
			achievement.AddCondition(NPCKilledCondition.Create(-4));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("RAINBOWS_AND_UNICORNS");
			achievement.AddCondition(CustomFlagCondition.Create("Use"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("YOU_AND_WHAT_ARMY");
			achievement.AddCondition(CustomFlagCondition.Create("Spawn"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("PRISMANCER");
			achievement.AddCondition(ItemPickupCondition.Create(495));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("IT_CAN_TALK");
			achievement.AddCondition(ProgressionEventCondition.Create(18));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("WATCH_YOUR_STEP");
			achievement.AddCondition(CustomFlagCondition.Create("Hit"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("MARATHON_MEDALIST");
			achievement.AddCondition(CustomFloatCondition.Create("Move", 1106688f));
			achievement.UseTrackerFromCondition("Move");
			Main.Achievements.Register(achievement);
			achievement = new Achievement("GLORIOUS_GOLDEN_POLE");
			achievement.AddCondition(ItemPickupCondition.Create(2294));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("SERVANT_IN_TRAINING");
			achievement.AddCondition(CustomFlagCondition.Create("Finish"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("GOOD_LITTLE_SLAVE");
			achievement.AddCondition(CustomIntCondition.Create("Finish", 10));
			achievement.UseTrackerFromCondition("Finish");
			Main.Achievements.Register(achievement);
			achievement = new Achievement("TROUT_MONKEY");
			achievement.AddCondition(CustomIntCondition.Create("Finish", 25));
			achievement.UseTrackerFromCondition("Finish");
			Main.Achievements.Register(achievement);
			achievement = new Achievement("FAST_AND_FISHIOUS");
			achievement.AddCondition(CustomIntCondition.Create("Finish", 50));
			achievement.UseTrackerFromCondition("Finish");
			Main.Achievements.Register(achievement);
			achievement = new Achievement("SUPREME_HELPER_MINION");
			achievement.AddCondition(CustomIntCondition.Create("Finish", 200));
			achievement.UseTrackerFromCondition("Finish");
			Main.Achievements.Register(achievement);
			achievement = new Achievement("TOPPED_OFF");
			achievement.AddCondition(CustomFlagCondition.Create("Use"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("SLAYER_OF_WORLDS");
			achievement.AddCondition(NPCKilledCondition.Create(13, 14, 15));
			achievement.AddCondition(NPCKilledCondition.Create(113, 114));
			achievement.AddCondition(NPCKilledCondition.Create(125, 126));
			achievement.AddConditions(NPCKilledCondition.CreateMany(4, 266, 35, 50, 222, 134, 127, 262, 245, 439, 398, 370));
			achievement.UseConditionsCompletedTracker();
			Main.Achievements.Register(achievement);
			achievement = new Achievement("YOU_CAN_DO_IT");
			achievement.AddCondition(ProgressionEventCondition.Create(1));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("MATCHING_ATTIRE");
			achievement.AddCondition(CustomFlagCondition.Create("Equip"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("DEFEAT_EMPRESS_OF_LIGHT");
			achievement.AddCondition(NPCKilledCondition.Create(636));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("DEFEAT_QUEEN_SLIME");
			achievement.AddCondition(NPCKilledCondition.Create(657));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("DEFEAT_DREADNAUTILUS");
			achievement.AddCondition(NPCKilledCondition.Create(618));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("DEFEAT_OLD_ONES_ARMY_TIER3");
			achievement.AddCondition(ProgressionEventCondition.Create(23));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("GET_ZENITH");
			achievement.AddCondition(ItemPickupCondition.Create(4956));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("GET_TERRASPARK_BOOTS");
			achievement.AddCondition(ItemPickupCondition.Create(5000));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("FLY_A_KITE_ON_A_WINDY_DAY");
			achievement.AddCondition(CustomFlagCondition.Create("Use"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("FOUND_GRAVEYARD");
			achievement.AddCondition(CustomFlagCondition.Create("Reach"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("GO_LAVA_FISHING");
			achievement.AddCondition(CustomFlagCondition.Create("Do"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("TURN_GNOME_TO_STATUE");
			achievement.AddCondition(ProgressionEventCondition.Create(24));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("TALK_TO_NPC_AT_MAX_HAPPINESS");
			achievement.AddCondition(CustomFlagCondition.Create("Do"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("PET_THE_PET");
			achievement.AddCondition(CustomFlagCondition.Create("Do"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("FIND_A_FAIRY");
			achievement.AddCondition(CustomFlagCondition.Create("Do"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("THROW_A_PARTY");
			achievement.AddCondition(ProgressionEventCondition.Create(25));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("DIE_TO_DEAD_MANS_CHEST");
			achievement.AddCondition(CustomFlagCondition.Create("Do"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("DEFEAT_DEERCLOPS");
			achievement.AddCondition(NPCKilledCondition.Create(668));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("GET_GOLDEN_DELIGHT");
			achievement.AddCondition(ItemPickupCondition.Create(4022));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("DRINK_BOTTLED_WATER_WHILE_DROWNING");
			achievement.AddCondition(CustomFlagCondition.Create("Use"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("GET_CELL_PHONE");
			achievement.AddCondition(ItemPickupCondition.Create(3124));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("GET_ANKH_SHIELD");
			achievement.AddCondition(ItemPickupCondition.Create(1613));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("GAIN_TORCH_GODS_FAVOR");
			achievement.AddCondition(CustomFlagCondition.Create("Use"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("PLAY_ON_A_SPECIAL_SEED");
			achievement.AddCondition(CustomFlagCondition.Create("Do"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("ALL_TOWN_SLIMES");
			achievement.AddCondition(ProgressionEventCondition.Create(26));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("TRANSMUTE_ITEM");
			achievement.AddCondition(ProgressionEventCondition.Create(27));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("PURIFY_ENTIRE_WORLD");
			achievement.AddCondition(CustomFlagCondition.Create("Do"));
			Main.Achievements.Register(achievement);
			achievement = new Achievement("TO_INFINITY_AND_BEYOND");
			achievement.AddCondition(CustomFlagCondition.Create("Do"));
			Main.Achievements.Register(achievement);
			int num = 0;
			Main.Achievements.RegisterIconIndex("TIMBER", num++);
			Main.Achievements.RegisterIconIndex("NO_HOBO", num++);
			Main.Achievements.RegisterIconIndex("OBTAIN_HAMMER", num++);
			Main.Achievements.RegisterIconIndex("HEART_BREAKER", num++);
			Main.Achievements.RegisterIconIndex("OOO_SHINY", num++);
			Main.Achievements.RegisterIconIndex("HEAVY_METAL", num++);
			Main.Achievements.RegisterIconIndex("I_AM_LOOT", num++);
			Main.Achievements.RegisterIconIndex("STAR_POWER", num++);
			Main.Achievements.RegisterIconIndex("HOLD_ON_TIGHT", num++);
			Main.Achievements.RegisterIconIndex("EYE_ON_YOU", num++);
			Main.Achievements.RegisterIconIndex("SMASHING_POPPET", num++);
			Main.Achievements.RegisterIconIndex("WORM_FODDER", num++);
			Main.Achievements.RegisterIconIndex("MASTERMIND", num++);
			Main.Achievements.RegisterIconIndex("WHERES_MY_HONEY", num++);
			Main.Achievements.RegisterIconIndex("STING_OPERATION", num++);
			Main.Achievements.RegisterIconIndex("BONED", num++);
			Main.Achievements.RegisterIconIndex("DUNGEON_HEIST", num++);
			Main.Achievements.RegisterIconIndex("ITS_GETTING_HOT_IN_HERE", num++);
			Main.Achievements.RegisterIconIndex("MINER_FOR_FIRE", num++);
			Main.Achievements.RegisterIconIndex("STILL_HUNGRY", num++);
			Main.Achievements.RegisterIconIndex("ITS_HARD", num++);
			Main.Achievements.RegisterIconIndex("BEGONE_EVIL", num++);
			Main.Achievements.RegisterIconIndex("EXTRA_SHINY", num++);
			Main.Achievements.RegisterIconIndex("HEAD_IN_THE_CLOUDS", num++);
			Main.Achievements.RegisterIconIndex("LIKE_A_BOSS", num++);
			Main.Achievements.RegisterIconIndex("BUCKETS_OF_BOLTS", num++);
			Main.Achievements.RegisterIconIndex("DRAX_ATTAX", num++);
			Main.Achievements.RegisterIconIndex("PHOTOSYNTHESIS", num++);
			Main.Achievements.RegisterIconIndex("GET_A_LIFE", num++);
			Main.Achievements.RegisterIconIndex("THE_GREAT_SOUTHERN_PLANTKILL", num++);
			Main.Achievements.RegisterIconIndex("TEMPLE_RAIDER", num++);
			Main.Achievements.RegisterIconIndex("LIHZAHRDIAN_IDOL", num++);
			Main.Achievements.RegisterIconIndex("ROBBING_THE_GRAVE", num++);
			Main.Achievements.RegisterIconIndex("BIG_BOOTY", num++);
			Main.Achievements.RegisterIconIndex("FISH_OUT_OF_WATER", num++);
			Main.Achievements.RegisterIconIndex("OBSESSIVE_DEVOTION", num++);
			Main.Achievements.RegisterIconIndex("STAR_DESTROYER", num++);
			Main.Achievements.RegisterIconIndex("CHAMPION_OF_TERRARIA", num++);
			Main.Achievements.RegisterIconIndex("BLOODBATH", num++);
			Main.Achievements.RegisterIconIndex("GOBLIN_PUNTER", num++);
			Main.Achievements.RegisterIconIndex("KILL_THE_SUN", num++);
			Main.Achievements.RegisterIconIndex("WALK_THE_PLANK", num++);
			Main.Achievements.RegisterIconIndex("DO_YOU_WANT_TO_SLAY_A_SNOWMAN", num++);
			Main.Achievements.RegisterIconIndex("TIN_FOIL_HATTER", num++);
			Main.Achievements.RegisterIconIndex("BALEFUL_HARVEST", num++);
			Main.Achievements.RegisterIconIndex("ICE_SCREAM", num++);
			Main.Achievements.RegisterIconIndex("SLIPPERY_SHINOBI", num++);
			Main.Achievements.RegisterIconIndex("STICKY_SITUATION", num++);
			Main.Achievements.RegisterIconIndex("REAL_ESTATE_AGENT", num++);
			Main.Achievements.RegisterIconIndex("NOT_THE_BEES", num++);
			Main.Achievements.RegisterIconIndex("JEEPERS_CREEPERS", num++);
			Main.Achievements.RegisterIconIndex("FUNKYTOWN", num++);
			Main.Achievements.RegisterIconIndex("INTO_ORBIT", num++);
			Main.Achievements.RegisterIconIndex("ROCK_BOTTOM", num++);
			Main.Achievements.RegisterIconIndex("MECHA_MAYHEM", num++);
			Main.Achievements.RegisterIconIndex("GELATIN_WORLD_TOUR", num++);
			Main.Achievements.RegisterIconIndex("FASHION_STATEMENT", num++);
			Main.Achievements.RegisterIconIndex("VEHICULAR_MANSLAUGHTER", num++);
			Main.Achievements.RegisterIconIndex("BULLDOZER", num++);
			Main.Achievements.RegisterIconIndex("THERE_ARE_SOME_WHO_CALL_HIM", num++);
			Main.Achievements.RegisterIconIndex("DECEIVER_OF_FOOLS", num++);
			Main.Achievements.RegisterIconIndex("SWORD_OF_THE_HERO", num++);
			Main.Achievements.RegisterIconIndex("LUCKY_BREAK", num++);
			Main.Achievements.RegisterIconIndex("THROWING_LINES", num++);
			Main.Achievements.RegisterIconIndex("DYE_HARD", num++);
			Main.Achievements.RegisterIconIndex("FREQUENT_FLYER", num++);
			Main.Achievements.RegisterIconIndex("THE_CAVALRY", num++);
			Main.Achievements.RegisterIconIndex("COMPLETELY_AWESOME", num++);
			Main.Achievements.RegisterIconIndex("TIL_DEATH", num++);
			Main.Achievements.RegisterIconIndex("ARCHAEOLOGIST", num++);
			Main.Achievements.RegisterIconIndex("PRETTY_IN_PINK", num++);
			Main.Achievements.RegisterIconIndex("RAINBOWS_AND_UNICORNS", num++);
			Main.Achievements.RegisterIconIndex("YOU_AND_WHAT_ARMY", num++);
			Main.Achievements.RegisterIconIndex("PRISMANCER", num++);
			Main.Achievements.RegisterIconIndex("IT_CAN_TALK", num++);
			Main.Achievements.RegisterIconIndex("WATCH_YOUR_STEP", num++);
			Main.Achievements.RegisterIconIndex("MARATHON_MEDALIST", num++);
			Main.Achievements.RegisterIconIndex("GLORIOUS_GOLDEN_POLE", num++);
			Main.Achievements.RegisterIconIndex("SERVANT_IN_TRAINING", num++);
			Main.Achievements.RegisterIconIndex("GOOD_LITTLE_SLAVE", num++);
			Main.Achievements.RegisterIconIndex("TROUT_MONKEY", num++);
			Main.Achievements.RegisterIconIndex("FAST_AND_FISHIOUS", num++);
			Main.Achievements.RegisterIconIndex("SUPREME_HELPER_MINION", num++);
			Main.Achievements.RegisterIconIndex("TOPPED_OFF", num++);
			Main.Achievements.RegisterIconIndex("SLAYER_OF_WORLDS", num++);
			Main.Achievements.RegisterIconIndex("YOU_CAN_DO_IT", num++);
			Main.Achievements.RegisterIconIndex("SICK_THROW", num++);
			Main.Achievements.RegisterIconIndex("MATCHING_ATTIRE", num++);
			Main.Achievements.RegisterIconIndex("BENCHED", num++);
			Main.Achievements.RegisterIconIndex("DEFEAT_QUEEN_SLIME", num++);
			Main.Achievements.RegisterIconIndex("DEFEAT_EMPRESS_OF_LIGHT", num++);
			Main.Achievements.RegisterIconIndex("GET_ZENITH", num++);
			Main.Achievements.RegisterIconIndex("FIND_A_FAIRY", num++);
			Main.Achievements.RegisterIconIndex("DEFEAT_DREADNAUTILUS", num++);
			Main.Achievements.RegisterIconIndex("DEFEAT_OLD_ONES_ARMY_TIER3", num++);
			Main.Achievements.RegisterIconIndex("FLY_A_KITE_ON_A_WINDY_DAY", num++);
			Main.Achievements.RegisterIconIndex("TURN_GNOME_TO_STATUE", num++);
			Main.Achievements.RegisterIconIndex("TALK_TO_NPC_AT_MAX_HAPPINESS", num++);
			Main.Achievements.RegisterIconIndex("GET_TERRASPARK_BOOTS", num++);
			Main.Achievements.RegisterIconIndex("THROW_A_PARTY", num++);
			Main.Achievements.RegisterIconIndex("PET_THE_PET", num++);
			Main.Achievements.RegisterIconIndex("GO_LAVA_FISHING", num++);
			Main.Achievements.RegisterIconIndex("FOUND_GRAVEYARD", num++);
			Main.Achievements.RegisterIconIndex("DIE_TO_DEAD_MANS_CHEST", num++);
			Main.Achievements.RegisterIconIndex("DEFEAT_DEERCLOPS", num++);
			Main.Achievements.RegisterIconIndex("GET_GOLDEN_DELIGHT", num++);
			Main.Achievements.RegisterIconIndex("DRINK_BOTTLED_WATER_WHILE_DROWNING", num++);
			Main.Achievements.RegisterIconIndex("GET_CELL_PHONE", num++);
			Main.Achievements.RegisterIconIndex("GET_ANKH_SHIELD", num++);
			Main.Achievements.RegisterIconIndex("GAIN_TORCH_GODS_FAVOR", num++);
			Main.Achievements.RegisterIconIndex("PLAY_ON_A_SPECIAL_SEED", num++);
			Main.Achievements.RegisterIconIndex("ALL_TOWN_SLIMES", num++);
			Main.Achievements.RegisterIconIndex("TRANSMUTE_ITEM", num++);
			Main.Achievements.RegisterIconIndex("PURIFY_ENTIRE_WORLD", num++);
			Main.Achievements.RegisterIconIndex("TO_INFINITY_AND_BEYOND", num++);
			AchievementCategory category = AchievementCategory.Slayer;
			Main.Achievements.RegisterAchievementCategory("EYE_ON_YOU", category);
			Main.Achievements.RegisterAchievementCategory("SLIPPERY_SHINOBI", category);
			Main.Achievements.RegisterAchievementCategory("WORM_FODDER", category);
			Main.Achievements.RegisterAchievementCategory("MASTERMIND", category);
			Main.Achievements.RegisterAchievementCategory("STING_OPERATION", category);
			Main.Achievements.RegisterAchievementCategory("DEFEAT_DEERCLOPS", category);
			Main.Achievements.RegisterAchievementCategory("BONED", category);
			Main.Achievements.RegisterAchievementCategory("STILL_HUNGRY", category);
			Main.Achievements.RegisterAchievementCategory("DEFEAT_DREADNAUTILUS", category);
			Main.Achievements.RegisterAchievementCategory("DEFEAT_QUEEN_SLIME", category);
			Main.Achievements.RegisterAchievementCategory("BUCKETS_OF_BOLTS", category);
			Main.Achievements.RegisterAchievementCategory("THE_GREAT_SOUTHERN_PLANTKILL", category);
			Main.Achievements.RegisterAchievementCategory("LIHZAHRDIAN_IDOL", category);
			Main.Achievements.RegisterAchievementCategory("FISH_OUT_OF_WATER", category);
			Main.Achievements.RegisterAchievementCategory("DEFEAT_EMPRESS_OF_LIGHT", category);
			Main.Achievements.RegisterAchievementCategory("OBSESSIVE_DEVOTION", category);
			Main.Achievements.RegisterAchievementCategory("STAR_DESTROYER", category);
			Main.Achievements.RegisterAchievementCategory("CHAMPION_OF_TERRARIA", category);
			Main.Achievements.RegisterAchievementCategory("GOBLIN_PUNTER", category);
			Main.Achievements.RegisterAchievementCategory("DO_YOU_WANT_TO_SLAY_A_SNOWMAN", category);
			Main.Achievements.RegisterAchievementCategory("WALK_THE_PLANK", category);
			Main.Achievements.RegisterAchievementCategory("BALEFUL_HARVEST", category);
			Main.Achievements.RegisterAchievementCategory("ICE_SCREAM", category);
			Main.Achievements.RegisterAchievementCategory("TIN_FOIL_HATTER", category);
			Main.Achievements.RegisterAchievementCategory("DEFEAT_OLD_ONES_ARMY_TIER3", category);
			Main.Achievements.RegisterAchievementCategory("TIL_DEATH", category);
			Main.Achievements.RegisterAchievementCategory("THERE_ARE_SOME_WHO_CALL_HIM", category);
			Main.Achievements.RegisterAchievementCategory("ARCHAEOLOGIST", category);
			Main.Achievements.RegisterAchievementCategory("PRETTY_IN_PINK", category);
			Main.Achievements.RegisterAchievementCategory("DECEIVER_OF_FOOLS", category);
			Main.Achievements.RegisterAchievementCategory("VEHICULAR_MANSLAUGHTER", category);
			category = AchievementCategory.Explorer;
			Main.Achievements.RegisterAchievementCategory("SMASHING_POPPET", category);
			Main.Achievements.RegisterAchievementCategory("BEGONE_EVIL", category);
			Main.Achievements.RegisterAchievementCategory("FOUND_GRAVEYARD", category);
			Main.Achievements.RegisterAchievementCategory("ITS_HARD", category);
			Main.Achievements.RegisterAchievementCategory("FUNKYTOWN", category);
			Main.Achievements.RegisterAchievementCategory("WATCH_YOUR_STEP", category);
			Main.Achievements.RegisterAchievementCategory("YOU_CAN_DO_IT", category);
			Main.Achievements.RegisterAchievementCategory("BLOODBATH", category);
			Main.Achievements.RegisterAchievementCategory("KILL_THE_SUN", category);
			Main.Achievements.RegisterAchievementCategory("STICKY_SITUATION", category);
			Main.Achievements.RegisterAchievementCategory("NO_HOBO", category);
			Main.Achievements.RegisterAchievementCategory("IT_CAN_TALK", category);
			Main.Achievements.RegisterAchievementCategory("HEART_BREAKER", category);
			Main.Achievements.RegisterAchievementCategory("I_AM_LOOT", category);
			Main.Achievements.RegisterAchievementCategory("ROBBING_THE_GRAVE", category);
			Main.Achievements.RegisterAchievementCategory("GET_A_LIFE", category);
			Main.Achievements.RegisterAchievementCategory("FIND_A_FAIRY", category);
			Main.Achievements.RegisterAchievementCategory("TRANSMUTE_ITEM", category);
			Main.Achievements.RegisterAchievementCategory("JEEPERS_CREEPERS", category);
			Main.Achievements.RegisterAchievementCategory("WHERES_MY_HONEY", category);
			Main.Achievements.RegisterAchievementCategory("DUNGEON_HEIST", category);
			Main.Achievements.RegisterAchievementCategory("BIG_BOOTY", category);
			Main.Achievements.RegisterAchievementCategory("ITS_GETTING_HOT_IN_HERE", category);
			Main.Achievements.RegisterAchievementCategory("INTO_ORBIT", category);
			Main.Achievements.RegisterAchievementCategory("ROCK_BOTTOM", category);
			Main.Achievements.RegisterAchievementCategory("OOO_SHINY", category);
			Main.Achievements.RegisterAchievementCategory("EXTRA_SHINY", category);
			Main.Achievements.RegisterAchievementCategory("PHOTOSYNTHESIS", category);
			Main.Achievements.RegisterAchievementCategory("PLAY_ON_A_SPECIAL_SEED", category);
			category = AchievementCategory.Challenger;
			Main.Achievements.RegisterAchievementCategory("GELATIN_WORLD_TOUR", category);
			Main.Achievements.RegisterAchievementCategory("SLAYER_OF_WORLDS", category);
			Main.Achievements.RegisterAchievementCategory("REAL_ESTATE_AGENT", category);
			Main.Achievements.RegisterAchievementCategory("ALL_TOWN_SLIMES", category);
			Main.Achievements.RegisterAchievementCategory("YOU_AND_WHAT_ARMY", category);
			Main.Achievements.RegisterAchievementCategory("TOPPED_OFF", category);
			Main.Achievements.RegisterAchievementCategory("MECHA_MAYHEM", category);
			Main.Achievements.RegisterAchievementCategory("BULLDOZER", category);
			Main.Achievements.RegisterAchievementCategory("PURIFY_ENTIRE_WORLD", category);
			Main.Achievements.RegisterAchievementCategory("NOT_THE_BEES", category);
			Main.Achievements.RegisterAchievementCategory("FLY_A_KITE_ON_A_WINDY_DAY", category);
			Main.Achievements.RegisterAchievementCategory("DIE_TO_DEAD_MANS_CHEST", category);
			Main.Achievements.RegisterAchievementCategory("GO_LAVA_FISHING", category);
			Main.Achievements.RegisterAchievementCategory("RAINBOWS_AND_UNICORNS", category);
			Main.Achievements.RegisterAchievementCategory("THROWING_LINES", category);
			Main.Achievements.RegisterAchievementCategory("TURN_GNOME_TO_STATUE", category);
			Main.Achievements.RegisterAchievementCategory("TALK_TO_NPC_AT_MAX_HAPPINESS", category);
			Main.Achievements.RegisterAchievementCategory("FREQUENT_FLYER", category);
			Main.Achievements.RegisterAchievementCategory("LUCKY_BREAK", category);
			Main.Achievements.RegisterAchievementCategory("MARATHON_MEDALIST", category);
			Main.Achievements.RegisterAchievementCategory("PET_THE_PET", category);
			Main.Achievements.RegisterAchievementCategory("THROW_A_PARTY", category);
			Main.Achievements.RegisterAchievementCategory("DRINK_BOTTLED_WATER_WHILE_DROWNING", category);
			Main.Achievements.RegisterAchievementCategory("TO_INFINITY_AND_BEYOND", category);
			Main.Achievements.RegisterAchievementCategory("SERVANT_IN_TRAINING", category);
			Main.Achievements.RegisterAchievementCategory("GOOD_LITTLE_SLAVE", category);
			Main.Achievements.RegisterAchievementCategory("TROUT_MONKEY", category);
			Main.Achievements.RegisterAchievementCategory("FAST_AND_FISHIOUS", category);
			Main.Achievements.RegisterAchievementCategory("SUPREME_HELPER_MINION", category);
			category = AchievementCategory.Collector;
			Main.Achievements.RegisterAchievementCategory("OBTAIN_HAMMER", category);
			Main.Achievements.RegisterAchievementCategory("BENCHED", category);
			Main.Achievements.RegisterAchievementCategory("HEAVY_METAL", category);
			Main.Achievements.RegisterAchievementCategory("STAR_POWER", category);
			Main.Achievements.RegisterAchievementCategory("GET_GOLDEN_DELIGHT", category);
			Main.Achievements.RegisterAchievementCategory("MINER_FOR_FIRE", category);
			Main.Achievements.RegisterAchievementCategory("HEAD_IN_THE_CLOUDS", category);
			Main.Achievements.RegisterAchievementCategory("GET_TERRASPARK_BOOTS", category);
			Main.Achievements.RegisterAchievementCategory("GET_CELL_PHONE", category);
			Main.Achievements.RegisterAchievementCategory("GET_ANKH_SHIELD", category);
			Main.Achievements.RegisterAchievementCategory("DRAX_ATTAX", category);
			Main.Achievements.RegisterAchievementCategory("PRISMANCER", category);
			Main.Achievements.RegisterAchievementCategory("SWORD_OF_THE_HERO", category);
			Main.Achievements.RegisterAchievementCategory("GET_ZENITH", category);
			Main.Achievements.RegisterAchievementCategory("HOLD_ON_TIGHT", category);
			Main.Achievements.RegisterAchievementCategory("THE_CAVALRY", category);
			Main.Achievements.RegisterAchievementCategory("DYE_HARD", category);
			Main.Achievements.RegisterAchievementCategory("MATCHING_ATTIRE", category);
			Main.Achievements.RegisterAchievementCategory("FASHION_STATEMENT", category);
			Main.Achievements.RegisterAchievementCategory("COMPLETELY_AWESOME", category);
			Main.Achievements.RegisterAchievementCategory("TIMBER", category);
			Main.Achievements.RegisterAchievementCategory("SICK_THROW", category);
			Main.Achievements.RegisterAchievementCategory("GLORIOUS_GOLDEN_POLE", category);
			Main.Achievements.RegisterAchievementCategory("TEMPLE_RAIDER", category);
			Main.Achievements.RegisterAchievementCategory("LIKE_A_BOSS", category);
			Main.Achievements.Load();
			Main.Achievements.OnAchievementCompleted += OnAchievementCompleted;
			AchievementsHelper.Initialize();
		}
	}

	private static void OnAchievementCompleted(Achievement achievement)
	{
		Main.NewText(Language.GetTextValue("Achievements.Completed", AchievementTagHandler.GenerateTag(achievement)));
		if (SoundEngine.FindActiveSound(SoundID.AchievementComplete) == null)
			SoundEngine.PlayTrackedSound(SoundID.AchievementComplete);
	}
}
