﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkingLotApi.Dtos;
using ParkingLotApi.Repository;
using Microsoft.EntityFrameworkCore;

namespace ParkingLotApi.Services
{
    public class parkingService
    {
        private readonly parkingDbContext parkingDbContext;

        public parkingService(parkingDbContext parkingDbContext)
        {
            this.parkingDbContext = parkingDbContext;
        }

        public async Task<List<parkingDto>> GetAll()
        {
            var Parkings = parkingDbContext.Parkings.Include(parking => parking.order).ToList();

            return Parkings.Select(parkingEntity => new parkingDto(parkingEntity)).ToList();

        }

        public async Task<int> Addparking(parkingDto parkingDto)
        {
            // convert dto to entity
            parkingEntity parkingEntity = parkingDto.ToEntity();

            // save entity to db
            await parkingDbContext.Parkings.AddAsync(parkingEntity);
            await parkingDbContext.SaveChangesAsync();

            // return parking id
            return parkingEntity.Id;
        }

        public async Task Deleteparking(int id)
        {
            var foundparking = parkingDbContext.Parkings
                .Include(parking => parking.order)
                .FirstOrDefault(_ => _.Id == id);

            parkingDbContext.Parkings.Remove(foundparking);
            await parkingDbContext.SaveChangesAsync();

        }

        public async Task<parkingDto> GetById(int id)
        {
            var foundparking = parkingDbContext.Parkings
                .Include(parking => parking.order)
                .FirstOrDefault(_ => _.Id == id);

            return new parkingDto(foundparking);

        }
    }
}