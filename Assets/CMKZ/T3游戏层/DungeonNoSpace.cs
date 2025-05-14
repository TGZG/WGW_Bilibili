using Microsoft.CodeAnalysis;
using System;
using System.Linq;//from XX select XX

namespace CMKZ {
    public static partial class LocalStorage {
        public static List<IDungeon> AllDungeon = new();
        public static void CreateDungeon<T>() where T : IDungeon, new() {
            AllDungeon.Add(new T());
        }
        public static void CreateDungeon(Type X) {
            AllDungeon.Add(X.创建实例() as IDungeon);
        }
        public static T GetDungeon<T>() where T : class, IDungeon {
            return AllDungeon.Find<T>();
        }
        public static IDungeon GetDungeon(Type X) {
            return AllDungeon.Find(t => t.GetType() == X);
        }
        public static void DestroyDungeon<T>() where T : IDungeon, new() {
            AllDungeon.Remove(new T());
        }
        public static void Update(this IDungeonNoSpace X, double 时间) {
            if (!X.战斗中 && X.HasPlayer()) {
                X.战斗中 = true;
                X.Spawn();
                Print($"{X.Name} 战斗开始 战场总单位数{X.Entitys.Count}");
                Print($"场上敌人：【{X.Entitys.Where(t => !t.IsPlayer()).Select(t => t.Name).ToString(t => t)}】");
            }
            if (X.战斗中) {
                //foreach (var i in X.Entitys) {
                //    Print(X.Entitys.Select(t => t.名称).ToString(t => t));
                //    i.UpdateFight();
                //    //运行到这里，报错说列表已经被修改
                //    //也就是UpdataFight里增添移动了【副本的Entitys】
                //    //不能在UpdateFight里增添移动【副本的Entitys】
                //    //应该在Foreach外单独处理副本的增添。
                //    //看一下具体是哪一行修改了副本增添。
                //    Print(X.Entitys.Select(t => t.名称).ToString(t => t));
                //}

                //区分玩家和非玩家，避免玩家死亡后，非玩家还继续攻击，导致列表修改错误。

                var 玩家 = X.Entitys.Where(t => t.IsPlayer()).ToList();
                玩家.ForEach(t => t.UpdateFight());
                var 非玩家 = X.Entitys.Where(t => !t.IsPlayer()).ToList();
                非玩家.ForEach(t => t.UpdateFight());
                //Tobo:还应该区分第三方和敌人。

                if (X.无玩家()) {
                    X.战斗中 = false;
                    X.OnDefeat();
                    X.OnFinally();
                    foreach (var i in X.EntitysActiving()) {
                        i.战斗中 = false;
                        i.OnFightFinally();
                    }
                    return;
                }
                if (X.不存在非玩家()) {
                    X.OnEnemyWiped();
                }
                if (X.不存在非玩家()) {
                    X.战斗中 = false;
                    X.OnVictory();
                    X.OnFinally();
                    foreach (var i in X.EntitysActiving()) { //必须先副本结束再角色结束
                        i.战斗中 = false;
                        i.OnFightFinally();
                    }
                    return;
                }
            }
            X.OnUpdate(时间);
        }
        public static void EnterDungeon(this IEntityNoSpace X, IDungeonNoSpace Y) {
            X.Dungeon?.Entitys.Remove(X);
            X.Dungeon = Y;
            Y.Entitys.Add(X);
        }
        public static void LeaveDungeon(this IEntityNoSpace X) {
            X.Dungeon?.Entitys.Remove(X);
            X.Dungeon = null;
        }
    }
    public interface IDungeon {

    }
    public interface IDungeonNoSpace : I设定, IDungeon {
        public List<IEntityNoSpace> Entitys { get; }
        public bool 战斗中 { get; set; }
        public void Spawn();//生成第一波敌人
        public void OnUpdate(double 时间);
        public void OnEnemyWiped();
        public void OnVictory();
        public void OnDefeat();
        public void OnFinally();
    }
    public interface IEntityNoSpace : I设定 {
        public IDungeonNoSpace Dungeon { get; set; }
        public IFaction Faction { get; }
        public bool IsAlive { get; }
        public bool 战斗中 { get; set; }//通常是基于活着。也可以基于活着且上场
        public void OnFightFinally();//战斗结束时下场
        public void UpdateFight();
    }
    public interface IFaction {
        public string Name { get; }
    }
    public class PlayerFaction : IFaction {
        public string Name => "Player";
    }
    public static partial class LocalStorage {
        public static bool IsPlayer(this IEntityNoSpace X) => X.Faction.GetType() == typeof(PlayerFaction);
        public static List<IEntityNoSpace> EntitysActiving(this IDungeonNoSpace X) => X.Entitys.Where(t => t.战斗中).ToList();
        public static bool HasPlayer(this IDungeonNoSpace X) => X.EntitysActiving().Contains(t => t.IsPlayer());
        public static bool 不存在非玩家(this IDungeonNoSpace X) => !X.EntitysActiving().Contains(t => !t.IsPlayer());
        public static bool 无玩家(this IDungeonNoSpace X) => !X.EntitysActiving().Contains(t => t.IsPlayer());
    }
}