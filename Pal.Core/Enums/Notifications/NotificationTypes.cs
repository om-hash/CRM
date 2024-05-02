using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pal.Core.Enums.Notifications
{
    public enum NotificationTypes : int
    {
        None = 0,
        test = 1,
        ToAdmin_CompanyAddedANewProject = 2,
        ToAdmin_CompanyUpdatedAProject = 3,
        ToAdmin_CompanyAddedANewRealEstate = 4,
        ToAdmin_CompanyUpdatedARealEstate = 5,
        ToAdmin_CompanyRepeatedAProject = 6,
        ToAdmin_CompanyRepeatedARealEstate = 7,
        ToCompany_YourProjectHasBeenApproved = 8,
        ToCompany_YourRealEstateHasBeenApproved = 9,
        ToCompany_YourProjectDidNotApproved = 10,
        ToCompany_YourRealEstateDidNotApproved = 11,
        ToAdmin_NewCompanyNeedsVerification = 12,
        ToAdmin_UserAddedNewComment = 13,
        ToAll_YouHaveANewMsg = 14,
        ToAdmin_CustomerHasRequestedATour = 15,
        ToAdvisor_YouHaveANewTourRequest = 16,
        ToAdmin_AdvisorHasApprovedATour = 17,
        ToAdmin_AdvisorHasDisApprovedATour = 18,
        ToAdmin_YouHaveANewTask=19,
        ToAdmin_YouHaveAnUpdatedTask=20,
    }
}
