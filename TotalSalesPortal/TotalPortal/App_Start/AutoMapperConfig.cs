﻿using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;


using TotalModel.Models;

using TotalDTO.Sales;
using TotalDTO.Inventories;
using TotalDTO.Commons;
using TotalDTO.Accounts;

using TotalPortal.Areas.Sales.ViewModels;
using TotalPortal.Areas.Inventories.ViewModels;
using TotalPortal.Areas.Accounts.ViewModels;


[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(TotalPortal.App_Start.AutoMapperConfig), "SetupMappings")]
namespace TotalPortal.App_Start
{
    public static class AutoMapperConfig
    {
        public static void SetupMappings()
        {
        ////////https://github.com/AutoMapper/AutoMapper/wiki/Static-and-Instance-API

            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<DeliveryAdvice, DeliveryAdviceViewModel>();
                //cfg.CreateMap<DeliveryAdviceViewModel, DeliveryAdvice>();
                cfg.CreateMap<DeliveryAdvice, DeliveryAdviceDTO>();
                cfg.CreateMap<DeliveryAdvicePrimitiveDTO, DeliveryAdvice>();
                cfg.CreateMap<DeliveryAdviceViewDetail, DeliveryAdviceDetailDTO>();
                cfg.CreateMap<DeliveryAdviceDetailDTO, DeliveryAdviceDetail>();

                cfg.CreateMap<GoodsIssue, GoodsIssueViewModel>();
                cfg.CreateMap<GoodsIssue, GoodsIssueDTO>();
                cfg.CreateMap<GoodsIssuePrimitiveDTO, GoodsIssue>();
                cfg.CreateMap<GoodsIssueViewDetail, GoodsIssueDetailDTO>();
                cfg.CreateMap<GoodsIssueDetailDTO, GoodsIssueDetail>();

                cfg.CreateMap<GoodsIssue, GoodsIssueBoxDTO>();

                cfg.CreateMap<AccountInvoice, AccountInvoiceViewModel>();
                cfg.CreateMap<AccountInvoice, AccountInvoiceDTO>();
                cfg.CreateMap<AccountInvoicePrimitiveDTO, AccountInvoice>();
                cfg.CreateMap<AccountInvoiceViewDetail, AccountInvoiceDetailDTO>();
                cfg.CreateMap<AccountInvoiceDetailDTO, AccountInvoiceDetail>();

                cfg.CreateMap<Receipt, ReceiptViewModel>();
                cfg.CreateMap<Receipt, ReceiptDTO>();
                cfg.CreateMap<ReceiptPrimitiveDTO, Receipt>();
                cfg.CreateMap<ReceiptViewDetail, ReceiptDetailDTO>();
                cfg.CreateMap<ReceiptDetailDTO, ReceiptDetail>();

                cfg.CreateMap<Employee, EmployeeBaseDTO>();
                cfg.CreateMap<Customer, CustomerBaseDTO>();
                cfg.CreateMap<Promotion, PromotionDTO>();
                cfg.CreateMap<VoidType, VoidTypeBaseDTO>();

                //cfg.CreateMap<Module, ModuleViewModel>();
                //cfg.CreateMap<ModuleDetail, ModuleDetailViewModel>();
            });



            //Mapper.CreateMap<Module, ModuleViewModel>();
            //Mapper.CreateMap<ModuleDetail, ModuleDetailViewModel>();
        }
    }
}