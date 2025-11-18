namespace CMKZ.风神界 {
    public interface I伤害发起者 { }
    public interface IEntity {
        public string 图片名 { get; set; }
        public Number 血量 { get; set; }
        public void BeAttacked(I伤害发起者 X, int Y);
        public void Die(I伤害发起者 X);
    }
}