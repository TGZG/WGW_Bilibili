namespace CMKZ {
    public static partial class LocalStorage {
        public static RoleError _CheckError(this IEquipController X) {
            var 累计五道 = X.五道.Clone().SetNowZero();
            var 累计五行 = X.五行.Clone().SetNowZero();
            var 基类列表 = new List<string>();
            for (var i = 0; i < X.EquipList.Count; i++) {
                var 装备 = X.EquipList[i];
                if(装备 is not I可重复Equip) {
                    if (基类列表.Contains(装备.GetType().BaseType.Name)) {
                        return new RoleError {
                            IsError = true,
                            Type = RoleErrorType.装备重复,
                            装备名 = 装备.Name,
                            装备序号 = i,
                        };
                    }
                    基类列表.Add(装备.GetType().BaseType.Name);
                }
                var B = 装备.五行.当前高于(累计五行);
                if (B != -1) {
                    return new RoleError {
                        IsError = true,
                        Type = RoleErrorType.五行溢出,
                        装备名 = 装备.Name,
                        装备序号 = i,
                        五槽位 = B,
                        实际值 = 累计五行.获取角色五行(B),
                        所需值 = 装备.五行.获取装备五行(B)
                    };
                }
                累计五行.AddAllNow(装备.五变.Multiply(X.五变));
                累计五道.AddAllNow(装备.五道);
                var A = 累计五道.溢出;
                if (A != -1) {
                    return new RoleError {
                        IsError = true,
                        Type = RoleErrorType.五道溢出,
                        装备名 = 装备.Name,
                        装备序号 = i,
                        五槽位 = A,
                        实际值 = 累计五道.获取五道上限(A),
                        所需值 = 累计五道.获取角色五道(A)
                    };
                }
            }
            return new RoleError { IsError = false };
        }
        public static void Wear(this IEquipController X, IEquip Y) {
            X.EquipList.Add(Y);
            Y.Parent = X;
        }
        public static IEquip UnWear(this IEquipController X, IEquip Y) {
            X.EquipList.Remove(Y);
            Y.Parent = null;
            return Y;
        }
        public static IEquip UnWear(this IEquipController X, int Y) {
            return X.UnWear(X.EquipList[Y]);
        }
    }
    public interface IEquipController {
        public 全域值 五行 { get; }
        public 全域值 五道 { get; }
        public 全域值 五变 { get; }
        public List<IEquip> EquipList { get; }
    }
    public interface IEquip : I设定 {
        public 全域值 五行 { get; }
        public 全域值 五道 { get; }
        public 全域值 五变 { get; }
        public IEquipController Parent { get; set; }
    }
    public interface I可重复Equip {

    }
    public struct RoleError {
        public bool IsError;
        public RoleErrorType Type;
        public long 装备序号;
        public long 五槽位;
        public string 装备名;
        public double 实际值;
        public double 所需值;
        public override readonly string ToString() {
            if (IsError) {
                if (Type == RoleErrorType.五道溢出) {
                    return $"五道溢出：序号{装备序号} {装备名} 槽位{五槽位} 上限{实际值} 当前{所需值}";
                } else if (Type == RoleErrorType.五行溢出) {
                    return $"五行溢出：序号{装备序号} {装备名} 槽位{五槽位} 实际{实际值} 所需{所需值}";
                } else {
                    return $"装备重复：序号{装备序号} {装备名}";
                }
            } else {
                return "没有错误";
            }
        }
    }
    public enum RoleErrorType {
        五道溢出,
        五行溢出,
        装备重复,
    }
}