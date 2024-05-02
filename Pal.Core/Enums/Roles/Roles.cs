using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Enums.Roles
{
    public enum PalRoles
    {
        SuperAdmin = 1,

        //#region Regions
        //Region_List = 2,
        //Region_Add =3,
        //Region_Edit = 4,
        //Region_Remove = 5,
        //#endregion

        #region Customers
        Customer_List = 6,
        Customer_Add = 7,
        Customer_Edit = 8,
        Customer_Remove = 9,
        Customer_Note_Add = 10,
        Customer_Note_Update = 11,
        Customer_Note_Delete = 12,
        Customer_Delete = 13,
        Customer_ConvertLeadToCustomer = 14,
        #endregion

        #region Employees
        Employee_List = 15,
        Employee_Add = 16,
        Employee_Edit = 17,
        Employee_Remove = 18,
        #endregion
        
        #region Leads
        Lead_List = 19,
        Lead_Add = 20,
        Lead_Edit = 21,
        Lead_Remove = 22,
        #endregion

        #region Deals
        Deal_List = 23,
        Deal_Add = 24,
        Deal_Edit = 25,
        Deal_Remove = 26,
        #endregion

        #region Meetings
        Meeting_List = 27,
        Meeting_Add = 28,
        Meeting_Edit = 29,
        Meeting_Remove = 30,
        #endregion

        #region AboutUs
        WebSiteInfo_Aboutus_Show = 31,
        WebSiteInfo_Aboutus_Edit = 32,
        #endregion

        #region GeneralInformation
        WebSiteInfo_GeneralInfo_Show = 33,
        WebSiteInfo_GeneralInfo_Edit = 34,
        #endregion

        #region Contactus
        WebSiteInfo_Contactus_Show = 35,
        WebSiteInfo_Contactus_Edit = 36,
        #endregion

        #region Languages
        Settings_Language_List = 37,
        Settings_Language_Add = 38,
        Settings_Language_Edit = 39,
        Settings_Language_Remove = 40,
        #endregion

        #region Lookups
        Settings_Lookup_List = 41,
        Settings_Lookup_Add = 42,
        Settings_Lookup_Edit = 43,
        Settings_Lookup_Remove = 44,
        #endregion

        #region Tasks
        Task_List = 45,
        Task_Add = 46,
        Task_Edit = 47,
        Task_Details = 48,
        Task_Remove = 49,
        Task_Manager = 50,
        Task_MyTasks_Scheduler = 51,
        #endregion

        #region Calls
        Call_List = 52,
        Call_Add = 53,
        Call_Edit = 54,
        Call_Details = 55,
        Call_Remove = 56,
        #endregion

        #region Companies
        Companies_List = 57,
        Companies_Add = 58,
        Companies_Edit = 59,
        Companies_Delete = 60,
        Companies_Approvement = 61,
        #endregion

    }
}


//#region Companies
//Companies_List =2,
//Companies_Edit = 3,
//Companies_Delete = 4,
//Companies_Approvement = 5,
//#endregion

//#region Projects
//Project_List = 6,
//Project_Add = 7,
//Project_Edit = 8,
//Project_Remove = 9,
//Project_Approvement = 10,
//Project_Repeat = 11,
//#endregion

//#region RealEstates
//RealEstate_List = 16,
//RealEstate_Add = 17,
//RealEstate_Edit = 18,
//RealEstate_Remove = 19,
//RealEstate_Approvement = 20,
//RealEstate_Repeat = 21,
//#endregion

//#region Chat
//Chat = 46,
//#endregion

//#region Adviors
//Advior_List = 26,
//Advior_Add = 27,
//Advior_Edit = 28,
//Advior_Remove = 29,
//#endregion

//#region Reviews
//Review_Approvement = 30,
//Review_List = 31,
//#endregion
