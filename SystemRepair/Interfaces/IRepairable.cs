
using System;

namespace SystemRepair.Interfaces
{
    public interface IRepairable
    {
        string SerialNumber { get; }

        SystemRepair.Models.RepairStatus Status { get; }

        decimal CalculateRepairCost();

        void ChangeStatus(SystemRepair.Models.RepairStatus newStatus);

        string GetDetails();
    }
}
