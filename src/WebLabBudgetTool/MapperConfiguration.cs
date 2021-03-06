﻿using System;
using AutoMapper;
using WebLabBudgetTool.DataTransferObjects;
using WebLabBudgetTool.Entities;

namespace WebLabBudgetTool
{
    public class MapperConfiguration
    {
        public static void Init()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Account, AccountDto>();
                cfg.CreateMap<AccountDto, Account>();

                cfg.CreateMap<Category, CategoryDto>();
                cfg.CreateMap<CategoryDto, Category>();

                cfg.CreateMap<Payment, PaymentDto>()
                   .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date.ToString("d")));

                cfg.CreateMap<PaymentDto, Payment>();
            });
        }
    }
}
