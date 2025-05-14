namespace CMKZ.风神界 {
    public interface I伤害发起者 { }
    public interface IEntity {
        public Number 血量 { get; set; }
        public void OnBeAttacked(I伤害发起者 X, int Y);
        public void OnDie(I伤害发起者 X);
    }
}