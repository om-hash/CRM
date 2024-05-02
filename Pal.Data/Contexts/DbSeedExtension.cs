using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pal.Core.Domains.Account;
using Pal.Core.Domains.Languages;
using Pal.Core.Domains.Lookups;
using Pal.Core.Domains.Lookups.CRM;
using Pal.Core.Domains.Notifications;
using Pal.Core.Domains.OurServices;
using Pal.Core.Enums.Account;
using Pal.Data.DTOs.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Pal.Data.Contexts
{

    public static class DbSeedExtension
    {

        public static void SeedData(this IApplicationBuilder applicationBuilder)
        {
            SeedLookups(applicationBuilder);
            SeedRoles(applicationBuilder);
            SeedIdentityAsync(applicationBuilder);
            SeedBasicCallTypes(applicationBuilder);
            SeedNotificationTypes(applicationBuilder);
            //SeedCompanies(applicationBuilder);
            //SeedProjects(applicationBuilder);
            //SeedRealEstate(applicationBuilder);
            //SeedContactUs(applicationBuilder);
            //SeedAboutUs(applicationBuilder);
            //SeedGeneralInfo(applicationBuilder);
            //SeedOurService(applicationBuilder);
        }

        //------------------------------------------------------------------------------------------
        private static async Task SeedIdentityAsync(IApplicationBuilder applicationBuilder)
        {
            using var scope = applicationBuilder.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

            if (userManager!.FindByNameAsync("Admin").Result == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "Admin",
                    Email = "Admin@Admin.com",
                    FullName = "Administrator",
                    FCMToken = "",
                    PhoneNumber = "0123456789",
                    WhatsappNumber = "0123456789",
                    ReferenceId = "",
                    PhoneNumber2 = "",
                    UserType = Core.Enums.Account.UserType.Admins,
                    EmailConfirmed = true,

                };
                var result = userManager!.CreateAsync(user, "Admin123!@#").Result;
                if (result.Succeeded)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(type: PalClaimType.UserType.ToString(), value: user.UserType.ToString()),
                        new Claim(type: ClaimTypes.MobilePhone.ToString(), value: user.PhoneNumber.ToString())
                    };
                    userManager.AddClaimsAsync(user, claims).Wait();
                    userManager.AddToRoleAsync(user, "SUPERADMIN").Wait();
                }
            }
            if (await userManager.FindByNameAsync("Deneme") == null)
            {
                var user1 = new ApplicationUser
                {
                    UserName = "Deneme",
                    Email = "deneme@deneme.com",
                    FullName = "Deneme",
                    FCMToken = "",
                    PhoneNumber = "0123456789",
                    WhatsappNumber = "0123456789",
                    ReferenceId = "",
                    PhoneNumber2 = "",
                    UserType = Core.Enums.Account.UserType.Unknown,
                    EmailConfirmed = true,

                };
                var result1 = userManager.CreateAsync(user1, "Deneme123!@#").Result;
                if (result1.Succeeded)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(type: PalClaimType.UserType.ToString(), value: user1.UserType.ToString()),
                        new Claim(type: ClaimTypes.MobilePhone.ToString(), value: user1.PhoneNumber.ToString())
                    };
                    userManager.AddClaimsAsync(user1, claims).Wait();
                    userManager.AddToRoleAsync(user1, "ConvertLeadToCustomer").Wait();
                }
            }
            if (userManager.FindByNameAsync("Deneme1").Result == null)
            {
                var user1 = new ApplicationUser
                {
                    UserName = "Deneme1",
                    Email = "deneme1@deneme1.com",
                    FullName = "Deneme1",
                    PhoneNumber = "0123456789",
                    FCMToken = "",
                    WhatsappNumber = "0123456789",
                    ReferenceId = "",
                    PhoneNumber2 = "",
                    UserType = Core.Enums.Account.UserType.Unknown,
                    EmailConfirmed = true,

                };
                var result1 = userManager.CreateAsync(user1, "Deneme123!@#").Result;
                if (result1.Succeeded)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(type: PalClaimType.UserType.ToString(), value: user1.UserType.ToString()),
                        new Claim(type: ClaimTypes.MobilePhone.ToString(), value: user1.PhoneNumber.ToString())
                    };
                    userManager.AddClaimsAsync(user1, claims).Wait();
                    userManager.AddToRoleAsync(user1, "ConvertLeadToCustomer").Wait();
                }
            }
            if (userManager.FindByNameAsync("Deneme2").Result == null)
            {
                var user1 = new ApplicationUser
                {
                    UserName = "Deneme2",
                    Email = "deneme2@deneme.com",
                    FullName = "Deneme2",
                    PhoneNumber = "0123456789",
                    FCMToken = "",
                    WhatsappNumber = "0123456789",
                    ReferenceId = "",
                    PhoneNumber2 = "",
                    UserType = Core.Enums.Account.UserType.Unknown,
                    EmailConfirmed = true,

                };
                var result1 = userManager.CreateAsync(user1, "Deneme123!@#").Result;
                if (result1.Succeeded)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(type: PalClaimType.UserType.ToString(), value: user1.UserType.ToString()),
                        new Claim(type: ClaimTypes.MobilePhone.ToString(), value: user1.PhoneNumber.ToString())
                    };
                    userManager.AddClaimsAsync(user1, claims).Wait();
                    userManager.AddToRoleAsync(user1, "ConvertLeadToCustomer").Wait();
                }
            }
            //if (userManager.FindByNameAsync("Company").Result == null)
            //{
            //    var user = new ApplicationUser
            //    {
            //        UserName = "Company",
            //        Email = "Company@Company.com",
            //        FullName = "Company",
            //        ReferenceId = "1",
            //        FCMToken = "",
            //        WhatsappNumber = "0123456789",
            //        PhoneNumber2 = "",
            //        UserType = Core.Enums.Account.UserType.Companies,
            //        EmailConfirmed = true,
            //    };

            //    var companyUser = userManager.CreateAsync(user, "Company123!@#").Result;
            //    if (companyUser.Succeeded)
            //    {
            //        var claims = new List<Claim>
            //        {
            //            new Claim(type: PalClaimType.UserType.ToString(), value: user.UserType.ToString()),
            //            new Claim(type: PalClaimType.CompanyId.ToString(), value: "1"),
            //            new Claim(type: ClaimTypes.MobilePhone.ToString(), value: user.PhoneNumber.ToString())
            //        };
            //        userManager.AddClaimsAsync(user, claims).Wait();
            //    }

            //}


        }
        //------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------
        private static void SeedLookups(IApplicationBuilder applicationBuilder)
        {
            using var scope = applicationBuilder.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            var ApplicationContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

            context.Database.EnsureCreated();//database is exist or not existing

            #region Languages
            if (!context.Languages.Any())
            {
                context.Languages.AddRange(
                    new Language
                    {
                        Id = 1,
                        Name = "English",
                        Shortcut = "en",
                        Culture = "en-US",
                        IsRtl = false,
                        IsDefault = true,

                    },
                    new Language
                    {
                        Id = 2,
                        Name = "العربية",
                        Shortcut = "ar",
                        Culture = "ar-SY",
                        IsRtl = true,
                        IsDefault = false,
                    });
                context.SaveChanges();

            }
            if (!ApplicationContext.Languages.Any())
            {
                ApplicationContext.Languages.AddRange(
                    new Language
                    {
                        Id = 1,
                        Name = "English",
                        Shortcut = "en",
                        Culture = "en-US",
                        IsRtl = false,
                        IsDefault = true,
                    },
                    new Language
                    {
                        Id = 2,
                        Name = "العربية",
                        Shortcut = "ar",
                        Culture = "ar-SY",
                        IsRtl = true,
                        IsDefault = false,
                    });
                ApplicationContext.SaveChanges();

            }

            #endregion

            //--------------------------------------------------------------------- Seedıng Countries
            if (!ApplicationContext.SysCountries.Any())
            {
                ApplicationContext.SysCountries.AddRange(
                     new SysCountry { Id = 1, },
                     new SysCountry { Id = 2, });
                ApplicationContext.SaveChanges();
                ApplicationContext.SysCountryTranslates.AddRange(
                    new SysCountryTranslate
                    {
                        CountryName = "Trukey",
                        LanguageId = 1,
                        CountryId = 1,

                    },
                    new SysCountryTranslate
                    {
                        CountryName = "Syria",
                        LanguageId = 1,
                        CountryId = 2,
                    },
                    new SysCountryTranslate
                    {
                        CountryName = "تركيا",
                        LanguageId = 2,
                        CountryId = 1,
                    },
                    new SysCountryTranslate
                    {
                        CountryName = "سوريا",
                        LanguageId = 2,
                        CountryId = 2,
                    });
                ApplicationContext.SaveChanges();
            }
            //---------------------------------------------------------------------end of Seedıng Countries


            //--------------------------------------------------------------------- Seeding Cities
            if (!ApplicationContext.SysCities.Any())
            {
                ApplicationContext.SysCities.AddRange(
                     new SysCity
                     { Id = 1, CountryId = 1, },
                     new SysCity
                     { Id = 2, CountryId = 1, },
                     new SysCity
                     { Id = 3, CountryId = 2, },
                     new SysCity
                     { Id = 4, CountryId = 2, });

                ApplicationContext.SaveChanges();
                ApplicationContext.SysCityTranslates.AddRange(
                    new SysCityTranslate
                    {
                        CityName = "Istanbul",
                        LanguageId = 1,
                        CityId = 1,
                    },
                    new SysCityTranslate
                    {
                        CityName = "Adana",
                        LanguageId = 1,
                        CityId = 2,
                    },
                    new SysCityTranslate
                    {
                        CityName = "Aleppo",
                        LanguageId = 1,
                        CityId = 3,
                    },
                    new SysCityTranslate
                    {
                        CityName = "Hama",
                        LanguageId = 1,
                        CityId = 4,
                    },
                    new SysCityTranslate
                    {
                        CityName = "اسطنبول",
                        LanguageId = 2,
                        CityId = 1,
                    },
                    new SysCityTranslate
                    {
                        CityName = "اضنة",
                        LanguageId = 2,
                        CityId = 2
                    },
                     new SysCityTranslate
                     {
                         CityName = "حلب",
                         LanguageId = 2,
                         CityId = 3,
                     },
                    new SysCityTranslate
                    {
                        CityName = "حماة",
                        LanguageId = 2,
                        CityId = 4,

                    });
                ApplicationContext.SaveChanges();
            }
            //---------------------------------------------------------------------end of Seeding Cities

            //---------------------------------------------------------------------start of Seeding nationonalty
            if (!ApplicationContext.SysNationalities.Any())
            {
                ApplicationContext.SysNationalities.AddRange(
                new SysNationality
                {
                    Id = 1,
                },
                new SysNationality
                {
                    Id = 2,
                });
                ApplicationContext.SaveChanges();

                ApplicationContext.SysNationalityTranslates.AddRange(
                new SysNationalityTranslate
                {
                    NationalityId = 1,
                    NationalityName = "Turkish",
                    LanguageId = 1,
                },
                new SysNationalityTranslate
                {
                    NationalityId = 1,
                    NationalityName = "تركي",
                    LanguageId = 2,
                },
                new SysNationalityTranslate
                {
                    NationalityId = 2,
                    NationalityName = "Syrian",
                    LanguageId = 1,
                },
                new SysNationalityTranslate
                {
                    NationalityId = 2,
                    NationalityName = "سوري",
                    LanguageId = 2,
                });
            }
            //---------------------------------------------------------------------end of Seeding nationonalty


            //---------------------------------------------------------------------strart of Seeding payment typs
            if (!ApplicationContext.SysPaymentTypes.Any())
            {
                ApplicationContext.SysPaymentTypes.AddRange(
                new SysPaymentType
                {
                    Id = 1,
                },
                new SysPaymentType
                {
                    Id = 2,
                });
                ApplicationContext.SaveChanges();

                ApplicationContext.SysPaymentTypeTranslates.AddRange(
                new SysPaymentTypeTranslate
                {
                    PaymentTypeId = 1,
                    PaymentTypeName = "Cash",
                    LanguageId = 1,
                },
                new SysPaymentTypeTranslate
                {
                    PaymentTypeId = 1,
                    PaymentTypeName = "كاش",
                    LanguageId = 2,
                },
                new SysPaymentTypeTranslate
                {
                    PaymentTypeId = 2,
                    PaymentTypeName = "Crdit Card",
                    LanguageId = 1,
                },
                new SysPaymentTypeTranslate
                {
                    PaymentTypeId = 2,
                    PaymentTypeName = "بطاقة مصرفية",
                    LanguageId = 2,
                });
                ApplicationContext.SaveChanges();
            }
            //---------------------------------------------------------------------end of Seeding payment typs

        }


        //---------------------------------------------------------------------start seed of NotificationTypes;

        private static void SeedNotificationTypes(IApplicationBuilder applicationBuilder)
        {
            using var scope = applicationBuilder.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            context.Database.EnsureCreated();
            if (!context.NotificationTypes.Any())
            {
                var notificationType = new List<NotificationType> {
                   new NotificationType { Id = 1},
                   new NotificationType { Id = 2},
                   new NotificationType { Id = 3},
                   new NotificationType { Id = 4},
                   new NotificationType { Id = 5},
                   new NotificationType { Id = 6},
                   new NotificationType { Id = 7},
                   new NotificationType { Id = 8},
                   new NotificationType { Id = 9},
                   new NotificationType { Id = 10},
                   new NotificationType { Id = 11},
                   new NotificationType { Id = 12},
                   new NotificationType { Id = 13},
                   new NotificationType { Id = 14},
                   new NotificationType { Id = 15},
                   new NotificationType { Id = 16},
                   new NotificationType { Id = 17},
                   new NotificationType { Id = 18},
                   new NotificationType { Id = 19},
                   new NotificationType { Id = 20},
                   new NotificationType { Id = 21},
                   new NotificationType { Id = 22},
                   new NotificationType { Id = 23},
                   new NotificationType { Id = 24},
                   new NotificationType { Id = 25},
                   new NotificationType { Id = 26},
                   new NotificationType { Id = 27},
                   new NotificationType { Id = 28},
                }.ToList();
                context.NotificationTypes.AddRange(notificationType);
                context.SaveChanges();
                var NotificationTypesTranslate = new List<NotificationTypeTranslate>
                {

                 new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 1,TypeName = "test"},
                 new NotificationTypeTranslate {LanguageId =  2, NotificationTypeId = 1,TypeName = "test"},

                 new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 2,TypeName = "{0} added a new project {1}"},
                 new NotificationTypeTranslate {LanguageId =  2, NotificationTypeId = 2,TypeName = "قامت الشركة {0} باضافة مشروع جديد {1} "},
                 new NotificationTypeTranslate {LanguageId =  3, NotificationTypeId = 2,TypeName = "{0} yeni bir proje ekledi {1}"},

                 new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 3,TypeName = "{0} made updates on project {1}"},
                 new NotificationTypeTranslate {LanguageId =  2, NotificationTypeId = 3,TypeName = "قامت الشركة {0} بتعديل المشروع {1}"},
                 new NotificationTypeTranslate {LanguageId =  3, NotificationTypeId = 3,TypeName = "{0} , {1} projesinde güncellemeler yaptı"},

                 new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 4,TypeName = "{0} added a new real estate {1}"},
                 new NotificationTypeTranslate {LanguageId =  2, NotificationTypeId = 4,TypeName = "قامت الشركة{0} باضافة عقار جديد {1}"},
                 new NotificationTypeTranslate {LanguageId =  3, NotificationTypeId = 4,TypeName = "{0} yeni bir mülk ekledi {1}"},

                 new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 5,TypeName = "{0} made updates on real estate {1}"},
                 new NotificationTypeTranslate {LanguageId =  2, NotificationTypeId = 5,TypeName = "قامت الشركة {0} بتعديل العقار {1}"},
                 new NotificationTypeTranslate {LanguageId =  3, NotificationTypeId = 5,TypeName = "{0} emlakla ilgili güncellemeler yaptı {1}"},

                 new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 6,TypeName = "{0} repeated a project {1}"},
                 new NotificationTypeTranslate {LanguageId =  2, NotificationTypeId = 6,TypeName = "قامت الشركة {0} بتكرار المشروع {1}"},
                 new NotificationTypeTranslate {LanguageId =  3, NotificationTypeId = 6,TypeName = "{0} , {1} projesini tekrarladı"},

                 new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 7,TypeName = "{0} repeated a real estate {1}"},
                 new NotificationTypeTranslate {LanguageId =  2, NotificationTypeId = 7,TypeName = "قامت الشركة {1} بتكرار العقار {1}"},
                 new NotificationTypeTranslate {LanguageId =  3, NotificationTypeId = 7,TypeName = "{0} bir mülk tekrarladı {1}"},

                 new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 8,TypeName = "Your Project {0} Has been approved By {1}"},
                 new NotificationTypeTranslate {LanguageId =  2, NotificationTypeId = 8,TypeName = "تمت الموافقة من قبل {1} على مشروعك {1}"},
                 new NotificationTypeTranslate {LanguageId =  3, NotificationTypeId = 8,TypeName = "{0} Projeniz {1} Tarafından Onaylandı"},

                 new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 9,TypeName = "Your Real Estate {0} Has been approved By {1}"},
                 new NotificationTypeTranslate {LanguageId =  2, NotificationTypeId = 9,TypeName = "تمت الموافقة من قبل {0} على عقارك {1}"},
                 new NotificationTypeTranslate {LanguageId =  3, NotificationTypeId = 9,TypeName = "{0} mülkünüz {1} Tarafından Onaylandı"},

                 new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 10,TypeName = "unfortuatly your project{0} didn't approved"},
                 new NotificationTypeTranslate {LanguageId =  2, NotificationTypeId = 10,TypeName = "مع الاسف لم يتم الموافقة على مشروعك {0}"},
                 new NotificationTypeTranslate {LanguageId =  2, NotificationTypeId = 10,TypeName = "maalesef projeniz {0} onaylanmadı"},

                 new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 11,TypeName = "unfortuatly your real estate {0} didn't approved"},
                 new NotificationTypeTranslate {LanguageId =  2, NotificationTypeId = 11,TypeName = "مع الاسف لم يتم الموافقة على عقارك {0}"},
                 new NotificationTypeTranslate {LanguageId =  3, NotificationTypeId = 11,TypeName = "maalesef mülkünüz {0} onaylanmadı"},

                 new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 12,TypeName = "there is a new Company {0} needs to virification"},
                 new NotificationTypeTranslate {LanguageId =  2, NotificationTypeId = 12,TypeName = "تم تسجيل شركة جديدة {0} يرجى مراجعة الطلب"},
                 new NotificationTypeTranslate {LanguageId =  3, NotificationTypeId = 12,TypeName = "{0} tarafından doğrulaması gereken yeni bir Şirket var"},

                 new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 13,TypeName = "{0} Added new comment"},
                 new NotificationTypeTranslate { LanguageId = 2, NotificationTypeId = 13, TypeName = "قام المستخدم {0} باضافة تعليق جديد" },
                 new NotificationTypeTranslate { LanguageId = 3, NotificationTypeId = 13, TypeName = "{0} Yeni yorum eklendi" },

                new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 14,TypeName = "You have a new Massage"},
                 new NotificationTypeTranslate { LanguageId = 2, NotificationTypeId = 14, TypeName = "لديك رسالة جديدة" },
                 new NotificationTypeTranslate { LanguageId = 3, NotificationTypeId = 14, TypeName = "Yeni bir mesajınız var" },

                 new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 15,TypeName = "There is a new tour"},
                 new NotificationTypeTranslate { LanguageId = 2, NotificationTypeId = 15, TypeName = "يوجد جولة عقارية جديدة" },
                 new NotificationTypeTranslate { LanguageId = 3, NotificationTypeId = 15, TypeName = "yeni bir tur var" },

                 new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 16,TypeName = "You got a new tour"},
                 new NotificationTypeTranslate { LanguageId = 2, NotificationTypeId = 16, TypeName = "يوجد لديك جولة عقارية جديدة" },
                 new NotificationTypeTranslate { LanguageId = 3, NotificationTypeId = 16, TypeName = "yeni bir tur aldın" },

                 new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 17,TypeName = "A tour has been approved"},
                 new NotificationTypeTranslate { LanguageId = 2, NotificationTypeId = 17, TypeName = "تمت الموافقة على جولة عقارية" },
                 new NotificationTypeTranslate { LanguageId = 3, NotificationTypeId = 17, TypeName = "Bir tur onaylandı" },

                    new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 18,TypeName = "A tour has been dis approved"},
                 new NotificationTypeTranslate { LanguageId = 2, NotificationTypeId = 18, TypeName = "لم يتم الموافقة على جولة عقارية" },
                 new NotificationTypeTranslate { LanguageId = 3, NotificationTypeId = 18, TypeName = "Bir tur reddedildi" },

                  new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 19,TypeName = "You have a new task"},
                 new NotificationTypeTranslate { LanguageId = 2, NotificationTypeId = 19, TypeName = "لديك مهمة جديدة" },
                 new NotificationTypeTranslate { LanguageId = 3, NotificationTypeId = 19, TypeName = "yeni bir görevin var" },

                    new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 20,TypeName = "You have an updated task"},
                 new NotificationTypeTranslate { LanguageId = 2, NotificationTypeId = 20, TypeName = "لديك تعديل على إحدى المهام" },
                 new NotificationTypeTranslate { LanguageId = 3, NotificationTypeId = 20, TypeName = "Güncellenmiş bir göreviniz var" },


                new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 21,TypeName = "New property added to tour"},
                 new NotificationTypeTranslate { LanguageId = 2, NotificationTypeId = 21, TypeName = "تمت اضافة عقار جديد على جولة" },
                 new NotificationTypeTranslate { LanguageId = 3, NotificationTypeId = 21, TypeName = "Tura yeni mülkünüz eklendi" },

                  new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 22,TypeName = "There is update on tour from Customer"},
                 new NotificationTypeTranslate { LanguageId = 2, NotificationTypeId = 22, TypeName = "يوجد تعديل على جولة من الزبون" },
                 new NotificationTypeTranslate { LanguageId = 3, NotificationTypeId = 22, TypeName = "Müşteriden turla ilgili güncelleme var" },


                new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 23,TypeName = "Property deleted from tour by customer"},
                 new NotificationTypeTranslate { LanguageId = 2, NotificationTypeId = 23, TypeName = "تم حذف عقار من الجولة بواسطة الزبون" },
                 new NotificationTypeTranslate { LanguageId = 3, NotificationTypeId = 23, TypeName = "Tesis müşteri tarafından turdan silindi" },


                 new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 24,TypeName = "Tour Canceled by customer"},
                 new NotificationTypeTranslate { LanguageId = 2, NotificationTypeId = 24, TypeName = "تم الغاء الجولة بواسطة العميل" },
                 new NotificationTypeTranslate { LanguageId = 3, NotificationTypeId = 24, TypeName = "Tur müşteri tarafından iptal edildi" },

                 new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 25,TypeName = "There is update on your tour please check!"},
                 new NotificationTypeTranslate { LanguageId = 2, NotificationTypeId = 25, TypeName = "يوجد تعديل على جولتك" },
                 new NotificationTypeTranslate { LanguageId = 3, NotificationTypeId = 25, TypeName = "Turunuzda güncelleme var, lütfen kontrol edin!" },


                 new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 26,TypeName = "Tour canceled please check!"},
                 new NotificationTypeTranslate { LanguageId = 2, NotificationTypeId = 26, TypeName = "تم إلغاء الجولة ، يرجى التحقق!" },
                 new NotificationTypeTranslate { LanguageId = 3, NotificationTypeId = 26, TypeName = "Tur iptal edildi lütfen kontrol edin!" },

                 new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 27,TypeName = "Your tour has been approved"},
                 new NotificationTypeTranslate { LanguageId = 2, NotificationTypeId = 27, TypeName = "تمت الموافقة على جولتك" },
                 new NotificationTypeTranslate { LanguageId = 3, NotificationTypeId = 27, TypeName = "Turunuz onaylandı" },


                 new NotificationTypeTranslate {LanguageId =  1, NotificationTypeId = 28,TypeName = "Sorry! your trou didn't approve please click here to see details"},
                 new NotificationTypeTranslate { LanguageId = 2, NotificationTypeId = 28, TypeName = "مع الاسف لم يتم الموافقة على جولتك, يرجى الضغط هنا لمعرفة السبب" },
                 new NotificationTypeTranslate { LanguageId = 3, NotificationTypeId = 28, TypeName = "Üzgünüm! Trou'nuz onaylamadı ayrıntıları görmek için lütfen buraya tıklayın" },


            };
                context.NotificationTypeTranslates.AddRange(NotificationTypesTranslate);
                context.SaveChanges();
            }
        }
        //---------------------------------------------------------------------start seed of NotificationTypes;

        //---------------------------------------------------------------------start seed of Roles;
        private static async void SeedRoles(IApplicationBuilder applicationBuilder)
        {
            using var scope = applicationBuilder.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            context.Database.EnsureCreated();
            string[] roles = {

                "SuperAdmin",
                "Region_List",
                "Region_Add",
                "Region_Edit" ,
                "Region_Remove",

                "Customer_List",
                "Customer_Add",
                "Customer_Edit",
                "Customer_Remove",
                "Customer_Note_Add",
                "Customer_Note_Update",
                "Customer_Note_Delete",
                "Customer_Delete",
                "Customer_ConvertLeadToCustomer",

                "Employee_List",
                "Employee_Add",
                "Employee_Edit",
                "Employee_Remove",

                "Lead_List",
                "Lead_Add",
                "LEAD_EDIT",
                "Lead_Remove",

		        "Deal_List",
                "Deal_Add",
                "Deal_Edit",
                "Deal_Remove",

                "Meeting_List",
                "Meeting_Add",
                "Meeting_Edit",
                "Meeting_Remove",
                
                "Call_List",
                "Call_Add",
                "Call_Edit",
                "Call_Remove",
                "Call_Details",


                "WebSiteInfo_Aboutus_Show",
                "WebSiteInfo_Aboutus_Edit",

                "WebSiteInfo_GeneralInfo_Show",
                "WebSiteInfo_GeneralInfo_Edit",

                "WebSiteInfo_Contactus_Show",
                "WebSiteInfo_Contactus_Edit",

                "Settings_Language_List",
                "Settings_Language_Add",
                "Settings_Language_Edit",
                "Settings_Language_Remove",

                "Settings_Lookup_List",
                "Settings_Lookup_Add",
                "Settings_Lookup_Edit",
                "Settings_Lookup_Remove",

                "Task_List",
                "Task_Add",
                "Task_Edit",
                "Task_Details",
                "Task_Remove",
                "Task_Manager",
                "Task_MyTasks_Scheduler",

                "Companies_List",
                "Companies_Edit" ,
                "Companies_Delete" ,
                "Companies_Add" ,
                "Companies_Approvement" ,

            };
            //string[] roles = {"SuperAdmin",
            //        "Companies_List",
            //        "Companies_Edit" ,
            //        "Companies_Delete" ,
            //        "Companies_Approvement" ,
            //        "Project_List" ,
            //        "Project_Add" ,
            //        "Project_Edit",
            //        "Project_Remove" ,
            //        "Project_Approvement" ,
            //        "Project_Repeat" ,
            //        "Region_List" ,
            //        "Region_Add" ,
            //        "Region_Edit",
            //        "Region_Remove" ,
            //        "RealEstate_List" ,
            //        "RealEstate_Add",
            //        "RealEstate_Edit",
            //        "RealEstate_Remove" ,
            //        "RealEstate_Approvement" ,
            //        "RealEstate_Repeat" ,
            //        "Customer_List" ,
            //        "Customer_Add" ,
            //        "Customer_Edit",
            //        "Customer_Remove" ,
            //        "Advior_List" ,
            //        "Advior_Add" ,
            //        "Advior_Edit",
            //        "Advior_Remove" ,
            //        "Review_Approvement" ,
            //        "Review_List" ,
            //        "WebSiteInfo_Aboutus_Show" ,
            //        "WebSiteInfo_Aboutus_Edit",
            //        "WebSiteInfo_GeneralInfo_Show" ,
            //        "WebSiteInfo_GeneralInfo_Edit" ,
            //        "WebSiteInfo_Contactus_Show" ,
            //        "WebSiteInfo_Contactus_Edit" ,
            //        "Settings_Language_List" ,
            //        "Settings_Language_Add" ,
            //        "Settings_Language_Edit",
            //        "Settings_Language_Remove" ,
            //        "Settings_Lookup_List",
            //        "Settings_Lookup_Add",
            //        "Settings_Lookup_Edit" ,
            //        "Chat",
            //        "Settings_Lookup_Remove",
            //        "Leads_List",
            //        "Lead_Add",
            //        "Call_List",
            //        "Call_Add",
            //        "Call_Edit",
            //        "Call_Remove",
            //        "Meeting_List",
            //        "Meeting_Add",
            //        "Meeting_Edit",
            //        "Meeting_Remove",
            //        "Deal_List",
            //        "Deal_Add",
            //        "Deal_Edit",
            //        "Deal_Remove",
            //        "Task_List",
            //        "Task_Add",
            //        "Task_Edit",
            //        "Task_Details",
            //        "Task_Remove",
            //        "Task_Manager",
            //        "Task_MyTasks_Scheduler",
            //        "ConvertLeadToCustomer",
            //         };
            foreach (var role in roles)
            {
                var roleStore = new RoleStore<IdentityRole>(context);

                if (!context.Roles.Any(r => r.Name == role))
                {
                    var roleToAdd = new IdentityRole(role);

                    await roleStore.SetNormalizedRoleNameAsync(roleToAdd, role.ToUpper());
                    await roleStore.CreateAsync(roleToAdd);

                }
                //if (!context.Roles.Any(x => x.Name == item))
                //{
                //    var role = new IdentityRole() { NormalizedName = item };
                //    context.Roles.Add(role);
                //    context.SaveChanges();
                //}
            }

            //---------------------------------------------------------------------end of seed of Roles;
        }

        private static void SeedBasicCallTypes(IApplicationBuilder applicationBuilder)
        {
            using var scope = applicationBuilder.ApplicationServices.CreateScope();
            var ApplicationContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

            ApplicationContext.Database.EnsureCreated();
            if (!ApplicationContext.SysCallTypes.Any())
            {

                List<SysCallType> basicCallTypes = new List<SysCallType>
                {
                    new SysCallType
                    {
                        Id = 1,
                        Translates = new List<SysCallTypeTranslate>
                        {
                            new SysCallTypeTranslate
                            {
                                CallTypeId = 1,
                                LanguageId = 1,
                                CallTypeName = "Chat"
                            },

                            new SysCallTypeTranslate
                            {
                                CallTypeId = 1,
                                LanguageId = 2,
                                CallTypeName = "محادثة"
                            },

                        }
                    },
                    new SysCallType
                    {
                        Id = 2,
                        Translates = new List<SysCallTypeTranslate>
                        {
                            new SysCallTypeTranslate
                            {
                                CallTypeId = 2,
                                LanguageId = 1,
                                CallTypeName = "Phone Call"
                            },

                            new SysCallTypeTranslate
                            {
                                CallTypeId = 2,
                                LanguageId = 2,
                                CallTypeName = "مكالمة هاتف"
                            },

                        }
                    }
                };

                ApplicationContext.SysCallTypes.AddRange(basicCallTypes);
                ApplicationContext.SaveChanges();
            }
        }
    }
}







