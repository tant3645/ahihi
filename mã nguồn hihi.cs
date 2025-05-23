using System;
using AssemblyCSharp.Mod.Xmap;

internal class AutoPean
{
    private static bool IsWait;
    private static long TimeStartWait;
    private static long TimeWait;
    private static long lastThuDauTime;

    public static void AutoThuDau()
    {
        if (mSystem.currentTimeMillis() - AutoPean.lastThuDauTime < 2000L)
        {
            return;
        }
        int dauTrongTui = 0;
        for (int i = 0; i < global::Char.myCharz().arrItemBag.Length; i++)
        {
            Item item = global::Char.myCharz().arrItemBag[i];
            if (item != null && item.template.type == 6)
            {
                dauTrongTui += item.quantity;
            }
        }
        if (dauTrongTui <= 20)
        {
            for (int j = 0; j < global::Char.myCharz().arrItemBox.Length; j++)
            {
                Item item2 = global::Char.myCharz().arrItemBox[j];
                if (item2 != null && item2.template.type == 6)
                {
                    Service.gI().getItem(0, (sbyte)j);
                    AutoPean.lastThuDauTime = mSystem.currentTimeMillis();
                    return;
                }
            }
        }
        if ((GameScr.gI().magicTree.currPeas > 0 && GameScr.hpPotion < 10) || (dauTrongTui < 30 && GameCanvas.gameTick % 200 == 0))
        {
            Service.gI().openMenu(4);
            Service.gI().confirmMenu(4, 0);
            AutoPean.lastThuDauTime = mSystem.currentTimeMillis();
        }
    }

    public static void Chodau()
    {
        for (int i = 0; i < ClanMessage.vMessage.size(); i++)
        {
            ClanMessage clanMessage = (ClanMessage)ClanMessage.vMessage.elementAt(i);
            bool flag = clanMessage.maxCap != 0 && clanMessage.playerName != global::Char.myCharz().cName && clanMessage.recieve != clanMessage.maxCap;
            if (flag)
            {
                for (int j = 0; j < 5; j++)
                {
                    Service.gI().clanDonate(clanMessage.id);
                }
            }
        }
    }

    public static void Xindau()
    {
        int clanID = global::Char.myCharz().clanID;
        bool flag = true;
        if (flag)
        {
            Service.gI().clanMessage(1, "", -1);
        }
    }

    public static int Home()
    {
        return global::Char.myCharz().cgender + 21;
    }

    private static void Wait(int time)
    {
        AutoPean.IsWait = true;
        AutoPean.TimeStartWait = mSystem.currentTimeMillis();
        AutoPean.TimeWait = (long)time;
    }

    private static bool IsWaiting()
    {
        bool flag = AutoPean.IsWait && mSystem.currentTimeMillis() - AutoPean.TimeStartWait >= AutoPean.TimeWait;
        if (flag)
        {
            AutoPean.IsWait = false;
        }
        return AutoPean.IsWait;
    }

    public static void Update()
    {
        if (AutoPean.IsWaiting()) return;

        // Chỉ type 3 mới thao tác mọi thứ liên quan đến đậu
        if (DataAccount.Type == 3)
        {
            // Xin đậu mỗi 5 phút (nếu muốn, có thể giữ lại hoặc bỏ dòng này)
            AutoPean.Xindau();
            AutoPean.Wait(301000);

            // Nếu đang ở nhà thì cho đậu + thu đậu
            if (TileMap.mapID == AutoPean.Home())
            {
                AutoPean.Chodau();
                AutoPean.AutoThuDau();
                AutoPean.Wait(2000);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using AssemblyCSharp.Mod.Xmap;

// Token: 0x0200000F RID: 15
internal class AutoBroly
{
	// Token: 0x0600004E RID: 78 RVA: 0x00003EA5 File Offset: 0x000020A5
	private static void Wait(int time)
	{
		AutoBroly.IsWait = true;
		AutoBroly.TimeStartWait = mSystem.currentTimeMillis();
		AutoBroly.TimeWait = (long)time;
	}

	// Token: 0x0600004F RID: 79 RVA: 0x00003EBE File Offset: 0x000020BE
	private static bool IsWaiting()
	{
		if (AutoBroly.IsWait && mSystem.currentTimeMillis() - AutoBroly.TimeStartWait >= AutoBroly.TimeWait)
		{
			AutoBroly.IsWait = false;
		}
		return AutoBroly.IsWait;
	}

	// Token: 0x06000050 RID: 80 RVA: 0x000093CC File Offset: 0x000075CC
	public static bool IsBoss()
	{
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			global::Char @char = (global::Char)GameScr.vCharInMap.elementAt(i);
			if (@char != null && @char.cName.Contains("Broly") && @char.cName.Contains("Super") && @char.cHPFull >= 16070777L)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00009438 File Offset: 0x00007638
	public static void SearchBoss()
	{
		int currentZone = TileMap.zoneID;
		int count = GameScr.gI().zones.Length;
		if (AutoBroly.IsBoss())
		{
			AutoBroly.visitedZones.Clear();
			return;
		}
		AutoBroly.visitedZones.Add(currentZone);
		List<int> list = (from z in Enumerable.Range(0, count)
		where z != currentZone && !AutoBroly.visitedZones.Contains(z)
		select z).ToList<int>();
		if (list.Count == 0)
		{
			AutoBroly.visitedZones.Clear();
			return;
		}
		int zoneId = list[AutoBroly.random.Next(list.Count)];
		Service.gI().requestChangeZone(zoneId, -1);
	}

	// Token: 0x06000052 RID: 82 RVA: 0x000094DC File Offset: 0x000076DC
	public static void FocusSuperBroly()
	{
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			global::Char @char = (global::Char)GameScr.vCharInMap.elementAt(i);
			if (@char != null && @char.cName.Contains("Broly") && @char.cName.Contains("Super") && @char.cHP > 0L && global::Char.myCharz().charFocus != @char)
			{
				global::Char.myCharz().npcFocus = null;
				global::Char.myCharz().mobFocus = null;
				global::Char.myCharz().charFocus = @char;
				return;
			}
		}
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00009574 File Offset: 0x00007774
	public static void Update()
	{
		if (DataAccount.Type == 3)
		{
			if (AutoBroly.IsInHome())
			{
				if (AutoBroly.GetSoLuongDau() <= 20)
				{
					AutoPean.AutoThuDau();
				}
			}
			else
			{
				long cHP = global::Char.myCharz().cHP;
				long cHPFull = global::Char.myCharz().cHPFull;
				long cMP = global::Char.myCharz().cMP;
				long cMPFull = global::Char.myCharz().cMPFull;
				if ((cHPFull > 0L && cHP * 100L / cHPFull <= 5L) || (cMPFull > 0L && cMP * 100L / cMPFull <= 5L))
				{
					AutoBroly.AnDau();
				}
			}
		}
		if (DataAccount.Type == 2 && AutoBroly.IsBoss())
		{
			for (int i = 0; i < GameScr.vCharInMap.size(); i++)
			{
				global::Char @char = (global::Char)GameScr.vCharInMap.elementAt(i);
				if (@char != null && @char.cName.Contains("Broly") && @char.cName.Contains("Super") && (@char.cHPFull < 16077777L || @char.cHP <= 0L))
				{
					AutoBroly.Map = -1;
					AutoBroly.Khu = -1;
					Service.gI().requestChangeZone(0, -1);
				}
			}
		}
		if (DataAccount.Type == 3 && File.Exists(AutoBroly.SuperBrolyCallPath))
		{
			string[] array = File.ReadAllText(AutoBroly.SuperBrolyCallPath).Split(new char[]
			{
				':'
			});
			if (array.Length == 3)
			{
				int mapId = int.Parse(array[0]);
				int zoneId = int.Parse(array[1]);
				if (array[2] == DataAccount.Account)
				{
					AutoBroly.MoveToMapAndZone(mapId, zoneId);
					return;
				}
			}
		}
		if (DataAccount.Type == 1 && AutoBroly.IsBoss())
		{
			for (int j = 0; j < GameScr.vCharInMap.size(); j++)
			{
				global::Char char2 = (global::Char)GameScr.vCharInMap.elementAt(j);
				if (char2 != null && char2.cName.Contains("Broly") && char2.cName.Contains("Super") && char2.cHPFull >= 16077777L && char2.cHP > 0L)
				{
					AutoBroly.CallType3(TileMap.mapID, TileMap.zoneID, "ten_acc_type3");
					break;
				}
			}
		}
		if (DataAccount.Type == 3 && !AutoBroly.IsBoss())
		{
			AutoBroly.RemoveZoneOwner(TileMap.mapID, TileMap.zoneID);
		}
		if (AutoBroly.Map != -1 && AutoBroly.Khu != -1 && TileMap.mapID == AutoBroly.Map && TileMap.zoneID == AutoBroly.Khu && !AutoBroly.IsBoss())
		{
			AutoBroly.Map = -1;
			AutoBroly.Khu = -1;
		}
		if (!AutoBroly.IsWaiting())
		{
			if (DataAccount.Type == 3)
			{
				AutoBroly.TryCheckSuperBroly();
			}
			if (global::Char.myCharz().cHP <= 0L || global::Char.myCharz().meDead)
			{
				if (AutoBroly.IsBoss() && DataAccount.Type != 1)
				{
					AutoBroly.Map = TileMap.mapID;
					AutoBroly.Khu = TileMap.zoneID;
				}
				Service.gI().returnTownFromDead();
				AutoBroly.Wait(3000);
				return;
			}
			if (AutoBroly.Map != -1 && AutoBroly.Khu != -1 && TileMap.mapID == AutoBroly.Map && TileMap.zoneID == AutoBroly.Khu && !AutoBroly.IsBoss())
			{
				AutoBroly.Map = -1;
				AutoBroly.Khu = -1;
			}
			if (AutoBroly.IsBoss())
			{
				if (DataAccount.Type != 1)
				{
					AutoBroly.Map = TileMap.mapID;
					AutoBroly.Khu = TileMap.zoneID;
				}
				AutoBroly.TrangThai = "SP: " + TileMap.mapNames[TileMap.mapID].ToString() + " - " + TileMap.zoneID.ToString();
				if (AutoBroly.visitedZones.Count > 0)
				{
					AutoBroly.visitedZones.Clear();
				}
			}
			else
			{
				AutoBroly.TrangThai = "Không có thông tin ";
			}
			if (AutoBroly.Map != -1 && TileMap.mapID != AutoBroly.Map && !Pk9rXmap.IsXmapRunning)
			{
				XmapController.StartRunToMapId(AutoBroly.Map);
				AutoBroly.Wait(3000);
				return;
			}
			if (TileMap.mapID == AutoBroly.Map && TileMap.zoneID != AutoBroly.Khu && AutoBroly.Khu != -1)
			{
				Service.gI().requestChangeZone(AutoBroly.Khu, -1);
				AutoBroly.Wait(2000);
				return;
			}
			if (TileMap.mapID == AutoBroly.Map && TileMap.zoneID == AutoBroly.Khu && AutoBroly.IsBoss())
			{
				AutoBroly.FocusSuperBroly();
			}
			if (!AutoBroly.IsBoss() && AutoBroly.isDoKhu)
			{
				AutoBroly.SearchBoss();
				AutoBroly.Wait(2000);
				return;
			}
			if (DataAccount.Type == 1 && !AutoBroly.IsBoss() && AutoBroly.NhayNe == 1)
			{
				AutoBroly.NhayNe = 0;
			}
			if (DataAccount.Type == 3)
			{
				if (AutoBroly.NhayNe == 0 && !AutoBroly.IsBoss())
				{
					AutoBroly.NhayNe = 1;
					AutoBroly.NhayCuoiMap();
					AutoBroly.Wait(1000);
					return;
				}
				if (!AutoBroly.IsBoss() && AutoBroly.NhayNe == 1)
				{
					AutoBroly.NhayNe = 0;
				}
			}
			if (DataAccount.Type == 1)
			{
				AutoBroly.AvoidNormalBroly();
			}
			AutoBroly.Wait(500);
		}
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00003EE4 File Offset: 0x000020E4
	public static void NhayCuoiMap()
	{
		if (GameScr.getX(2) > 0 && GameScr.getY(2) > 0)
		{
			KsSupper.TelePortTo(GameScr.getX(2) - 50, GameScr.getY(2));
		}
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00009A2C File Offset: 0x00007C2C
	public static bool IsBroly()
	{
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			global::Char @char = (global::Char)GameScr.vCharInMap.elementAt(i);
			if (@char != null && @char.cName.Contains("Broly") && !@char.cName.Contains("Super"))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00009A8C File Offset: 0x00007C8C
	public static void Painting(mGraphics g)
	{
		string str = TileMap.mapNames[TileMap.mapID];
		string str2 = " - " + TileMap.zoneID.ToString();
		string str3 = NinjaUtil.getMoneys(global::Char.myCharz().cHP).ToString() + " / " + NinjaUtil.getMoneys(global::Char.myCharz().cHPFull).ToString();
		string str4 = NinjaUtil.getMoneys(global::Char.myCharz().cMP).ToString() + " / " + NinjaUtil.getMoneys(global::Char.myCharz().cMPFull).ToString();
		if (AutoBroly.IsBoss())
		{
			for (int i = 0; i < GameScr.vCharInMap.size(); i++)
			{
				global::Char @char = (global::Char)GameScr.vCharInMap.elementAt(i);
				if (@char != null && @char.cName.Contains("Broly") && @char.cName.Contains("Super") && @char.cHPFull >= 16070777L)
				{
					string st = string.Concat(new string[]
					{
						@char.cName,
						" [ ",
						NinjaUtil.getMoneys(@char.cHP).ToString(),
						" / ",
						NinjaUtil.getMoneys(@char.cHPFull).ToString(),
						" ]"
					});
					mFont.tahoma_7b_yellow.drawString(g, st, 20, GameCanvas.h - (GameCanvas.h - GameCanvas.h / 3), 0);
				}
			}
		}
		if (AutoBroly.IsBroly())
		{
			for (int j = 0; j < GameScr.vCharInMap.size(); j++)
			{
				global::Char char2 = (global::Char)GameScr.vCharInMap.elementAt(j);
				if (char2 != null && char2.cName.Contains("Broly") && !char2.cName.Contains("Super"))
				{
					string st2 = string.Concat(new string[]
					{
						char2.cName,
						" [ ",
						NinjaUtil.getMoneys(char2.cHP).ToString(),
						" / ",
						NinjaUtil.getMoneys(char2.cHPFull).ToString(),
						" ]"
					});
					mFont.tahoma_7b_white.drawString(g, st2, 20, GameCanvas.h - (GameCanvas.h - GameCanvas.h / 3), 0);
				}
			}
		}
		mFont.tahoma_7b_white.drawString(g, "HP: " + str3, 30, GameCanvas.h - (GameCanvas.h - 25), 0);
		mFont.tahoma_7b_white.drawString(g, "MP: " + str4, 30, GameCanvas.h - (GameCanvas.h - 35), 0);
		mFont.tahoma_7b_white.drawString(g, str + " " + str2 + " ", 30, GameCanvas.h - (GameCanvas.h - 10), 0);
		mFont.tahoma_7b_white.drawString(g, AutoBroly.Map.ToString() + " " + AutoBroly.Khu.ToString() + " ", GameCanvas.w - 30, GameCanvas.h - (GameCanvas.h - 10), 0);
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00009DC0 File Offset: 0x00007FC0
	static AutoBroly()
	{
		AutoBroly.SuperBrolyInfoPath = "C:\\Users\\Admin\\Desktop\\QLTK - SP ver 008\\Nro_244_Data\\Resources\\Data\\super_broly_info.txt";
		AutoBroly.SuperBrolyOwnerPath = "C:\\Users\\Admin\\Desktop\\QLTK - SP ver 008\\Nro_244_Data\\Resources\\Data\\super_broly_owners.txt";
		AutoBroly.SuperBrolyBlacklistPath = "C:\\Users\\Admin\\Desktop\\QLTK - SP ver 008\\Nro_244_Data\\Resources\\Data\\super_broly_blacklist.txt";
		AutoBroly.TrangThai = "Không có thông tin";
		AutoBroly.Map = -1;
		AutoBroly.Khu = -1;
		AutoBroly.isDoKhu = false;
		AutoBroly.visitedZones = new HashSet<int>();
		AutoBroly.random = new Random();
		AutoBroly.NhayNe = 0;
		try
		{
			if (DataAccount.Type == 3)
			{
				foreach (string path in new string[]
				{
					AutoBroly.AreaControlPath,
					AutoBroly.SuperBrolyBlacklistPath,
					AutoBroly.SuperBrolyOwnerPath,
					AutoBroly.SuperBrolyInfoPath,
					AutoBroly.SuperBrolyCallPath,
					"C:\\Users\\Admin\\Desktop\\QLTK - SP ver 008\\Nro_244_Data\\Resources\\Data\\zone_tracking.txt",
					AutoBroly.BlacklistPath
				})
				{
					try
					{
						if (!File.Exists(path))
						{
							File.WriteAllText(path, string.Empty);
						}
					}
					catch
					{
					}
				}
			}
		}
		catch
		{
		}
	}

	// Token: 0x06000059 RID: 89 RVA: 0x00009EC0 File Offset: 0x000080C0
	private static double MySqrt(double x)
	{
		if (x < 0.0)
		{
			return 0.0;
		}
		double num = x;
		double num2;
		do
		{
			num2 = num;
			num = (num + x / num) / 2.0;
		}
		while (System.Math.Abs(num - num2) > 0.0001);
		return num;
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00003F14 File Offset: 0x00002114
	private static int MyMin(int a, int b)
	{
		if (a >= b)
		{
			return b;
		}
		return a;
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00003F1D File Offset: 0x0000211D
	private static int MyMax(int a, int b)
	{
		if (a <= b)
		{
			return b;
		}
		return a;
	}

	// Token: 0x0600005C RID: 92 RVA: 0x00009F0C File Offset: 0x0000810C
	private static void AvoidNormalBroly()
	{
		if (DataAccount.Type != 1)
		{
			return;
		}
		if (!AutoBroly.IsBroly())
		{
			return;
		}
		global::Char @char = global::Char.myCharz();
		int pxw = TileMap.pxw;
		int num = 30;
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			global::Char char2 = (global::Char)GameScr.vCharInMap.elementAt(i);
			if (char2 != null && char2.cName.Contains("Broly") && !char2.cName.Contains("Super") && char2.cHP > 0L)
			{
				int num2 = @char.cx - char2.cx;
				int num3 = @char.cy - char2.cy;
				double num4 = AutoBroly.MySqrt((double)(num2 * num2 + num3 * num3));
				long num5 = mSystem.currentTimeMillis();
				if (num4 <= 100.0 && num5 - AutoBroly.lastAvoidBrolyTime >= 500L)
				{
					int x = @char.cx;
					if (@char.cx <= num)
					{
						x = pxw - num;
					}
					else if (@char.cx >= pxw - num)
					{
						x = num;
					}
					else if (@char.cx < char2.cx)
					{
						x = AutoBroly.MyMax(@char.cx - (int)(100.0 - num4), num);
					}
					else
					{
						x = AutoBroly.MyMin(@char.cx + (int)(100.0 - num4), pxw - num);
					}
					KsSupper.TelePortTo(x, @char.cy);
					AutoBroly.lastAvoidBrolyTime = num5;
					return;
				}
			}
		}
	}

	// Token: 0x0600005D RID: 93 RVA: 0x0000A088 File Offset: 0x00008288
	public static void TryCheckSuperBroly()
	{
		if (DataAccount.Type != 3)
		{
			return;
		}
		string superBrolyInfoPath = AutoBroly.SuperBrolyInfoPath;
		if (!File.Exists(superBrolyInfoPath))
		{
			return;
		}
		string[] array = File.ReadAllText(superBrolyInfoPath).Split(new char[]
		{
			'|'
		});
		int mapId = 0;
		int num = 0;
		List<int> source = new List<int>();
		List<int> checkedZones = new List<int>();
		foreach (string text in array)
		{
			if (text.StartsWith("mapId:"))
			{
				mapId = int.Parse(text.Substring(6));
			}
			if (text.StartsWith("superCount:"))
			{
				num = int.Parse(text.Substring(11));
			}
			if (text.StartsWith("zones:"))
			{
				source = text.Substring(6).Split(new char[]
				{
					','
				}, StringSplitOptions.RemoveEmptyEntries).Select(new Func<string, int>(int.Parse)).ToList<int>();
			}
			if (text.StartsWith("checkedZones:"))
			{
				checkedZones = text.Substring(13).Split(new char[]
				{
					','
				}, StringSplitOptions.RemoveEmptyEntries).Select(new Func<string, int>(int.Parse)).ToList<int>();
			}
		}
		string zoneTrackingPath = "C:\\Users\\Admin\\Desktop\\QLTK - SP ver 008\\Nro_244_Data\\Resources\\Data\\zone_tracking.txt";
		List<int> list = (from z in source
		where !checkedZones.Contains(z) && !AutoBroly.IsBlacklisted(mapId, z)
		select z).Where(delegate(int z)
		{
			string key = string.Format("{0}:{1}:", mapId, z);
			if (File.Exists(AutoBroly.AreaControlPath) && File.ReadAllLines(AutoBroly.AreaControlPath).Any((string line) => line.StartsWith(key)))
			{
				return false;
			}
			string zoneTrackKey = string.Format("{0}:{1}:{2}", mapId, z, DataAccount.Account);
			return !File.Exists(zoneTrackingPath) || !File.ReadAllLines(zoneTrackingPath).Any((string line) => line.StartsWith(zoneTrackKey));
		}).ToList<int>();
		if (checkedZones.Count >= num || list.Count == 0)
		{
			return;
		}
		int num2 = list[0];
		if (!AutoBroly.TryReserveZone(mapId, num2, DataAccount.Account))
		{
			return;
		}
		string zoneTrackingKey = string.Format("{0}:{1}:{2}", mapId, num2, DataAccount.Account);
		File.AppendAllText(zoneTrackingPath, zoneTrackingKey + Environment.NewLine);
		checkedZones.Add(num2);
		string format = "mapId:{0}|superCount:{1}|zones:{2}|checkedZones:{3}|time:{4:yyyyMMdd_HHmmss}";
		object[] array3 = new object[5];
		array3[0] = mapId;
		array3[1] = num;
		array3[2] = string.Join(",", source.Select(delegate(int z)
		{
			int num4 = z;
			return num4.ToString();
		}).ToArray<string>());
		array3[3] = string.Join(",", checkedZones.Select(delegate(int z)
		{
			int num4 = z;
			return num4.ToString();
		}).ToArray<string>());
		array3[4] = DateTime.Now;
		string contents = string.Format(format, array3);
		File.WriteAllText(superBrolyInfoPath, contents);
		Thread.Sleep(50);
		AutoBroly.MoveToMapAndZone(mapId, num2);
		Thread.Sleep(1000);
		int num3 = 0;
		bool flag = false;
		while (!AutoBroly.HasValidSuperBroly() && num3 < 5000)
		{
			Thread.Sleep(1000);
			num3 += 1000;
		}
		if (AutoBroly.HasValidSuperBroly())
		{
			for (int j = 0; j < GameScr.vCharInMap.size(); j++)
			{
				global::Char @char = (global::Char)GameScr.vCharInMap.elementAt(j);
				if (@char != null && @char.cName.Contains("Broly") && @char.cName.Contains("Super") && @char.cHPFull >= 16077777L && @char.cHP > 0L)
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				AutoBroly.UpdateZoneStatus(mapId, num2, DataAccount.Account, true);
				AutoBroly.SetZoneOwner(mapId, num2, DataAccount.Account);
				return;
			}
		}
		AutoBroly.UpdateZoneStatus(mapId, num2, DataAccount.Account, false);
		AutoBroly.AddToBlacklist(mapId, num2);
		AutoBroly.RemoveZoneOwner(mapId, num2);
		if (File.Exists(zoneTrackingPath))
		{
			string[] contents2 = (from line in File.ReadAllLines(zoneTrackingPath)
			where line != zoneTrackingKey
			select line).ToArray<string>();
			File.WriteAllLines(zoneTrackingPath, contents2);
		}
		Service.gI().requestChangeZone(0, -1);
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00003F26 File Offset: 0x00002126
	private static void MoveToMapAndZone(int mapId, int zoneId)
	{
		XmapController.StartRunToMapId(mapId);
		Service.gI().requestChangeZone(zoneId, -1);
	}

	// Token: 0x0600005F RID: 95 RVA: 0x0000A4A4 File Offset: 0x000086A4
	private static int[] GetZonesOfMap(int mapId)
	{
		int num = GameScr.gI().zones.Length;
		List<int> list = new List<int>();
		for (int i = 0; i < num; i++)
		{
			if (i != 0 && i != 1)
			{
				list.Add(i);
			}
		}
		return list.ToArray();
	}

	// Token: 0x06000060 RID: 96 RVA: 0x0000A4E4 File Offset: 0x000086E4
	public static void OnGameMessageReceived(string message)
	{
		if (message.Contains("Super Broly xuất hiện tại map"))
		{
			if (!File.Exists(AutoBroly.AreaControlPath))
			{
				File.WriteAllText(AutoBroly.AreaControlPath, string.Empty);
			}
			int num = 0;
			int superCount = 0;
			try
			{
				string[] array = message.Split(new char[]
				{
					' '
				});
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] == "map")
					{
						num = int.Parse(array[i + 1].Replace(",", ""));
					}
					if (array[i] == "có")
					{
						superCount = int.Parse(array[i + 1]);
					}
				}
			}
			catch
			{
			}
			int[] zonesOfMap = AutoBroly.GetZonesOfMap(num);
			AutoBroly.SaveSuperBrolyInfoToFile(num, superCount, zonesOfMap, new List<int>());
			if (File.Exists(AutoBroly.SuperBrolyBlacklistPath))
			{
				File.Delete(AutoBroly.SuperBrolyBlacklistPath);
			}
			if (File.Exists(AutoBroly.SuperBrolyOwnerPath))
			{
				File.Delete(AutoBroly.SuperBrolyOwnerPath);
			}
			if (File.Exists(AutoBroly.AreaControlPath))
			{
				string prefix = string.Format("{0}:", num);
				string[] contents = (from line in File.ReadAllLines(AutoBroly.AreaControlPath)
				where !line.StartsWith(prefix)
				select line).ToArray<string>();
				File.WriteAllLines(AutoBroly.AreaControlPath, contents);
			}
			string path = "C:\\Users\\Admin\\Desktop\\QLTK - SP ver 008\\Nro_244_Data\\Resources\\Data\\zone_tracking.txt";
			if (File.Exists(path))
			{
				string prefix = string.Format("{0}:", num);
				string[] contents2 = (from line in File.ReadAllLines(path)
				where !line.StartsWith(prefix)
				select line).ToArray<string>();
				File.WriteAllLines(path, contents2);
			}
		}
	}

	// Token: 0x06000061 RID: 97 RVA: 0x0000A694 File Offset: 0x00008894
	public static void SaveSuperBrolyInfoToFile(int mapId, int superCount, int[] zones, List<int> checkedZones)
	{
		string format = "mapId:{0}|superCount:{1}|zones:{2}|checkedZones:{3}|time:{4:yyyyMMdd_HHmmss}";
		object[] array = new object[5];
		array[0] = mapId;
		array[1] = superCount;
		array[2] = string.Join(",", zones.Select(delegate(int z)
		{
			int num = z;
			return num.ToString();
		}).ToArray<string>());
		array[3] = string.Join(",", checkedZones.Select(delegate(int z)
		{
			int num = z;
			return num.ToString();
		}).ToArray<string>());
		array[4] = DateTime.Now;
		string contents = string.Format(format, array);
		File.WriteAllText(AutoBroly.SuperBrolyInfoPath, contents);
	}

	// Token: 0x06000062 RID: 98 RVA: 0x0000A74C File Offset: 0x0000894C
	public static int CountSuperBrolyInMap()
	{
		int num = 0;
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			global::Char @char = (global::Char)GameScr.vCharInMap.elementAt(i);
			if (@char != null && @char.cName.Contains("Broly") && @char.cName.Contains("Super") && @char.cHPFull >= 16070777L)
			{
				num++;
			}
		}
		return num;
	}

	// Token: 0x06000063 RID: 99 RVA: 0x0000A7BC File Offset: 0x000089BC
	private static bool IsZoneOwned(int mapId, int zoneId)
	{
		if (!File.Exists(AutoBroly.SuperBrolyOwnerPath))
		{
			return false;
		}
		string value = string.Format("{0}:{1}:", mapId, zoneId);
		string[] array = File.ReadAllLines(AutoBroly.SuperBrolyOwnerPath);
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].StartsWith(value))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000064 RID: 100 RVA: 0x0000A818 File Offset: 0x00008A18
	private static void SetZoneOwner(int mapId, int zoneId, string accName)
	{
		string str = string.Format("{0}:{1}:{2}", mapId, zoneId, accName);
		File.AppendAllText(AutoBroly.SuperBrolyOwnerPath, str + Environment.NewLine);
	}

	// Token: 0x06000065 RID: 101 RVA: 0x0000A854 File Offset: 0x00008A54
	private static bool IsBlacklisted(int mapId, int zoneId)
	{
		if (!File.Exists(AutoBroly.SuperBrolyBlacklistPath))
		{
			return false;
		}
		IEnumerable<string> source = File.ReadAllLines(AutoBroly.SuperBrolyBlacklistPath);
		string value = string.Format("{0}:{1}", mapId, zoneId);
		return source.Contains(value);
	}

	// Token: 0x06000066 RID: 102 RVA: 0x0000A898 File Offset: 0x00008A98
	private static void AddToBlacklist(int mapId, int zoneId)
	{
		string str = string.Format("{0}:{1}", mapId, zoneId);
		File.AppendAllText(AutoBroly.SuperBrolyBlacklistPath, str + Environment.NewLine);
	}

	// Token: 0x06000067 RID: 103 RVA: 0x0000A8D4 File Offset: 0x00008AD4
	private static bool HasValidSuperBroly()
	{
		for (int i = 0; i < GameScr.vCharInMap.size(); i++)
		{
			global::Char @char = (global::Char)GameScr.vCharInMap.elementAt(i);
			if (@char != null && @char.cName.Contains("Broly") && @char.cName.Contains("Super") && @char.cHPFull >= 16077777L && @char.cHP > 0L)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000068 RID: 104 RVA: 0x0000A94C File Offset: 0x00008B4C
	public static void CallType3(int mapId, int zoneId, string accName)
	{
		string contents = string.Format("{0}:{1}:{2}", mapId, zoneId, accName);
		File.WriteAllText(AutoBroly.SuperBrolyCallPath, contents);
	}

	// Token: 0x06000069 RID: 105 RVA: 0x0000A97C File Offset: 0x00008B7C
	private static void RemoveZoneOwner(int mapId, int zoneId)
	{
		if (!File.Exists(AutoBroly.SuperBrolyOwnerPath))
		{
			return;
		}
		string key = string.Format("{0}:{1}:", mapId, zoneId);
		string[] contents = (from line in File.ReadAllLines(AutoBroly.SuperBrolyOwnerPath)
		where !line.StartsWith(key)
		select line).ToArray<string>();
		File.WriteAllLines(AutoBroly.SuperBrolyOwnerPath, contents);
	}

	// Token: 0x0600006A RID: 106 RVA: 0x0000A9E4 File Offset: 0x00008BE4
	private static bool IsZoneControlled(int mapId, int zoneId)
	{
		if (!File.Exists(AutoBroly.AreaControlPath))
		{
			return false;
		}
		string key = string.Format("{0}:{1}:", mapId, zoneId);
		return File.ReadAllLines(AutoBroly.AreaControlPath).Any((string line) => line.StartsWith(key));
	}

	// Token: 0x0600006B RID: 107 RVA: 0x0000AA3C File Offset: 0x00008C3C
	private static void SetZoneControlled(int mapId, int zoneId, string accName)
	{
		string str = string.Format("{0}:{1}:{2}", mapId, zoneId, accName);
		File.AppendAllText(AutoBroly.AreaControlPath, str + Environment.NewLine);
	}

	// Token: 0x0600006C RID: 108 RVA: 0x0000AA78 File Offset: 0x00008C78
	private static void RemoveZoneControlled(int mapId, int zoneId)
	{
		if (!File.Exists(AutoBroly.AreaControlPath))
		{
			return;
		}
		string key = string.Format("{0}:{1}:", mapId, zoneId);
		string[] contents = (from line in File.ReadAllLines(AutoBroly.AreaControlPath)
		where !line.StartsWith(key)
		select line).ToArray<string>();
		File.WriteAllLines(AutoBroly.AreaControlPath, contents);
	}

	// Token: 0x0600006D RID: 109 RVA: 0x0000AAE0 File Offset: 0x00008CE0
	private static void ResetAreaControl(int mapId)
	{
		if (!File.Exists(AutoBroly.AreaControlPath))
		{
			return;
		}
		string[] contents = (from line in File.ReadAllLines(AutoBroly.AreaControlPath)
		where !line.StartsWith(string.Format("{0}:", mapId))
		select line).ToArray<string>();
		File.WriteAllLines(AutoBroly.AreaControlPath, contents);
	}

	// Token: 0x0600006E RID: 110 RVA: 0x0000AB34 File Offset: 0x00008D34
	private static bool IsZoneBlacklisted(int mapId, int zoneId)
	{
		if (!File.Exists(AutoBroly.BlacklistPath))
		{
			return false;
		}
		string key = string.Format("{0}:{1}", mapId, zoneId);
		return File.ReadAllLines(AutoBroly.BlacklistPath).Any((string line) => line == key);
	}

	// Token: 0x0600006F RID: 111 RVA: 0x0000AB8C File Offset: 0x00008D8C
	private static void AddZoneBlacklist(int mapId, int zoneId)
	{
		string text = string.Format("{0}:{1}", mapId, zoneId);
		if (!File.Exists(AutoBroly.BlacklistPath) || !File.ReadAllLines(AutoBroly.BlacklistPath).Contains(text))
		{
			File.AppendAllText(AutoBroly.BlacklistPath, text + Environment.NewLine);
		}
	}

	// Token: 0x06000070 RID: 112 RVA: 0x0000ABE4 File Offset: 0x00008DE4
	private static void RemoveZoneBlacklist(int mapId, int zoneId)
	{
		if (!File.Exists(AutoBroly.BlacklistPath))
		{
			return;
		}
		string key = string.Format("{0}:{1}", mapId, zoneId);
		string[] contents = (from line in File.ReadAllLines(AutoBroly.BlacklistPath)
		where line != key
		select line).ToArray<string>();
		File.WriteAllLines(AutoBroly.BlacklistPath, contents);
	}

	// Token: 0x06000071 RID: 113 RVA: 0x0000AC4C File Offset: 0x00008E4C
	private static void ResetBlacklist(int mapId)
	{
		if (!File.Exists(AutoBroly.BlacklistPath))
		{
			return;
		}
		string[] contents = (from line in File.ReadAllLines(AutoBroly.BlacklistPath)
		where !line.StartsWith(string.Format("{0}:", mapId))
		select line).ToArray<string>();
		File.WriteAllLines(AutoBroly.BlacklistPath, contents);
	}

	// Token: 0x06000072 RID: 114 RVA: 0x0000ACA0 File Offset: 0x00008EA0
	private static bool IsInHome()
	{
		int mapID = TileMap.mapID;
		return mapID == 21 || mapID == 22 || mapID == 23;
	}

	// Token: 0x06000073 RID: 115 RVA: 0x0000ACC4 File Offset: 0x00008EC4
	private static int GetSoLuongDau()
	{
		int num = 0;
		for (int i = 0; i < global::Char.myCharz().arrItemBag.Length; i++)
		{
			Item item = global::Char.myCharz().arrItemBag[i];
			if (item != null && item.template.type == 6)
			{
				num += item.quantity;
			}
		}
		return num;
	}

	// Token: 0x06000074 RID: 116 RVA: 0x0000AD14 File Offset: 0x00008F14
	private static void AnDau()
	{
		for (int i = 0; i < global::Char.myCharz().arrItemBag.Length; i++)
		{
			Item item = global::Char.myCharz().arrItemBag[i];
			if (item != null && item.template.type == 6)
			{
				Service.gI().useItem(1, 1, (sbyte)i, -1);
				return;
			}
		}
	}

	// Token: 0x06000075 RID: 117 RVA: 0x0000AD68 File Offset: 0x00008F68
	private static void UpdateZoneStatus(int mapId, int zoneId, string accName, bool foundBoss)
	{
		string key = string.Format("{0}:{1}:", mapId, zoneId);
		Type typeFromHandle = typeof(AutoBroly);
		lock (typeFromHandle)
		{
			if (File.Exists(AutoBroly.AreaControlPath))
			{
				List<string> list = File.ReadAllLines(AutoBroly.AreaControlPath).ToList<string>();
				list = (from l in list
				where !l.StartsWith(key)
				select l).ToList<string>();
				if (foundBoss)
				{
					list.Add(string.Format("{0}:{1}:{2}:holding", mapId, zoneId, accName));
				}
				File.WriteAllLines(AutoBroly.AreaControlPath, list.ToArray());
			}
		}
	}

	// Token: 0x06000076 RID: 118 RVA: 0x0000AE28 File Offset: 0x00009028
	private static bool TryReserveZone(int mapId, int zoneId, string accName)
	{
		string key = string.Format("{0}:{1}:", mapId, zoneId);
		string text = string.Format("{0}:{1}:{2}:checking", mapId, zoneId, accName);
		Type typeFromHandle = typeof(AutoBroly);
		bool result;
		lock (typeFromHandle)
		{
			if (!File.Exists(AutoBroly.AreaControlPath))
			{
				File.WriteAllText(AutoBroly.AreaControlPath, text + Environment.NewLine);
				result = true;
			}
			else
			{
				List<string> list = File.ReadAllLines(AutoBroly.AreaControlPath).ToList<string>();
				if (list.Any((string l) => l.StartsWith(key)))
				{
					result = false;
				}
				else
				{
					list.Add(text);
					File.WriteAllLines(AutoBroly.AreaControlPath, list.ToArray());
					result = true;
				}
			}
		}
		return result;
	}

	// Token: 0x0400004D RID: 77
	public static string TrangThai;

	// Token: 0x0400004E RID: 78
	public static int Map;

	// Token: 0x0400004F RID: 79
	public static int Khu;

	// Token: 0x04000050 RID: 80
	private static bool IsWait;

	// Token: 0x04000051 RID: 81
	private static long TimeStartWait;

	// Token: 0x04000052 RID: 82
	private static long TimeWait;

	// Token: 0x04000053 RID: 83
	public static bool isDoKhu;

	// Token: 0x04000054 RID: 84
	private static HashSet<int> visitedZones;

	// Token: 0x04000055 RID: 85
	private static Random random;

	// Token: 0x04000056 RID: 86
	public static int NhayNe;

	// Token: 0x04000057 RID: 87
	private static long lastAvoidBrolyTime;

	// Token: 0x04000058 RID: 88
	private const int avoidDistance = 100;

	// Token: 0x04000059 RID: 89
	private const int avoidInterval = 500;

	// Token: 0x0400005A RID: 90
	private static string SuperBrolyInfoPath;

	// Token: 0x0400005B RID: 91
	private static string SuperBrolyOwnerPath;

	// Token: 0x0400005C RID: 92
	private static string SuperBrolyBlacklistPath;

	// Token: 0x0400005D RID: 93
	private static string SuperBrolyCallPath = "C:\\Users\\Admin\\Desktop\\QLTK - SP ver 008\\Nro_244_Data\\Resources\\Data\\super_broly_call.txt";

	// Token: 0x0400005E RID: 94
	private static string AreaControlPath;

	// Token: 0x0400005F RID: 95
	private static string BlacklistPath;
}

using System;
using System.IO;
using System.Windows.Forms;

namespace QLTK___SP
{
	// Token: 0x02000002 RID: 2
	internal class Data
	{
		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		// (set) Token: 0x06000002 RID: 2 RVA: 0x00002068 File Offset: 0x00000268
		public DataGridView DataGridView
		{
			get
			{
				return this.dataGridView;
			}
			set
			{
				this.dataGridView = value;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x00002074 File Offset: 0x00000274
		// (set) Token: 0x06000004 RID: 4 RVA: 0x0000208C File Offset: 0x0000028C
		public TextBox Acc
		{
			get
			{
				return this.acc;
			}
			set
			{
				this.acc = value;
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x00002096 File Offset: 0x00000296
		public Data(DataGridView dataGridView, TextBox textBox)
		{
			this.dataGridView = dataGridView;
			this.Acc = textBox;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x000020AF File Offset: 0x000002AF
		public Data(TextBox textBox)
		{
			this.Acc = textBox;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000020C4 File Offset: 0x000002C4
		public void ExporFile()
		{
			for (int i = 0; i < this.dataGridView.Rows.Count; i++)
			{
				this.DataGridView.Rows[i].Cells[0].Value = i;
			}
			TextWriter textWriter = new StreamWriter("Nro_244_Data//Resources//Data//Account.txt");
			for (int j = 0; j < this.dataGridView.Rows.Count; j++)
			{
				for (int k = 0; k < this.dataGridView.Columns.Count - 2; k++)
				{
					textWriter.Write(this.dataGridView.Rows[j].Cells[k].Value.ToString() + "|");
				}
				textWriter.WriteLine("");
			}
			textWriter.Close();
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000021C0 File Offset: 0x000003C0
		public void LoadFile()
		{
			try
			{
				this.dataGridView.Rows.Clear();
				string[] array = File.ReadAllLines("Nro_244_Data//Resources//Data//Account.txt");
				for (int i = 0; i < array.Length; i++)
				{
					string[] array2 = array[i].ToString().Split(new char[]
					{
						'|'
					});
					this.dataGridView.Rows.Add(new object[]
					{
						array2[0],
						array2[1],
						array2[2],
						array2[3],
						array2[4],
						array2[5],
						array2[6]
					});
				}
			}
			catch
			{
			}
		}

		// Token: 0x04000001 RID: 1
		public DataGridView dataGridView;

		// Token: 0x04000002 RID: 2
		private TextBox acc;
	}
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json.Linq;

namespace QLTK___SP
{
	// Token: 0x02000003 RID: 3
	public class Form1 : Form
	{
		// Token: 0x06000009 RID: 9
		[DllImport("user32.dll")]
		private static extern int SetWindowText(IntPtr hWnd, string text);

		// Token: 0x0600000A RID: 10
		[DllImport("user32.dll")]
		private static extern IntPtr FillWindow(string lpClassName, string lpWindowName);

		// Token: 0x0600000B RID: 11
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool MoveWindow(IntPtr hWnd, int x, int y, int nWidth, int nHeight, bool bRepaint);

		// Token: 0x0600000C RID: 12
		[DllImport("user32.dll")]
		private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x0600000D RID: 13
		[DllImport("user32.dll")]
		private static extern bool GetWindowRect(IntPtr hWnd, ref Form1.RECT lpRect);

		// Token: 0x0600000E RID: 14
		[DllImport("user32.dll")]
		private static extern bool IsWindow(IntPtr hWnd);

		// Token: 0x0600000F RID: 15
		[DllImport("user32.dll")]
		private static extern bool IsHungAppWindow(IntPtr hwnd);

		// Token: 0x06000010 RID: 16
		[DllImport("user32.dll")]
		private static extern bool EnumWindows(Form1.EnumWindowsProc lpEnumFunc, IntPtr lParam);

		// Token: 0x06000011 RID: 17
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		// Token: 0x06000012 RID: 18
		[DllImport("user32.dll")]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

		// Token: 0x06000013 RID: 19
		[DllImport("user32.dll")]
		private static extern bool IsWindowVisible(IntPtr hWnd);

		// Token: 0x06000014 RID: 20
		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000015 RID: 21
		[DllImport("user32.dll")]
		private static extern bool DestroyWindow(IntPtr hWnd);

		// Token: 0x06000016 RID: 22
		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		// Token: 0x06000017 RID: 23
		[DllImport("user32.dll")]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		// Token: 0x06000018 RID: 24
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool TerminateProcess(IntPtr hProcess, uint uExitCode);

		// Token: 0x06000019 RID: 25 RVA: 0x00002274 File Offset: 0x00000474
		public Form1()
		{
			this.InitializeComponent();
			this.listener.Start(this.dataGridView1);
		}

		// Token: 0x0600001A RID: 26 RVA: 0x000022EC File Offset: 0x000004EC
		private string GenerateUniqueCodeMAC()
		{
			string macAddress = this.GetMacAddress();
			string name = this.ReadNameFromJson();
			bool flag = string.IsNullOrEmpty(macAddress);
			string result;
			if (flag)
			{
				MessageBox.Show("Không lấy được địa chỉ MAC hợp lệ!");
				result = "ERROR";
			}
			else
			{
				Form1.IP = macAddress;
				Form1.Tool = name;
				using (SHA256 sha256 = SHA256.Create())
				{
					byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(macAddress));
					result = BitConverter.ToString(hashBytes).Replace("-", "").Substring(0, 16);
				}
			}
			return result;
		}

		// Token: 0x0600001B RID: 27 RVA: 0x00002390 File Offset: 0x00000590
		private string GetMacAddress()
		{
			foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
			{
				bool flag = nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback && !string.IsNullOrEmpty(nic.GetPhysicalAddress().ToString());
				if (flag)
				{
					return nic.GetPhysicalAddress().ToString();
				}
			}
			return null;
		}

		// Token: 0x0600001C RID: 28 RVA: 0x00002400 File Offset: 0x00000600
		[DebuggerStepThrough]
		private void ShowRemainingDays()
		{
			Form1.<ShowRemainingDays>d__46 <ShowRemainingDays>d__ = new Form1.<ShowRemainingDays>d__46();
			<ShowRemainingDays>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<ShowRemainingDays>d__.<>4__this = this;
			<ShowRemainingDays>d__.<>1__state = -1;
			<ShowRemainingDays>d__.<>t__builder.Start<Form1.<ShowRemainingDays>d__46>(ref <ShowRemainingDays>d__);
		}

		// Token: 0x0600001D RID: 29 RVA: 0x0000243C File Offset: 0x0000063C
		private SheetsService GetSheetsService()
		{
			SheetsService result;
			try
			{
				string credentialPath = Path.Combine(Application.StartupPath, this.serviceAccountKey);
				bool flag = !File.Exists(credentialPath);
				if (flag)
				{
					result = null;
				}
				else
				{
					GoogleCredential credential;
					using (FileStream stream = new FileStream(credentialPath, FileMode.Open, FileAccess.Read))
					{
						credential = GoogleCredential.FromStream(stream).CreateScoped(new string[]
						{
							SheetsService.Scope.Spreadsheets
						});
					}
					result = new SheetsService(new BaseClientService.Initializer
					{
						HttpClientInitializer = credential,
						ApplicationName = "WinFormsApp"
					});
				}
			}
			catch
			{
				result = null;
			}
			return result;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x000024E8 File Offset: 0x000006E8
		private string ReadNameFromJson()
		{
			try
			{
				bool flag = File.Exists(this.configFilePath);
				if (flag)
				{
					string jsonContent = File.ReadAllText(this.configFilePath);
					JObject jsonObj = JObject.Parse(jsonContent);
					JToken jtoken = jsonObj["name"];
					return ((jtoken != null) ? jtoken.ToString() : null) ?? "";
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi đọc file JSON: " + ex.Message);
			}
			return "";
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002578 File Offset: 0x00000778
		[DebuggerStepThrough]
		private Task CheckKey()
		{
			Form1.<CheckKey>d__50 <CheckKey>d__ = new Form1.<CheckKey>d__50();
			<CheckKey>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CheckKey>d__.<>4__this = this;
			<CheckKey>d__.<>1__state = -1;
			<CheckKey>d__.<>t__builder.Start<Form1.<CheckKey>d__50>(ref <CheckKey>d__);
			return <CheckKey>d__.<>t__builder.Task;
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000025BC File Offset: 0x000007BC
		[return: TupleElementNames(new string[]
		{
			"isSuccess",
			"isValid",
			"remainingDays",
			"endDate",
			"keyExists"
		})]
		private ValueTuple<bool, bool, int, DateTime?, bool> GetRemainingDays(string code)
		{
			ValueTuple<bool, bool, int, DateTime?, bool> result;
			try
			{
				SheetsService service = this.GetSheetsService();
				bool flag = service == null;
				if (flag)
				{
					result = new ValueTuple<bool, bool, int, DateTime?, bool>(false, false, 0, null, false);
				}
				else
				{
					SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(this.spreadsheetId, this.sheetRange);
					ValueRange response = request.Execute();
					bool flag2 = response.Values == null || response.Values.Count == 0;
					if (flag2)
					{
						result = new ValueTuple<bool, bool, int, DateTime?, bool>(true, false, 0, null, false);
					}
					else
					{
						DateTime now = DateTime.Now;
						foreach (IList<object> row in response.Values)
						{
							bool flag3 = row.Count < 3;
							if (!flag3)
							{
								string sheetCode = row[0].ToString().Trim();
								string endDateStr = row[2].ToString().Trim();
								string kh = row[3].ToString().Trim();
								Form1.KhachHang = kh;
								DateTime endDate;
								bool flag4 = sheetCode == code && DateTime.TryParse(endDateStr, out endDate);
								if (flag4)
								{
									int remainingDays = (endDate - now).Days;
									return new ValueTuple<bool, bool, int, DateTime?, bool>(true, now <= endDate, remainingDays, new DateTime?(endDate), true);
								}
							}
						}
						result = new ValueTuple<bool, bool, int, DateTime?, bool>(true, false, 0, null, false);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("[ERROR] Lỗi khi lấy dữ liệu từ Google Sheets: " + ex.Message);
				result = new ValueTuple<bool, bool, int, DateTime?, bool>(false, false, 0, null, false);
			}
			return result;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000027B4 File Offset: 0x000009B4
		public static bool TimeBaoTri()
		{
			DateTime now = DateTime.Now;
			return (now.Hour == 3 && now.Minute >= 10) || (now.Hour == 4 && now.Minute == 0) || File.Exists("baotri");
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000280C File Offset: 0x00000A0C
		[DebuggerStepThrough]
		private void Timer_Tick(object sender, EventArgs e)
		{
			Form1.<Timer_Tick>d__53 <Timer_Tick>d__ = new Form1.<Timer_Tick>d__53();
			<Timer_Tick>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<Timer_Tick>d__.<>4__this = this;
			<Timer_Tick>d__.sender = sender;
			<Timer_Tick>d__.e = e;
			<Timer_Tick>d__.<>1__state = -1;
			<Timer_Tick>d__.<>t__builder.Start<Form1.<Timer_Tick>d__53>(ref <Timer_Tick>d__);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002854 File Offset: 0x00000A54
		private void RemovePIDFromList(int pid)
		{
			int keyToRemove = this.loggedInPIDs.FirstOrDefault((KeyValuePair<int, int> x) => x.Value == pid).Key;
			bool flag = keyToRemove != -1;
			if (flag)
			{
				this.loggedInPIDs.Remove(keyToRemove);
				this.isLoggingIn[keyToRemove] = false;
			}
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000028B4 File Offset: 0x00000AB4
		[DebuggerStepThrough]
		private Task<int> LoginIDAsync(int id)
		{
			Form1.<LoginIDAsync>d__55 <LoginIDAsync>d__ = new Form1.<LoginIDAsync>d__55();
			<LoginIDAsync>d__.<>t__builder = AsyncTaskMethodBuilder<int>.Create();
			<LoginIDAsync>d__.<>4__this = this;
			<LoginIDAsync>d__.id = id;
			<LoginIDAsync>d__.<>1__state = -1;
			<LoginIDAsync>d__.<>t__builder.Start<Form1.<LoginIDAsync>d__55>(ref <LoginIDAsync>d__);
			return <LoginIDAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002900 File Offset: 0x00000B00
		private static void WriteLog(string message)
		{
			string filePath = "Nro_244_Data//Resources//log.txt";
			try
			{
				using (StreamWriter writer = new StreamWriter(filePath, true))
				{
					writer.WriteLine(string.Format("[{0:yyyy-MM-dd HH:mm:ss}] {1}", DateTime.Now, message));
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Không thể ghi log vào file: " + ex.Message);
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00002984 File Offset: 0x00000B84
		private void Form1_Load(object sender, EventArgs e)
		{
			this.Text = "QLTK";
			new Data(this.dataGridView1, this.txtAcc).LoadFile();
			this.isLoggingIn = new bool[this.dataGridView1.Rows.Count];
			this.InitializeTimers();
			bool flag = File.Exists("Port");
			if (flag)
			{
				this.txtPort.Text = File.ReadAllText("Port");
			}
			bool flag2 = File.Exists(Form1.size);
			if (flag2)
			{
				this.txtX.Text = File.ReadAllText(Form1.size).Split(new char[]
				{
					'x'
				})[0];
				this.txtY.Text = File.ReadAllText(Form1.size).Split(new char[]
				{
					'x'
				})[1];
			}
			this.checkBox1.Checked = File.Exists(Form1.dokhu);
			this.checkBox3.Checked = File.Exists("Port");
			this.checkBox2.Checked = File.Exists(Form1.tdlt);
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00002AA0 File Offset: 0x00000CA0
		[DebuggerStepThrough]
		private Task UpdateRowColorsAsync()
		{
			Form1.<UpdateRowColorsAsync>d__59 <UpdateRowColorsAsync>d__ = new Form1.<UpdateRowColorsAsync>d__59();
			<UpdateRowColorsAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<UpdateRowColorsAsync>d__.<>4__this = this;
			<UpdateRowColorsAsync>d__.<>1__state = -1;
			<UpdateRowColorsAsync>d__.<>t__builder.Start<Form1.<UpdateRowColorsAsync>d__59>(ref <UpdateRowColorsAsync>d__);
			return <UpdateRowColorsAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x00002AE4 File Offset: 0x00000CE4
		private void UpdateRowColors()
		{
			foreach (object obj in ((IEnumerable)this.dataGridView1.Rows))
			{
				DataGridViewRow row = (DataGridViewRow)obj;
				bool flag = row.Cells[0].Value == null;
				if (!flag)
				{
					string id = row.Cells[0].Value.ToString();
					string filePath = "Nro_244_Data/Resources/Status/xong" + id;
					bool flag2 = File.Exists(filePath);
					if (flag2)
					{
						row.DefaultCellStyle.BackColor = System.Drawing.Color.LightGreen;
					}
					else
					{
						row.DefaultCellStyle.BackColor = System.Drawing.Color.White;
					}
				}
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00002BBC File Offset: 0x00000DBC
		[DebuggerStepThrough]
		private void LoadTrangThai(object sender, EventArgs e)
		{
			Form1.<LoadTrangThai>d__61 <LoadTrangThai>d__ = new Form1.<LoadTrangThai>d__61();
			<LoadTrangThai>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<LoadTrangThai>d__.<>4__this = this;
			<LoadTrangThai>d__.sender = sender;
			<LoadTrangThai>d__.e = e;
			<LoadTrangThai>d__.<>1__state = -1;
			<LoadTrangThai>d__.<>t__builder.Start<Form1.<LoadTrangThai>d__61>(ref <LoadTrangThai>d__);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x00002C04 File Offset: 0x00000E04
		[DebuggerStepThrough]
		private Task LoadStatusAsync()
		{
			Form1.<LoadStatusAsync>d__62 <LoadStatusAsync>d__ = new Form1.<LoadStatusAsync>d__62();
			<LoadStatusAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<LoadStatusAsync>d__.<>4__this = this;
			<LoadStatusAsync>d__.<>1__state = -1;
			<LoadStatusAsync>d__.<>t__builder.Start<Form1.<LoadStatusAsync>d__62>(ref <LoadStatusAsync>d__);
			return <LoadStatusAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600002B RID: 43 RVA: 0x00002C48 File Offset: 0x00000E48
		[DebuggerStepThrough]
		private void Timer_Tick1()
		{
			Form1.<Timer_Tick1>d__63 <Timer_Tick1>d__ = new Form1.<Timer_Tick1>d__63();
			<Timer_Tick1>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<Timer_Tick1>d__.<>4__this = this;
			<Timer_Tick1>d__.<>1__state = -1;
			<Timer_Tick1>d__.<>t__builder.Start<Form1.<Timer_Tick1>d__63>(ref <Timer_Tick1>d__);
		}

		// Token: 0x0600002C RID: 44 RVA: 0x00002C84 File Offset: 0x00000E84
		private void InitializeTimers()
		{
			this._timer = new Timer
			{
				Interval = 10000
			};
			this._timer.Tick += this.Timer_Tick;
			this.timer = new Timer
			{
				Interval = 1000
			};
			this.timer.Tick += delegate(object s, EventArgs ev)
			{
				Form1.<<InitializeTimers>b__64_0>d <<InitializeTimers>b__64_0>d = new Form1.<<InitializeTimers>b__64_0>d();
				<<InitializeTimers>b__64_0>d.<>t__builder = AsyncVoidMethodBuilder.Create();
				<<InitializeTimers>b__64_0>d.<>4__this = this;
				<<InitializeTimers>b__64_0>d.s = s;
				<<InitializeTimers>b__64_0>d.ev = ev;
				<<InitializeTimers>b__64_0>d.<>1__state = -1;
				<<InitializeTimers>b__64_0>d.<>t__builder.Start<Form1.<<InitializeTimers>b__64_0>d>(ref <<InitializeTimers>b__64_0>d);
			};
			this.timer.Start();
			this.___timer = new Timer
			{
				Interval = 3000
			};
			this.___timer.Tick += delegate(object s, EventArgs ev)
			{
				Form1.<<InitializeTimers>b__64_1>d <<InitializeTimers>b__64_1>d = new Form1.<<InitializeTimers>b__64_1>d();
				<<InitializeTimers>b__64_1>d.<>t__builder = AsyncVoidMethodBuilder.Create();
				<<InitializeTimers>b__64_1>d.<>4__this = this;
				<<InitializeTimers>b__64_1>d.s = s;
				<<InitializeTimers>b__64_1>d.ev = ev;
				<<InitializeTimers>b__64_1>d.<>1__state = -1;
				<<InitializeTimers>b__64_1>d.<>t__builder.Start<Form1.<<InitializeTimers>b__64_1>d>(ref <<InitializeTimers>b__64_1>d);
			};
			this.___timer.Start();
			this.__timer = new Timer
			{
				Interval = 30000
			};
			this.__timer.Tick += delegate(object s, EventArgs ev)
			{
				Form1.<<InitializeTimers>b__64_2>d <<InitializeTimers>b__64_2>d = new Form1.<<InitializeTimers>b__64_2>d();
				<<InitializeTimers>b__64_2>d.<>t__builder = AsyncVoidMethodBuilder.Create();
				<<InitializeTimers>b__64_2>d.<>4__this = this;
				<<InitializeTimers>b__64_2>d.s = s;
				<<InitializeTimers>b__64_2>d.ev = ev;
				<<InitializeTimers>b__64_2>d.<>1__state = -1;
				<<InitializeTimers>b__64_2>d.<>t__builder.Start<Form1.<<InitializeTimers>b__64_2>d>(ref <<InitializeTimers>b__64_2>d);
			};
			this.__timer.Start();
			this.____timer = new Timer
			{
				Interval = 1000
			};
			this.____timer.Tick += this.LoadTrangThai;
			this.____timer.Tick += delegate(object s, EventArgs e)
			{
				Form1.<<InitializeTimers>b__64_3>d <<InitializeTimers>b__64_3>d = new Form1.<<InitializeTimers>b__64_3>d();
				<<InitializeTimers>b__64_3>d.<>t__builder = AsyncVoidMethodBuilder.Create();
				<<InitializeTimers>b__64_3>d.<>4__this = this;
				<<InitializeTimers>b__64_3>d.s = s;
				<<InitializeTimers>b__64_3>d.e = e;
				<<InitializeTimers>b__64_3>d.<>1__state = -1;
				<<InitializeTimers>b__64_3>d.<>t__builder.Start<Form1.<<InitializeTimers>b__64_3>d>(ref <<InitializeTimers>b__64_3>d);
			};
			this.____timer.Start();
		}

		// Token: 0x0600002D RID: 45 RVA: 0x00002DC8 File Offset: 0x00000FC8
		[DebuggerStepThrough]
		public Task FixCrashAsync()
		{
			Form1.<FixCrashAsync>d__65 <FixCrashAsync>d__ = new Form1.<FixCrashAsync>d__65();
			<FixCrashAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<FixCrashAsync>d__.<>4__this = this;
			<FixCrashAsync>d__.<>1__state = -1;
			<FixCrashAsync>d__.<>t__builder.Start<Form1.<FixCrashAsync>d__65>(ref <FixCrashAsync>d__);
			return <FixCrashAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002E0C File Offset: 0x0000100C
		[DebuggerStepThrough]
		private Task CheckTabNotRespondingAsync()
		{
			Form1.<CheckTabNotRespondingAsync>d__66 <CheckTabNotRespondingAsync>d__ = new Form1.<CheckTabNotRespondingAsync>d__66();
			<CheckTabNotRespondingAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<CheckTabNotRespondingAsync>d__.<>4__this = this;
			<CheckTabNotRespondingAsync>d__.<>1__state = -1;
			<CheckTabNotRespondingAsync>d__.<>t__builder.Start<Form1.<CheckTabNotRespondingAsync>d__66>(ref <CheckTabNotRespondingAsync>d__);
			return <CheckTabNotRespondingAsync>d__.<>t__builder.Task;
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002E50 File Offset: 0x00001050
		[DebuggerStepThrough]
		private Task<double> GetCpuUsageAsync(Process process)
		{
			Form1.<GetCpuUsageAsync>d__67 <GetCpuUsageAsync>d__ = new Form1.<GetCpuUsageAsync>d__67();
			<GetCpuUsageAsync>d__.<>t__builder = AsyncTaskMethodBuilder<double>.Create();
			<GetCpuUsageAsync>d__.<>4__this = this;
			<GetCpuUsageAsync>d__.process = process;
			<GetCpuUsageAsync>d__.<>1__state = -1;
			<GetCpuUsageAsync>d__.<>t__builder.Start<Form1.<GetCpuUsageAsync>d__67>(ref <GetCpuUsageAsync>d__);
			return <GetCpuUsageAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002E9C File Offset: 0x0000109C
		public bool CheckAdd(bool Add)
		{
			bool flag = string.IsNullOrEmpty(this.txtAcc.Text);
			bool check;
			if (flag)
			{
				MessageBox.Show("Chưa nhập tài khoản ", "Lỗi Thêm Tài Khoản ", MessageBoxButtons.OK);
				this.txtAcc.Focus();
				check = false;
			}
			else
			{
				bool flag2 = string.IsNullOrEmpty(this.txtPass.Text);
				if (flag2)
				{
					MessageBox.Show("Chưa nhập mật khẩu ", "Lỗi Nhập Mật Khẩu ", MessageBoxButtons.OK);
					this.txtPass.Focus();
					check = false;
				}
				else
				{
					bool flag3 = string.IsNullOrEmpty(this.cboSv.Text);
					if (flag3)
					{
						MessageBox.Show("Chưa nhập Server ", "Lỗi Thêm Server ", MessageBoxButtons.OK);
						this.cboSv.Focus();
						check = false;
					}
					else
					{
						check = true;
					}
				}
			}
			return check;
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002F5C File Offset: 0x0000115C
		private void button1_Click(object sender, EventArgs e)
		{
			bool flag = this.CheckAdd(true);
			if (flag)
			{
				string accPattern = this.txtAcc.Text.Trim();
				bool flag2 = accPattern.Contains("[") && accPattern.Contains("]");
				if (flag2)
				{
					int startIndex = accPattern.IndexOf("[") + 1;
					int endIndex = accPattern.IndexOf("]");
					string range = accPattern.Substring(startIndex, endIndex - startIndex);
					string[] rangeParts = range.Split(new char[]
					{
						'-'
					});
					int start;
					int end;
					bool flag3 = rangeParts.Length == 2 && int.TryParse(rangeParts[0], out start) && int.TryParse(rangeParts[1], out end);
					if (flag3)
					{
						for (int i = start; i <= end; i++)
						{
							string account = accPattern.Substring(0, startIndex - 1) + i.ToString();
							this.dataGridView1.Rows.Add(new object[]
							{
								this.dataGridView1.Rows.Count,
								account,
								this.txtPass.Text,
								this.cboSv.Text,
								this.txtTeam.Text,
								this.cboType.Text,
								string.IsNullOrEmpty(this.txtPrx.Text) ? "none" : this.txtPrx.Text
							});
						}
					}
					else
					{
						MessageBox.Show("Phạm vi không hợp lệ! Định dạng yêu cầu là [start-end].", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Hand);
					}
				}
				else
				{
					this.dataGridView1.Rows.Add(new object[]
					{
						this.dataGridView1.Rows.Count,
						accPattern,
						this.txtPass.Text,
						this.cboSv.Text,
						this.txtTeam.Text,
						this.cboType.Text,
						string.IsNullOrEmpty(this.txtPrx.Text) ? "none" : this.txtPrx.Text
					});
				}
				new Data(this.dataGridView1, this.txtAcc).ExporFile();
			}
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000031B0 File Offset: 0x000013B0
		private void button2_Click(object sender, EventArgs e)
		{
			bool flag = this.CheckAdd(false);
			if (flag)
			{
				int index = this.dataGridView1.CurrentRow.Index;
				this.dataGridView1.Rows[index].Cells[1].Value = this.txtAcc.Text;
				this.dataGridView1.Rows[index].Cells[2].Value = this.txtPass.Text;
				this.dataGridView1.Rows[index].Cells[3].Value = this.cboSv.Text;
				this.dataGridView1.Rows[index].Cells[4].Value = this.txtTeam.Text;
				this.dataGridView1.Rows[index].Cells[5].Value = this.cboType.Text;
				this.dataGridView1.Rows[index].Cells[6].Value = (string.IsNullOrEmpty(this.txtPrx.Text) ? "none" : this.txtPrx.Text);
				new Data(this.dataGridView1, this.txtAcc).ExporFile();
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003320 File Offset: 0x00001520
		private void button3_Click(object sender, EventArgs e)
		{
			bool flag = this.dataGridView1.Rows.Count > 0;
			if (flag)
			{
				this.dataGridView1.Rows.RemoveAt(this.dataGridView1.CurrentRow.Index);
				new Data(this.dataGridView1, this.txtAcc).ExporFile();
			}
			else
			{
				MessageBox.Show("Chưa có tài khoản ", "Lỗi xóa tài khoản ", MessageBoxButtons.OK);
			}
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00003394 File Offset: 0x00001594
		private void button4_Click(object sender, EventArgs e)
		{
			this._timer.Start();
			this.button4.Enabled = false;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000033B0 File Offset: 0x000015B0
		private void button5_Click(object sender, EventArgs e)
		{
			this._timer.Stop();
			this.button4.Enabled = true;
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000033CC File Offset: 0x000015CC
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			bool @checked = this.checkBox1.Checked;
			if (@checked)
			{
				File.Create(Form1.dokhu).Close();
			}
			else
			{
				bool flag = File.Exists(Form1.dokhu);
				if (flag)
				{
					File.Delete(Form1.dokhu);
				}
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x0000341C File Offset: 0x0000161C
		private void checkBox3_CheckedChanged(object sender, EventArgs e)
		{
			bool @checked = this.checkBox3.Checked;
			if (@checked)
			{
				File.WriteAllText("Port", this.txtPort.Text);
				this.txtPort.Enabled = false;
			}
			else
			{
				this.txtPort.Enabled = true;
			}
		}

		// Token: 0x06000038 RID: 56 RVA: 0x00003470 File Offset: 0x00001670
		private void button9_Click(object sender, EventArgs e)
		{
			string folderPath = "Nro_244_Data/Resources/Status/";
			string searchPattern = "xong*";
			try
			{
				string[] files = Directory.GetFiles(folderPath, searchPattern);
				foreach (string file in files)
				{
					File.Delete(file);
				}
				MessageBox.Show("Reset data successfully", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Lỗi khi xóa file: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003504 File Offset: 0x00001704
		private void button7_Click(object sender, EventArgs e)
		{
			string titleToClose = "BROLY: ";
			DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn tắt tất cả các tab game không?", "Xác nhận tắt tab", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
			bool flag = dialogResult == DialogResult.Yes;
			if (flag)
			{
				Process[] processes = Process.GetProcesses();
				foreach (Process process in processes)
				{
					try
					{
						bool flag2 = process.MainWindowTitle.Contains(titleToClose);
						if (flag2)
						{
							process.Kill();
							Form1.WriteLog(string.Format("Đã tắt tiến trình: {0} (ID: {1})", process.ProcessName, process.Id));
						}
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x000035B4 File Offset: 0x000017B4
		private void button8_Click(object sender, EventArgs e)
		{
			string localLowPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AppData\\LocalLow\\Team\\ragonboy244");
			DialogResult dialogResult = MessageBox.Show("Bạn có chắc chắn muốn xóa dữ liệu game không?", "Xóa dữ liệu Game???", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
			bool flag = dialogResult == DialogResult.Yes;
			if (flag)
			{
				try
				{
					bool flag2 = Directory.Exists(localLowPath);
					if (flag2)
					{
						string[] files = Directory.GetFileSystemEntries(localLowPath);
						foreach (string file in files)
						{
							bool flag3 = File.Exists(file);
							if (flag3)
							{
								File.Delete(file);
							}
							else
							{
								bool flag4 = Directory.Exists(file);
								if (flag4)
								{
									Directory.Delete(file, true);
								}
							}
						}
						MessageBox.Show("Đã xóa các tệp không cần thiết thành công.");
					}
					else
					{
						MessageBox.Show("Thư mục không tồn tại.");
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
				}
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000036A8 File Offset: 0x000018A8
		[DebuggerStepThrough]
		private void button6_Click(object sender, EventArgs e)
		{
			Form1.<button6_Click>d__80 <button6_Click>d__ = new Form1.<button6_Click>d__80();
			<button6_Click>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<button6_Click>d__.<>4__this = this;
			<button6_Click>d__.sender = sender;
			<button6_Click>d__.e = e;
			<button6_Click>d__.<>1__state = -1;
			<button6_Click>d__.<>t__builder.Start<Form1.<button6_Click>d__80>(ref <button6_Click>d__);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000036F0 File Offset: 0x000018F0
		private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			int index = this.dataGridView1.CurrentRow.Index;
			this.txtAcc.Text = this.dataGridView1.Rows[index].Cells[1].Value.ToString();
			this.txtPass.Text = this.dataGridView1.Rows[index].Cells[2].Value.ToString();
			this.cboSv.Text = this.dataGridView1.Rows[index].Cells[3].Value.ToString();
			this.txtTeam.Text = this.dataGridView1.Rows[index].Cells[4].Value.ToString();
			this.cboType.Text = this.dataGridView1.Rows[index].Cells[5].Value.ToString();
			this.txtPrx.Text = this.dataGridView1.Rows[index].Cells[6].Value.ToString();
		}

		// Token: 0x0600003D RID: 61 RVA: 0x0000383C File Offset: 0x00001A3C
		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			bool @checked = this.checkBox2.Checked;
			if (@checked)
			{
				File.Create(Form1.tdlt).Close();
			}
			else
			{
				bool flag = File.Exists(Form1.tdlt);
				if (flag)
				{
					File.Delete(Form1.tdlt);
				}
			}
		}

		// Token: 0x0600003E RID: 62 RVA: 0x0000388C File Offset: 0x00001A8C
		private void SaveTextToFile(string filePath, string text)
		{
			try
			{
				File.WriteAllText(filePath, text);
			}
			catch
			{
			}
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000038BC File Offset: 0x00001ABC
		private void button10_Click(object sender, EventArgs e)
		{
			try
			{
				this.SaveTextToFile(Form1.size, this.txtX.Text + "x" + this.txtY.Text);
				MessageBox.Show("Ok");
			}
			catch
			{
			}
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000391C File Offset: 0x00001B1C
		private void button11_Click(object sender, EventArgs e)
		{
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.Filter = "Text files (*.txt)|*.txt";
				openFileDialog.Title = "Chọn tệp TXT";
				bool flag = openFileDialog.ShowDialog() == DialogResult.OK;
				if (flag)
				{
					string[] lines = File.ReadAllLines(openFileDialog.FileName);
					foreach (string line in lines)
					{
						string[] parts = line.Split(new char[]
						{
							'|'
						});
						bool flag2 = parts.Length == 3;
						if (flag2)
						{
							this.dataGridView1.Rows.Add(new object[]
							{
								"",
								parts[0],
								parts[1],
								parts[2],
								"1",
								"3.Clone - sơ sinh",
								"none"
							});
							new Data(this.dataGridView1, this.txtAcc).ExporFile();
						}
					}
				}
			}
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00003A30 File Offset: 0x00001C30
		private void button12_Click(object sender, EventArgs e)
		{
			foreach (object obj in this.dataGridView1.SelectedRows)
			{
				DataGridViewRow row = (DataGridViewRow)obj;
				bool flag = !row.IsNewRow;
				if (flag)
				{
					row.Cells[8].Value = true;
				}
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00003AB4 File Offset: 0x00001CB4
		private void button13_Click(object sender, EventArgs e)
		{
			foreach (object obj in this.dataGridView1.SelectedRows)
			{
				DataGridViewRow row = (DataGridViewRow)obj;
				bool flag = !row.IsNewRow;
				if (flag)
				{
					row.Cells[8].Value = false;
				}
			}
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00003B38 File Offset: 0x00001D38
		[DebuggerStepThrough]
		private Task<string> ReadFileTextAsync(string path)
		{
			Form1.<ReadFileTextAsync>d__89 <ReadFileTextAsync>d__ = new Form1.<ReadFileTextAsync>d__89();
			<ReadFileTextAsync>d__.<>t__builder = AsyncTaskMethodBuilder<string>.Create();
			<ReadFileTextAsync>d__.<>4__this = this;
			<ReadFileTextAsync>d__.path = path;
			<ReadFileTextAsync>d__.<>1__state = -1;
			<ReadFileTextAsync>d__.<>t__builder.Start<Form1.<ReadFileTextAsync>d__89>(ref <ReadFileTextAsync>d__);
			return <ReadFileTextAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00003B84 File Offset: 0x00001D84
		[DebuggerStepThrough]
		private Task SapXepAsync()
		{
			Form1.<SapXepAsync>d__90 <SapXepAsync>d__ = new Form1.<SapXepAsync>d__90();
			<SapXepAsync>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SapXepAsync>d__.<>4__this = this;
			<SapXepAsync>d__.<>1__state = -1;
			<SapXepAsync>d__.<>t__builder.Start<Form1.<SapXepAsync>d__90>(ref <SapXepAsync>d__);
			return <SapXepAsync>d__.<>t__builder.Task;
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00003BC8 File Offset: 0x00001DC8
		[DebuggerStepThrough]
		private Task SapXepAsyncPhu()
		{
			Form1.<SapXepAsyncPhu>d__91 <SapXepAsyncPhu>d__ = new Form1.<SapXepAsyncPhu>d__91();
			<SapXepAsyncPhu>d__.<>t__builder = AsyncTaskMethodBuilder.Create();
			<SapXepAsyncPhu>d__.<>4__this = this;
			<SapXepAsyncPhu>d__.<>1__state = -1;
			<SapXepAsyncPhu>d__.<>t__builder.Start<Form1.<SapXepAsyncPhu>d__91>(ref <SapXepAsyncPhu>d__);
			return <SapXepAsyncPhu>d__.<>t__builder.Task;
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00003C0C File Offset: 0x00001E0C
		[DebuggerStepThrough]
		private void button14_Click(object sender, EventArgs e)
		{
			Form1.<button14_Click>d__92 <button14_Click>d__ = new Form1.<button14_Click>d__92();
			<button14_Click>d__.<>t__builder = AsyncVoidMethodBuilder.Create();
			<button14_Click>d__.<>4__this = this;
			<button14_Click>d__.sender = sender;
			<button14_Click>d__.e = e;
			<button14_Click>d__.<>1__state = -1;
			<button14_Click>d__.<>t__builder.Start<Form1.<button14_Click>d__92>(ref <button14_Click>d__);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00003C54 File Offset: 0x00001E54
		private void button15_Click(object sender, EventArgs e)
		{
			bool flag = !File.Exists("proxy.txt");
			if (flag)
			{
				MessageBox.Show("Không tìm thấy file.");
			}
			else
			{
				int soLanLap;
				bool flag2 = !int.TryParse(this.textBox1.Text, out soLanLap) || soLanLap <= 0;
				if (flag2)
				{
					MessageBox.Show("Ghi số dòng chuẩn chưa >>>> \n Ghi 10 dòng thì thêm dòng đầu cho 10 acc...\n :D");
					this.textBox1.Focus();
				}
				else
				{
					string[] lines = File.ReadAllLines("proxy.txt");
					bool flag3 = lines.Length == 0;
					if (flag3)
					{
						MessageBox.Show("File không có nội dung.");
					}
					else
					{
						int currentRowIndex = 0;
						int lineIndex = 0;
						while (currentRowIndex < this.dataGridView1.Rows.Count)
						{
							bool isNewRow = this.dataGridView1.Rows[currentRowIndex].IsNewRow;
							if (isNewRow)
							{
								currentRowIndex++;
							}
							else
							{
								int i = 0;
								while (i < soLanLap && currentRowIndex < this.dataGridView1.Rows.Count)
								{
									bool flag4 = !this.dataGridView1.Rows[currentRowIndex].IsNewRow;
									if (flag4)
									{
										this.dataGridView1.Rows[currentRowIndex].Cells[6].Value = lines[lineIndex];
									}
									currentRowIndex++;
									i++;
								}
								lineIndex++;
								bool flag5 = lineIndex >= lines.Length;
								if (flag5)
								{
									lineIndex = 0;
								}
							}
						}
						new Data(this.dataGridView1, this.txtAcc).ExporFile();
						MessageBox.Show("Hoàn tất.");
					}
				}
			}
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00003DEC File Offset: 0x00001FEC
		protected override void Dispose(bool disposing)
		{
			bool flag = disposing && this.components != null;
			if (flag)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00003E24 File Offset: 0x00002024
		private void InitializeComponent()
		{
			this.components = new Container();
			ComponentResourceManager resources = new ComponentResourceManager(typeof(Form1));
			this.dataGridView1 = new DataGridView();
			this.groupBox1 = new GroupBox();
			this.button3 = new Button();
			this.button2 = new Button();
			this.button1 = new Button();
			this.txtTeam = new TextBox();
			this.label6 = new Label();
			this.label5 = new Label();
			this.txtPrx = new TextBox();
			this.label4 = new Label();
			this.label3 = new Label();
			this.label2 = new Label();
			this.label1 = new Label();
			this.cboType = new ComboBox();
			this.cboSv = new ComboBox();
			this.txtPass = new TextBox();
			this.txtAcc = new TextBox();
			this.groupBox2 = new GroupBox();
			this.label7 = new Label();
			this.textBox1 = new TextBox();
			this.button15 = new Button();
			this.button14 = new Button();
			this.button13 = new Button();
			this.button12 = new Button();
			this.button11 = new Button();
			this.label10 = new Label();
			this.button10 = new Button();
			this.txtY = new TextBox();
			this.txtX = new TextBox();
			this.checkBox2 = new CheckBox();
			this.button9 = new Button();
			this.checkBox3 = new CheckBox();
			this.txtPort = new DomainUpDown();
			this.label8 = new Label();
			this.panel1 = new Panel();
			this.label9 = new Label();
			this.button8 = new Button();
			this.button7 = new Button();
			this.button6 = new Button();
			this.button5 = new Button();
			this.button4 = new Button();
			this.checkBox1 = new CheckBox();
			this.timer1 = new Timer(this.components);
			this.Column1 = new DataGridViewTextBoxColumn();
			this.Column2 = new DataGridViewTextBoxColumn();
			this.Column3 = new DataGridViewTextBoxColumn();
			this.Column4 = new DataGridViewTextBoxColumn();
			this.Column5 = new DataGridViewTextBoxColumn();
			this.Column6 = new DataGridViewTextBoxColumn();
			this.Column7 = new DataGridViewTextBoxColumn();
			this.Column8 = new DataGridViewTextBoxColumn();
			this.Column9 = new DataGridViewCheckBoxColumn();
			((ISupportInitialize)this.dataGridView1).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.panel1.SuspendLayout();
			base.SuspendLayout();
			this.dataGridView1.AllowUserToAddRows = false;
			this.dataGridView1.AllowUserToDeleteRows = false;
			this.dataGridView1.AllowUserToOrderColumns = true;
			this.dataGridView1.AllowUserToResizeColumns = false;
			this.dataGridView1.AllowUserToResizeRows = false;
			this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dataGridView1.Columns.AddRange(new DataGridViewColumn[]
			{
				this.Column1,
				this.Column2,
				this.Column3,
				this.Column4,
				this.Column5,
				this.Column6,
				this.Column7,
				this.Column8,
				this.Column9
			});
			this.dataGridView1.EnableHeadersVisualStyles = false;
			this.dataGridView1.Location = new Point(12, 12);
			this.dataGridView1.Name = "dataGridView1";
			this.dataGridView1.RowHeadersVisible = false;
			this.dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
			this.dataGridView1.Size = new Size(668, 248);
			this.dataGridView1.TabIndex = 0;
			this.dataGridView1.CellClick += this.dataGridView1_CellClick;
			this.groupBox1.Controls.Add(this.button3);
			this.groupBox1.Controls.Add(this.button2);
			this.groupBox1.Controls.Add(this.button1);
			this.groupBox1.Controls.Add(this.txtTeam);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.txtPrx);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.cboType);
			this.groupBox1.Controls.Add(this.cboSv);
			this.groupBox1.Controls.Add(this.txtPass);
			this.groupBox1.Controls.Add(this.txtAcc);
			this.groupBox1.Location = new Point(13, 268);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new Size(277, 270);
			this.groupBox1.TabIndex = 1;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Account";
			this.button3.Location = new Point(184, 226);
			this.button3.Name = "button3";
			this.button3.Size = new Size(75, 30);
			this.button3.TabIndex = 26;
			this.button3.Text = "Xóa";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += this.button3_Click;
			this.button2.Location = new Point(103, 226);
			this.button2.Name = "button2";
			this.button2.Size = new Size(75, 31);
			this.button2.TabIndex = 25;
			this.button2.Text = "Sửa ";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += this.button2_Click;
			this.button1.Location = new Point(22, 226);
			this.button1.Name = "button1";
			this.button1.Size = new Size(75, 30);
			this.button1.TabIndex = 24;
			this.button1.Text = "Thêm";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += this.button1_Click;
			this.txtTeam.Location = new Point(72, 128);
			this.txtTeam.Name = "txtTeam";
			this.txtTeam.Size = new Size(174, 20);
			this.txtTeam.TabIndex = 23;
			this.label6.AutoSize = true;
			this.label6.Location = new Point(17, 131);
			this.label6.Name = "label6";
			this.label6.Size = new Size(34, 13);
			this.label6.TabIndex = 22;
			this.label6.Text = "Team";
			this.label5.AutoSize = true;
			this.label5.Location = new Point(19, 191);
			this.label5.Name = "label5";
			this.label5.Size = new Size(33, 13);
			this.label5.TabIndex = 21;
			this.label5.Text = "Proxy";
			this.txtPrx.Location = new Point(72, 188);
			this.txtPrx.Name = "txtPrx";
			this.txtPrx.Size = new Size(174, 20);
			this.txtPrx.TabIndex = 12;
			this.label4.AutoSize = true;
			this.label4.Location = new Point(19, 164);
			this.label4.Name = "label4";
			this.label4.Size = new Size(31, 13);
			this.label4.TabIndex = 20;
			this.label4.Text = "Type";
			this.label3.AutoSize = true;
			this.label3.Location = new Point(13, 102);
			this.label3.Name = "label3";
			this.label3.Size = new Size(48, 13);
			this.label3.TabIndex = 19;
			this.label3.Text = "Máy chủ";
			this.label2.AutoSize = true;
			this.label2.Location = new Point(10, 62);
			this.label2.Name = "label2";
			this.label2.Size = new Size(52, 13);
			this.label2.TabIndex = 18;
			this.label2.Text = "Mật khẩu";
			this.label1.AutoSize = true;
			this.label1.Location = new Point(7, 26);
			this.label1.Name = "label1";
			this.label1.Size = new Size(55, 13);
			this.label1.TabIndex = 17;
			this.label1.Text = "Tài khoản";
			this.cboType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cboType.FormattingEnabled = true;
			this.cboType.Items.AddRange(new object[]
			{
				"1.Acc chính ",
				"2.Acc phụ",
				"3.Clone - sơ sinh"
			});
			this.cboType.Location = new Point(72, 156);
			this.cboType.Name = "cboType";
			this.cboType.Size = new Size(174, 21);
			this.cboType.TabIndex = 16;
			this.cboSv.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cboSv.FormattingEnabled = true;
			this.cboSv.Items.AddRange(new object[]
			{
				"1",
				"2",
				"3",
				"4",
				"5",
				"6",
				"7",
				"8",
				"9",
				"10",
				"11",
				"12",
				"13",
				"14",
				"15",
				"16",
				"17",
				"18",
				"19",
				"20",
				"21",
				"22",
				"23",
				"24",
				"25",
				"26",
				"27",
				"28",
				"29",
				"30"
			});
			this.cboSv.Location = new Point(72, 94);
			this.cboSv.Name = "cboSv";
			this.cboSv.Size = new Size(174, 21);
			this.cboSv.TabIndex = 15;
			this.txtPass.Location = new Point(72, 55);
			this.txtPass.Name = "txtPass";
			this.txtPass.Size = new Size(174, 20);
			this.txtPass.TabIndex = 14;
			this.txtPass.UseSystemPasswordChar = true;
			this.txtAcc.Location = new Point(72, 19);
			this.txtAcc.Name = "txtAcc";
			this.txtAcc.Size = new Size(174, 20);
			this.txtAcc.TabIndex = 13;
			this.groupBox2.Controls.Add(this.label7);
			this.groupBox2.Controls.Add(this.textBox1);
			this.groupBox2.Controls.Add(this.button15);
			this.groupBox2.Controls.Add(this.button14);
			this.groupBox2.Controls.Add(this.button13);
			this.groupBox2.Controls.Add(this.button12);
			this.groupBox2.Controls.Add(this.button11);
			this.groupBox2.Controls.Add(this.label10);
			this.groupBox2.Controls.Add(this.button10);
			this.groupBox2.Controls.Add(this.txtY);
			this.groupBox2.Controls.Add(this.txtX);
			this.groupBox2.Controls.Add(this.checkBox2);
			this.groupBox2.Controls.Add(this.button9);
			this.groupBox2.Controls.Add(this.checkBox3);
			this.groupBox2.Controls.Add(this.txtPort);
			this.groupBox2.Controls.Add(this.label8);
			this.groupBox2.Controls.Add(this.panel1);
			this.groupBox2.Controls.Add(this.button8);
			this.groupBox2.Controls.Add(this.button7);
			this.groupBox2.Controls.Add(this.button6);
			this.groupBox2.Controls.Add(this.button5);
			this.groupBox2.Controls.Add(this.button4);
			this.groupBox2.Controls.Add(this.checkBox1);
			this.groupBox2.Location = new Point(297, 268);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new Size(383, 270);
			this.groupBox2.TabIndex = 2;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Setting";
			this.label7.AutoSize = true;
			this.label7.Location = new Point(346, 208);
			this.label7.Name = "label7";
			this.label7.Size = new Size(31, 13);
			this.label7.TabIndex = 27;
			this.label7.Text = "dòng";
			this.textBox1.Location = new Point(315, 201);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new Size(29, 20);
			this.textBox1.TabIndex = 26;
			this.button15.Location = new Point(201, 198);
			this.button15.Name = "button15";
			this.button15.Size = new Size(107, 23);
			this.button15.TabIndex = 25;
			this.button15.Text = "Them Proxy Nhanh";
			this.button15.UseVisualStyleBackColor = true;
			this.button15.Click += this.button15_Click;
			this.button14.Location = new Point(23, 233);
			this.button14.Name = "button14";
			this.button14.Size = new Size(75, 23);
			this.button14.TabIndex = 24;
			this.button14.Text = "Sắp Xếp";
			this.button14.UseVisualStyleBackColor = true;
			this.button14.Click += this.button14_Click;
			this.button13.Location = new Point(185, 57);
			this.button13.Name = "button13";
			this.button13.Size = new Size(140, 23);
			this.button13.TabIndex = 23;
			this.button13.Text = "Bỏ tick Tài khỏa đã chọn";
			this.button13.UseVisualStyleBackColor = true;
			this.button13.Click += this.button13_Click;
			this.button12.Location = new Point(25, 57);
			this.button12.Name = "button12";
			this.button12.Size = new Size(140, 23);
			this.button12.TabIndex = 22;
			this.button12.Text = "Tick Tài Khoản đã chọn";
			this.button12.UseVisualStyleBackColor = true;
			this.button12.Click += this.button12_Click;
			this.button11.Location = new Point(120, 198);
			this.button11.Name = "button11";
			this.button11.Size = new Size(75, 23);
			this.button11.TabIndex = 21;
			this.button11.Text = "Thêm File";
			this.button11.UseVisualStyleBackColor = true;
			this.button11.Click += this.button11_Click;
			this.label10.AutoSize = true;
			this.label10.Location = new Point(260, 239);
			this.label10.Name = "label10";
			this.label10.Size = new Size(14, 13);
			this.label10.TabIndex = 19;
			this.label10.Text = "X";
			this.button10.Location = new Point(331, 235);
			this.button10.Name = "button10";
			this.button10.Size = new Size(38, 23);
			this.button10.TabIndex = 18;
			this.button10.Text = "Size";
			this.button10.UseVisualStyleBackColor = true;
			this.button10.Click += this.button10_Click;
			this.txtY.Location = new Point(280, 237);
			this.txtY.Name = "txtY";
			this.txtY.Size = new Size(45, 20);
			this.txtY.TabIndex = 17;
			this.txtX.Location = new Point(216, 237);
			this.txtX.Name = "txtX";
			this.txtX.Size = new Size(42, 20);
			this.txtX.TabIndex = 16;
			this.checkBox2.AutoSize = true;
			this.checkBox2.Location = new Point(122, 175);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new Size(54, 17);
			this.checkBox2.TabIndex = 15;
			this.checkBox2.Text = "TDLT";
			this.checkBox2.UseVisualStyleBackColor = true;
			this.checkBox2.CheckedChanged += this.checkBox2_CheckedChanged;
			this.button9.Location = new Point(300, 24);
			this.button9.Name = "button9";
			this.button9.Size = new Size(75, 23);
			this.button9.TabIndex = 14;
			this.button9.Text = "Reset";
			this.button9.UseVisualStyleBackColor = true;
			this.button9.Click += this.button9_Click;
			this.checkBox3.AutoSize = true;
			this.checkBox3.Location = new Point(205, 177);
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = new Size(45, 17);
			this.checkBox3.TabIndex = 13;
			this.checkBox3.Text = "Port";
			this.checkBox3.UseVisualStyleBackColor = true;
			this.checkBox3.CheckedChanged += this.checkBox3_CheckedChanged;
			this.txtPort.Location = new Point(262, 174);
			this.txtPort.Name = "txtPort";
			this.txtPort.Size = new Size(93, 20);
			this.txtPort.TabIndex = 12;
			this.label8.AutoSize = true;
			this.label8.Location = new Point(22, 94);
			this.label8.Name = "label8";
			this.label8.Size = new Size(59, 13);
			this.label8.TabIndex = 0;
			this.label8.Text = "Thông báo";
			this.panel1.BackColor = SystemColors.AppWorkspace;
			this.panel1.Controls.Add(this.label9);
			this.panel1.Location = new Point(23, 102);
			this.panel1.Name = "panel1";
			this.panel1.Size = new Size(332, 67);
			this.panel1.TabIndex = 10;
			this.label9.AutoSize = true;
			this.label9.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 163);
			this.label9.ForeColor = System.Drawing.Color.Snow;
			this.label9.Location = new Point(3, 15);
			this.label9.Name = "label9";
			this.label9.Size = new Size(27, 16);
			this.label9.TabIndex = 0;
			this.label9.Text = "null";
			this.button8.Location = new Point(25, 198);
			this.button8.Name = "button8";
			this.button8.Size = new Size(89, 23);
			this.button8.TabIndex = 9;
			this.button8.Text = "Xóa dl game";
			this.button8.UseVisualStyleBackColor = true;
			this.button8.Click += this.button8_Click;
			this.button7.Location = new Point(113, 233);
			this.button7.Name = "button7";
			this.button7.Size = new Size(75, 23);
			this.button7.TabIndex = 8;
			this.button7.Text = "Close all";
			this.button7.UseVisualStyleBackColor = true;
			this.button7.Click += this.button7_Click;
			this.button6.Location = new Point(23, 21);
			this.button6.Name = "button6";
			this.button6.Size = new Size(109, 28);
			this.button6.TabIndex = 7;
			this.button6.Text = "Mở tab đang chọn";
			this.button6.UseVisualStyleBackColor = true;
			this.button6.Click += this.button6_Click;
			this.button5.Location = new Point(219, 24);
			this.button5.Name = "button5";
			this.button5.Size = new Size(75, 23);
			this.button5.TabIndex = 6;
			this.button5.Text = "Tắt";
			this.button5.UseVisualStyleBackColor = true;
			this.button5.Click += this.button5_Click;
			this.button4.Location = new Point(138, 24);
			this.button4.Name = "button4";
			this.button4.Size = new Size(75, 23);
			this.button4.TabIndex = 5;
			this.button4.Text = "Bật";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += this.button4_Click;
			this.checkBox1.AutoSize = true;
			this.checkBox1.Location = new Point(23, 175);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new Size(91, 17);
			this.checkBox1.TabIndex = 1;
			this.checkBox1.Text = "Dò + báo khu";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += this.checkBox1_CheckedChanged;
			this.Column1.HeaderText = "ID";
			this.Column1.Name = "Column1";
			this.Column1.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.Column1.Width = 30;
			this.Column2.HeaderText = "Account";
			this.Column2.Name = "Column2";
			this.Column2.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.Column2.Width = 130;
			this.Column3.HeaderText = "PassWord";
			this.Column3.Name = "Column3";
			this.Column3.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.Column3.Visible = false;
			this.Column4.HeaderText = "Server";
			this.Column4.Name = "Column4";
			this.Column4.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.Column4.Width = 60;
			this.Column5.HeaderText = "Team";
			this.Column5.Name = "Column5";
			this.Column5.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.Column5.Width = 60;
			this.Column6.HeaderText = "Type";
			this.Column6.Name = "Column6";
			this.Column6.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.Column6.Width = 80;
			this.Column7.HeaderText = "Proxy";
			this.Column7.Name = "Column7";
			this.Column7.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.Column7.Width = 70;
			this.Column8.HeaderText = "Trạng Thái ";
			this.Column8.Name = "Column8";
			this.Column8.SortMode = DataGridViewColumnSortMode.NotSortable;
			this.Column8.Width = 180;
			this.Column9.HeaderText = "X";
			this.Column9.Name = "Column9";
			this.Column9.Width = 40;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(696, 548);
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.dataGridView1);
			base.Icon = (Icon)resources.GetObject("$this.Icon");
			base.Name = "Form1";
			base.StartPosition = FormStartPosition.CenterScreen;
			this.Text = "QLTK - SP";
			base.Load += this.Form1_Load;
			((ISupportInitialize)this.dataGridView1).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			base.ResumeLayout(false);
		}

		// Token: 0x04000003 RID: 3
		private const int SW_MINIMIZE = 6;

		// Token: 0x04000004 RID: 4
		private const int SW_RESTORE = 9;

		// Token: 0x04000005 RID: 5
		private const uint WM_CLOSE = 16U;

		// Token: 0x04000006 RID: 6
		public static string FileResource = "Nro_244_Data//Resources//Data";

		// Token: 0x04000007 RID: 7
		public static string AppGameExe = "Nro_244.exe";

		// Token: 0x04000008 RID: 8
		public static int PORT;

		// Token: 0x04000009 RID: 9
		private Timer timer;

		// Token: 0x0400000A RID: 10
		private Timer _timer;

		// Token: 0x0400000B RID: 11
		private Timer __timer;

		// Token: 0x0400000C RID: 12
		private Timer ___timer;

		// Token: 0x0400000D RID: 13
		private Timer ____timer;

		// Token: 0x0400000E RID: 14
		private bool[] isLoggingIn;

		// Token: 0x0400000F RID: 15
		private Dictionary<int, int> loggedInPIDs = new Dictionary<int, int>();

		// Token: 0x04000010 RID: 16
		private SocketOutPut listener = new SocketOutPut();

		// Token: 0x04000011 RID: 17
		private Timer checkTimer;

		// Token: 0x04000012 RID: 18
		private string spreadsheetId = "1__3pRCfIXpfo_UzqVhTHH9wxGVeVPNUkMlftrFACLHE";

		// Token: 0x04000013 RID: 19
		private string sheetRange = "KeyTools";

		// Token: 0x04000014 RID: 20
		private string serviceAccountKey = "credentials.json";

		// Token: 0x04000015 RID: 21
		private string currentCode;

		// Token: 0x04000016 RID: 22
		private string configFilePath = "sevice-setup.json";

		// Token: 0x04000017 RID: 23
		public static string IP;

		// Token: 0x04000018 RID: 24
		public static string Tool;

		// Token: 0x04000019 RID: 25
		public static string Ver;

		// Token: 0x0400001A RID: 26
		public static string KhachHang;

		// Token: 0x0400001B RID: 27
		public static string NameTool = "QLTK - SP Broly";

		// Token: 0x0400001C RID: 28
		public static int lastcheck = 0;

		// Token: 0x0400001D RID: 29
		public static string size = "Nro_244_Data//Resources//Data//size.txt";

		// Token: 0x0400001E RID: 30
		public static string dokhu = "Nro_244_Data//Resources//dokhu";

		// Token: 0x0400001F RID: 31
		public static string tdlt = "Nro_244_Data//Resources//tdlt";

		// Token: 0x04000020 RID: 32
		private IContainer components = null;

		// Token: 0x04000021 RID: 33
		private DataGridView dataGridView1;

		// Token: 0x04000022 RID: 34
		private GroupBox groupBox1;

		// Token: 0x04000023 RID: 35
		private Label label5;

		// Token: 0x04000024 RID: 36
		private TextBox txtPrx;

		// Token: 0x04000025 RID: 37
		private Label label4;

		// Token: 0x04000026 RID: 38
		private Label label3;

		// Token: 0x04000027 RID: 39
		private Label label2;

		// Token: 0x04000028 RID: 40
		private Label label1;

		// Token: 0x04000029 RID: 41
		private ComboBox cboType;

		// Token: 0x0400002A RID: 42
		private ComboBox cboSv;

		// Token: 0x0400002B RID: 43
		private TextBox txtPass;

		// Token: 0x0400002C RID: 44
		private TextBox txtAcc;

		// Token: 0x0400002D RID: 45
		private Button button3;

		// Token: 0x0400002E RID: 46
		private Button button2;

		// Token: 0x0400002F RID: 47
		private Button button1;

		// Token: 0x04000030 RID: 48
		private TextBox txtTeam;

		// Token: 0x04000031 RID: 49
		private Label label6;

		// Token: 0x04000032 RID: 50
		private GroupBox groupBox2;

		// Token: 0x04000033 RID: 51
		private CheckBox checkBox1;

		// Token: 0x04000034 RID: 52
		private Panel panel1;

		// Token: 0x04000035 RID: 53
		private Button button8;

		// Token: 0x04000036 RID: 54
		private Button button7;

		// Token: 0x04000037 RID: 55
		private Button button6;

		// Token: 0x04000038 RID: 56
		private Button button5;

		// Token: 0x04000039 RID: 57
		private Button button4;

		// Token: 0x0400003A RID: 58
		private Label label8;

		// Token: 0x0400003B RID: 59
		private Label label9;

		// Token: 0x0400003C RID: 60
		private DomainUpDown txtPort;

		// Token: 0x0400003D RID: 61
		private CheckBox checkBox3;

		// Token: 0x0400003E RID: 62
		private Timer timer1;

		// Token: 0x0400003F RID: 63
		private Button button9;

		// Token: 0x04000040 RID: 64
		private Button button10;

		// Token: 0x04000041 RID: 65
		private TextBox txtY;

		// Token: 0x04000042 RID: 66
		private TextBox txtX;

		// Token: 0x04000043 RID: 67
		private CheckBox checkBox2;

		// Token: 0x04000044 RID: 68
		private Label label10;

		// Token: 0x04000045 RID: 69
		private Button button11;

		// Token: 0x04000046 RID: 70
		private Button button13;

		// Token: 0x04000047 RID: 71
		private Button button12;

		// Token: 0x04000048 RID: 72
		private Button button14;

		// Token: 0x04000049 RID: 73
		private Button button15;

		// Token: 0x0400004A RID: 74
		private Label label7;

		// Token: 0x0400004B RID: 75
		private TextBox textBox1;

		// Token: 0x0400004C RID: 76
		private DataGridViewTextBoxColumn Column1;

		// Token: 0x0400004D RID: 77
		private DataGridViewTextBoxColumn Column2;

		// Token: 0x0400004E RID: 78
		private DataGridViewTextBoxColumn Column3;

		// Token: 0x0400004F RID: 79
		private DataGridViewTextBoxColumn Column4;

		// Token: 0x04000050 RID: 80
		private DataGridViewTextBoxColumn Column5;

		// Token: 0x04000051 RID: 81
		private DataGridViewTextBoxColumn Column6;

		// Token: 0x04000052 RID: 82
		private DataGridViewTextBoxColumn Column7;

		// Token: 0x04000053 RID: 83
		private DataGridViewTextBoxColumn Column8;

		// Token: 0x04000054 RID: 84
		private DataGridViewCheckBoxColumn Column9;

		// Token: 0x02000008 RID: 8
		private struct RECT
		{
			// Token: 0x0400005A RID: 90
			public int Left;

			// Token: 0x0400005B RID: 91
			public int Top;

			// Token: 0x0400005C RID: 92
			public int Right;

			// Token: 0x0400005D RID: 93
			public int Bottom;
		}

		// Token: 0x02000009 RID: 9
		// (Invoke) Token: 0x06000061 RID: 97
		private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
	}
}
using System;
using System.Windows.Forms;

namespace QLTK___SP
{
	// Token: 0x02000004 RID: 4
	internal static class Program
	{
		// Token: 0x06000054 RID: 84 RVA: 0x00005E88 File Offset: 0x00004088
		[STAThread]
		private static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new Form1());
		}
	}
}

