using Pal.Core.Domains.Customers;
using Pal.Core.Domains.Empolyees;
using Pal.Core.Domains.Lookups.CRM;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pal.Core.Domains.Deals
{
  public class Deal : BaseEntityWithLog<int>
  {

    public int TypeId { get; set; }

    public int LeadSourceId { get; set; }

    public long? CustomerId { get; set; }

    public int StageId { get; set; }

    public int EmployeeId { get; set; }

    public string DealName { get; set; }

    public string NextStep { get; set; }

    public string Description { get; set; }

    public float Amount { get; set; }

    public DateTime? ClosingDate { get; set; }

    /// <summary>
    /// 0.5 %  
    /// </summary>
    public float SuccessProbability { get; set; }

    #region Fathers
    [ForeignKey(nameof(LeadSourceId))]
    public virtual SysLeadSource LeadSource { get; set; }

    [ForeignKey(nameof(StageId))]
    public virtual SysDealStage DealStage { get; set; }

    [ForeignKey(nameof(TypeId))]
    public virtual SysDealType DealType { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public virtual Customer Customer { get; set; }

    [ForeignKey(nameof(EmployeeId))]

    public virtual Employee Employee { get; set; }
    #endregion


  }


}
