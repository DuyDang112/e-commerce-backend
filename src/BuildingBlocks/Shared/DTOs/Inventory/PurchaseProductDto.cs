using Shared.Enums.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DTOs.Inventory
{
    public class PurchaseProductDto
    {
        private string _itemNo;
        public string GetItemNo() => _itemNo;

        public void SetItemNo(string itemNo) => _itemNo = itemNo;
        public EDocumentType DocumentType => EDocumentType.Purchase;
        public string? DocumentNo { get; set; }
        
        public string? ExternalDocmentNo { get; set; }
        public int Quantity { get; set; }
    }
}
