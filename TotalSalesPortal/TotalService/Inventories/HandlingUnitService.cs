﻿using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

using System.Collections.Generic;
using System.Data.Entity.Core.Objects;

using TotalModel.Models;
using TotalDTO.Inventories;
using TotalCore.Repositories.Inventories;
using TotalCore.Services.Inventories;

namespace TotalService.Inventories
{
    public class HandlingUnitService : GenericWithViewDetailService<HandlingUnit, HandlingUnitDetail, HandlingUnitViewDetail, HandlingUnitDTO, HandlingUnitPrimitiveDTO, HandlingUnitDetailDTO>, IHandlingUnitService
    {
        public HandlingUnitService(IHandlingUnitRepository handlingUnitRepository)
            : base(handlingUnitRepository, "HandlingUnitPostSaveValidate", "HandlingUnitSaveRelative", null, null, null, "GetHandlingUnitViewDetails")
        {
        }

        public new bool Save(HandlingUnitDTO dto, bool useExistingTransaction)
        {
            return base.Save(dto, true);
        }

        public override ICollection<HandlingUnitViewDetail> GetViewDetails(int handlingUnitID)
        {
            ObjectParameter[] parameters = new ObjectParameter[] { new ObjectParameter("HandlingUnitID", handlingUnitID) };
            return this.GetViewDetails(parameters);
        }

    }
}
