using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pal.Core.Domains.Lookups;
using Pal.Core.Enums.Roles;
using Pal.Data.DTOs.Account;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Pal.Services.PalFunctions
{
    public static class PalFunctions
    {

        //------------------------------------------------------------------------------------
        public static List<SysDay> GetWeekDaysByCulture(string cultureName)
        {
            var culture = new CultureInfo(cultureName);
            return new List<SysDay>
            {
                new SysDay(0,culture.DateTimeFormat.GetDayName(DayOfWeek.Sunday)),
                new SysDay(1,culture.DateTimeFormat.GetDayName(DayOfWeek.Monday)),
                new SysDay(2,culture.DateTimeFormat.GetDayName(DayOfWeek.Tuesday)),
                new SysDay(3,culture.DateTimeFormat.GetDayName(DayOfWeek.Wednesday)),
                new SysDay(4,culture.DateTimeFormat.GetDayName(DayOfWeek.Thursday)),
                new SysDay(5,culture.DateTimeFormat.GetDayName(DayOfWeek.Friday)),
                new SysDay(6,culture.DateTimeFormat.GetDayName(DayOfWeek.Saturday)),
            };
        }


        //------------------------------------------------------------------------------------
        public static SelectList ToSelectList(this List<ComboboxModel> comboboxModels)
        {
            return new SelectList(comboboxModels, "Id", "Name");
        }

        //------------------------------------------------------------------------------------
        /// <summary>
        /// To generate a random string or number
        /// </summary>
        /// <param name="length"></param>
        /// <param name="OnlyNumbers"></param>
        /// <returns></returns>
        public static string GenerateRandom(int length, bool OnlyNumbers = false)
        {
            Random random = new();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmnopqrstuvwxyz";
            if (OnlyNumbers)
            {
                chars = "0123456789";
                return new string(Enumerable.Repeat(chars, length)
                    .Select(s => s[random.Next(s.Length)]).ToArray());
            }

            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        //------------------------------------------------------------------------------------
        public static List<RoleDTO> GetDefaultRoles()
        {
            var Roles = Enum.GetValues(typeof(PalRoles))
            .Cast<PalRoles>()
            .Select(a => new RoleDTO
            {
                SystemName = a.ToString().Split("_")[0],
                RoleCode = a.ToString(),
                RoleName = (a.ToString()),
            }).ToList();
            return Roles;

        }

    }
}
