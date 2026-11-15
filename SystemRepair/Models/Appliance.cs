
using System;
using SystemRepair.Interfaces;

namespace SystemRepair.Models
{
    public delegate void DeviceStatusChangedHandler(
        Appliance device,
        RepairStatus oldStatus,
        RepairStatus newStatus
    );

    public abstract class Appliance : IRepairable
    {
        private readonly string _serialNumber;
        private readonly decimal _baseLaborCost;

        public string SerialNumber
        {
            get { return _serialNumber; }
        }

        public string Model { get; protected set; }

        public RepairStatus Status { get; private set; }

        public DateTime EstimatedDeliveryDate { get; private set; }

        public event DeviceStatusChangedHandler StatusChanged;

        protected Appliance(
            string serialNumber,
            string model,
            DateTime estimatedDeliveryDate,
            decimal baseLaborCost)
        {
            _serialNumber = serialNumber;
            Model = model;
            EstimatedDeliveryDate = estimatedDeliveryDate;
            _baseLaborCost = baseLaborCost;
            Status = RepairStatus.Received;
        }

        public decimal BaseLaborCost
        {
            get { return _baseLaborCost; }
        }

        public bool IsOverdue()
        {
            return DateTime.Today > EstimatedDeliveryDate
                && Status != RepairStatus.Repaired
                && Status != RepairStatus.Delivered;
        }

        public abstract decimal CalculateRepairCost();

        public void ChangeStatus(RepairStatus newStatus)
        {
            if (!Enum.IsDefined(typeof(RepairStatus), newStatus))
            {
                throw new ArgumentException("Invalid repair status.");
            }

            RepairStatus oldStatus = Status;

            if (oldStatus == newStatus)
            {
                return;
            }

            Status = newStatus;

            DeviceStatusChangedHandler handler = StatusChanged;

            if (handler != null)
            {
                handler(this, oldStatus, newStatus);
            }
        }

        public virtual string GetDetails()
        {
            return string.Format(
                "Serial: {0} | Model: {1} | Status: {2} | Delivery: {3:yyyy-MM-dd} | Cost: {4}",
                SerialNumber,
                Model,
                Status,
                EstimatedDeliveryDate,
                CalculateRepairCost()
            );
        }
    }
}
