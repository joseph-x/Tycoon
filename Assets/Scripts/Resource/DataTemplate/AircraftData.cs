using UnityEngine;

namespace AirlineSimulator
{
    public enum AircraftCategory
    {

    }

    public enum AircraftClass
    {
        Small,      // 小型客机
        Medium,     // 中型客机
        Large       // 大型客机
    }

    public enum AircraftPurpose
    {
        Passenger,  // 客机
        Cargo       // 货机
    }

    /// <summary>
    /// 飞机数据 - Scriptable Object用于存储飞机模板数据
    /// </summary>
    [CreateAssetMenu(fileName = "NewAircraftData", menuName = "Airline Simulator/Aircraft Data", order = 1)]
    public class AircraftData : ScriptableObject
    {
        [Header("基本信息")]
        [SerializeField] private string _aircraftName;
        [SerializeField] private Sprite _icon;
        [SerializeField] private AircraftClass _class;
        [SerializeField] private AircraftPurpose _purpose;

        [Header("经济属性")]
        [SerializeField] private float _purchasePrice;
        [SerializeField] private float _sellPrice;
        [SerializeField] private float _maintenanceCostPerMonth;
        [SerializeField] private float _operatingCostPerHour;

        [Header("性能属性")]
        [SerializeField] private float _fuelCapacity;
        [SerializeField] private float _fuelConsumptionPerHour;
        [SerializeField] private int _passengerCapacity;
        [SerializeField] private float _speed;
        [SerializeField] private float _range; // 最大航程（公里）


        [Header("商业属性")]
        [SerializeField] private int _seats;
        [SerializeField] private int _cargoCapacity;

        // 属性访问器
        public string AircraftName => _aircraftName;
        public Sprite Icon => _icon;
        public AircraftClass AircraftClass => _class;
        public AircraftPurpose purpose => _purpose;
        public float PurchasePrice => _purchasePrice;
        public float SellPrice => _sellPrice;
        public float MaintenanceCostPerMonth => _maintenanceCostPerMonth;
        public float OperatingCostPerHour => _operatingCostPerHour;
        public float FuelCapacity => _fuelCapacity;
        public float FuelConsumptionPerHour => _fuelConsumptionPerHour;
        public int PassengerCapacity => _passengerCapacity;
        public float Speed => _speed;
        public float Range => _range;
        public int Seats => _seats;
        public int CargoCapacity => _cargoCapacity;
    }
}